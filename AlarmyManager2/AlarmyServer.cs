using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using System.Text;

namespace AlarmyManager
{
    internal static class AlarmyServer
    {
        internal static List<ConnectionState> s_clients = new List<ConnectionState>();
        internal static TcpServer s_internalServer;
        
        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void Start(int port, ServerStartParameters parameters)
        {
            s_internalServer = new TcpServer(new AlarmyServiceProvider(parameters), port);

            SecureString CertificatePassword = GetCertificatePassword();

            bool serverStartSuccessful = s_internalServer.Start(Properties.Settings.Default.ServerCertificatePath, 
                CertificatePassword);
            if (serverStartSuccessful)
            {
                parameters.OnServerStart(s_internalServer, new EventArgs());
            }
        }

        // Stop the server.
        internal static void Stop()
        {
            s_internalServer.Stop();
        }

        internal static void OnApplicationExit(object sender, EventArgs e)
        {
            Console.Write("\n\nStopping server... ");
            Stop();
            Console.WriteLine("Server stopped. Press any key to exit or close the console.");
            Console.ReadKey(intercept: true);
        }

        /// <summary>
        /// Retrieve the certificate password from the user.
        /// </summary>
        /// <returns><see cref="SecureString"/> holding the certificate password.</returns>
        private static SecureString GetCertificatePassword()
        {
            Console.Write("Please type the certificate password: ");

            SecureString password = ReadSecureString();

            return password;
        }

        /// <summary>
        /// Reads a <see cref="SecureString"/> from the console.
        /// </summary>
        private static SecureString ReadSecureString(string overridingValue = "")
        {
            SecureString secureString = new SecureString();
            if (overridingValue.Length != 0)
            {
                foreach (char c in overridingValue)
                {
                    secureString.AppendChar(c);
                }

                return secureString;
            }

            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    secureString.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    if ((key.Key == ConsoleKey.Backspace) && (secureString.Length > 0))
                    {
                        secureString.RemoveAt(secureString.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Console.Write("\n");
                        break;
                    }
                }
            } while (true);

            return secureString;
        }

        internal static void PingClient(ConnectionState client)
        {
            MessageWrapper<PingMessage> pmw = new MessageWrapper<PingMessage>
            {
                Message = new PingMessage()
            };

            try
            {
                ClientWriteString(client, pmw.Serialize() + Consts.EOFTag);
            }
            catch (SocketException se)
            {
                s_logger.Error(se, "Failed to write to client.");
            }
        }

        internal static void TriggerAlarm(ConnectionState client, Alarm alarm) 
        {
            MessageWrapper<ShowAlarmMessage> sam = new MessageWrapper<ShowAlarmMessage>
            {
                Message = new ShowAlarmMessage(alarm)
            };

            ClientWriteString(client, sam.Serialize() + Consts.EOFTag);
        }

        internal static void PingClients()
        {
            foreach (var client in s_clients)
            {
                PingClient(client);
            }
        }

        internal static void ClientWriteString(ConnectionState client, string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            client.Write(bytes, 0, bytes.Length);
        }
    }
}
