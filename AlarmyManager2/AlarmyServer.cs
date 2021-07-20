using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Security;

namespace AlarmyManager
{
    public class AlarmyServer
    {
        public static List<ConnectionState> Clients = new List<ConnectionState>();

        private static UnifiedLogger Logger = new UnifiedLogger("AlarmyServer");

        public static void Start(int port)
        {
            Logger.EnableConsoleLogging();
            Logger.EnableEventLogLogging(EventLogger.EventLogSource.AlarmyManager);
            Logger.EnableFileLogging(SharedWriter.Writer);

            AlarmyServiceProvider AlarmyProvider = new AlarmyServiceProvider();
            TcpServer AlarmyServiceProvider = new TcpServer(AlarmyProvider, port);

            // Allow the user to type the password again in case of a mistake.
            while (true)
            {
                SecureString CertificatePassword = GetCertificatePassword();
                AlarmyServiceProvider.Start(ManagerSettings.Default.ServerCertificatePath, CertificatePassword);
                Console.WriteLine("Server stopped. Press any key to restart or close the terminal.");
                Console.ReadKey();
            }
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

        /// <summary>
        /// Sends an event to a field unit.
        /// </summary>
        /// <param name="elr"><see cref="EventLaunchRequest"/> data.</param>
        /*internal static void LaunchEvent(EventLaunchRequest elr)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].DeviceID != null)
                    if (Clients[i].DeviceID == elr.TargetDeviceID)
                    {
                        // send the event!
                        byte[] msg = Encoding.UTF8.GetBytes("[evt]" + elr.EventRecordID.ToString() + "[/evt]");
                        Clients[i].Write(msg, 0, msg.Length);
                    }
            }
        }*/
    }
}
