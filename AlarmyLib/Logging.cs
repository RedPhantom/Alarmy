using System;
using System.Diagnostics;
using System.IO;

namespace AlarmyLib
{
    /// <summary>
    /// Provides uniform logging API.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log an entry to the logger.
        /// </summary>
        /// <param name="level">Severity of the message.</param>
        /// <param name="component">Component from which the meesage is sent.</param>
        /// <param name="format">Format String of the message to write.</param>
        /// <param name="parameters">Values that will populate the formatted message.</param>
        void Log(LoggingLevel level, string component, string format, params object[] parameters);
    }

    public enum LoggingLevel
    {
        Trace,
        Information,
        Warning,
        Error,
        Critical
    }

    public static class UnifiedLogging
    {
        /// <summary>
        /// Retrieve the appropriate <see cref="EventLogEntryType"/> per <see cref="LoggingLevel"/>.
        /// </summary>
        /// <exception cref="ArgumentException">An invalid <see cref="LoggingLevel"/> value was specified.</exception>
        /// <returns><c>null</c> when no matching type could be found, otherwise am <see cref="EventLogEntryType"/>.</returns>
        internal static EventLogEntryType? GetEventLogEntryType(LoggingLevel level)
        {
            switch (level)
            {
                case LoggingLevel.Trace:
                    return null;

                case LoggingLevel.Information:
                    return EventLogEntryType.Information;

                case LoggingLevel.Warning:
                    return EventLogEntryType.Warning;

                case LoggingLevel.Error:
                    return EventLogEntryType.Error;

                case LoggingLevel.Critical:
                    return EventLogEntryType.Error;

                default:
                    throw new ArgumentException("Invalid level " + level.ToString());
            }
        }
    }

    /// <summary>
    /// A Unified Logger that supports logging to a file, console and the Event Log.
    /// </summary>
    public class UnifiedLogger
    {
        public ConsoleLogger ConsoleLogger { get; private set; }
        public FileLogger FileLogger { get; private set; }
        public EventLogger EventLogger { get; private set; }

        private string Component = string.Empty;

        public UnifiedLogger(string component)
        {
            Component = component;
        }

        public void EnableConsoleLogging()
        {
            ConsoleLogger = new ConsoleLogger();
        }

        public void EnableFileLogging(StreamWriter writer)
        {
            FileLogger = new FileLogger(writer);
        }

        public void EnableEventLogLogging(EventLogger.EventLogSource source)
        {
            EventLogger = new EventLogger(source);
        }

        public void Log(LoggingLevel level, string format, params object[] parameters)
        {
            if (ConsoleLogger != null)
            {
                ConsoleLogger.Log(level, Component, format, parameters);
            }

            if (FileLogger != null)
            {
                FileLogger.Log(level, Component, format, parameters);
            }

            if (EventLogger != null)
            {
                EventLogger.Log(level, Component, format, parameters);
            }
        }
    }

    public class EventLogger : ILogger
    {
        public EventLogSource Source { get; private set; }

        public enum EventLogSource
        {
            Alarmy = 0,
            AlarmyLib = 1,
            AlarmyManager = 2,
            AlarmyService = 3
        }

        public EventLogger(EventLogSource source)
        {
            Source = source;
        }

        public void Log(LoggingLevel level, string component, string format, params object[] paramters)
        {
            using (EventLog eventLog = new EventLog())
            {
                eventLog.Source = GetEventSource(Source);
                EventLogEntryType? eventLogLevel = UnifiedLogging.GetEventLogEntryType(level);
                
                if (eventLogLevel != null)
                {
                    eventLog.WriteEntry(string.Format("{0} - {1}", component, string.Format(format, paramters), eventLogLevel));
                }
            }
        }

        private static string GetEventSource(EventLogSource source)
        {
            if (!EventLog.SourceExists(source.ToString()))
            {
                EventLog.CreateEventSource(source.ToString(), "Application");
            }

            return source.ToString();
        }
    }

    /// <summary>
    /// Represents a file used for loggin.
    /// </summary>
    public class LogFile
    {
        internal StreamWriter LogWriter { get; }

        public LogFile(string path)
        {
            string logName = DateTime.Now.ToString("dd-MM-yyyy HH-mm");
            
            LogWriter = new StreamWriter(Path.Combine(path, string.Format("{0}.log", logName)));
        }
    }

    /// <summary>
    /// Allows logging to the file system.
    /// </summary>
    public class FileLogger : ILogger
    {
        private StreamWriter Writer;

        public FileLogger(StreamWriter writer)
        {
            Writer = writer;
        }

        public void Log(LoggingLevel level, string component, string format, params object[] parameters)
        {
            // Using a mutex to prevent writing to the file at the same time from a different thread.
            lock (this)
            {
                Writer.WriteLine(string.Format("{0} [{1}] {2} - {3}",
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ff"),
                level.ToString(),
                component,
                string.Format(format, parameters)));
                Writer.Flush();
            }
        }
    }

    /// <summary>
    /// Allows logging to a console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(LoggingLevel level, string component, string message, params object[] parameters)
        {
            // Using a mutex to prevent writing to the console at the same time from a different thread.
            lock (this)
            {
                Console.Write(string.Format("{0} ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ff")));
                WriteLevel(level);
                Console.WriteLine(string.Format("{0} - {1}", component, string.Format(message, parameters)));
            }
        }

        private void WriteLevel(LoggingLevel level)
        {
            Console.Write("[");
            switch (level)
            {
                case LoggingLevel.Trace:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("TRACE");
                    Console.ResetColor();
                    break;

                case LoggingLevel.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("INFO");
                    Console.ResetColor();
                    break;

                case LoggingLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("WARNING");
                    Console.ResetColor();
                    break;

                case LoggingLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERROR");
                    Console.ResetColor();
                    break;

                case LoggingLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("CRITICAL");
                    Console.ResetColor();
                    break;

                default:
                    break;
            }
            Console.Write("] ");
        }
    }
}
