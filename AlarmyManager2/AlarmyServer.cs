using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace AlarmyManager
{
    internal static class AlarmyServer
    {
        internal static List<ConnectionState> Clients = new List<ConnectionState>();
        internal static UnifiedLogger Logger = new UnifiedLogger("AlarmyServer");
        internal static TcpServer InternalServer;
        
        internal static void Start(int port, ServerStartParameters parameters)
        {
            Logger.EnableConsoleLogging();
            Logger.EnableEventLogLogging(EventLogger.EventLogSource.AlarmyManager);
            Logger.EnableFileLogging(SharedWriter.Writer);

            InternalServer = new TcpServer(new AlarmyServiceProvider(parameters), port);

            SecureString CertificatePassword = GetCertificatePassword();

            bool serverStartSuccessful = InternalServer.Start(Properties.Settings.Default.ServerCertificatePath, 
                CertificatePassword);
            if (serverStartSuccessful)
            {
                parameters.OnServerStart(InternalServer, new EventArgs());
            }
        }

        // Stop the server.
        internal static void Stop()
        {
            InternalServer.Stop();
        }

        internal static void OnApplicationExit(object sender, EventArgs e)
        {
            Console.Write("\nStopping server... ");
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

            byte[] pmBytes = Encoding.UTF8.GetBytes(pmw.Serialize() + Consts.EOFTag);
            client.Write(pmBytes, 0, pmBytes.Length);
        }

        internal static void TriggerAlarm(ConnectionState client, Alarm alarm) 
        {
            MessageWrapper<ShowAlarmMessage> sam = new MessageWrapper<ShowAlarmMessage>
            {
                Message = new ShowAlarmMessage(alarm)
            };

            byte[] amBytes = Encoding.UTF8.GetBytes(sam.Serialize() + Consts.EOFTag);
            client.Write(amBytes, 0, amBytes.Length);
        }

        internal static void PingClients()
        {
            foreach (var client in Clients)
            {
                PingClient(client);
            }
        }
    }
}
