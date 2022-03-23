using System;
using System.Collections.Generic;
using System.Text;
using System.Threading; 

namespace fly.log
{
    class Lumberjack
    {
        private enum LogLevel { INFO = 0, WARNING, ERROR };
        readonly private string _caller = "";
        public Lumberjack(string caller)
        {
            _caller = caller;
            // TODO: Milestones
            // Milestone 1: Write to the console (done)
            // Milestone 2: Write to a log file
            // Milestone 3: Create a static thread/thread pool and send the Write requests to the thread to unblock the caller
        }

        public void Log(string output)
        {
            LogAtLevel(LogLevel.INFO, output);
        }

        public void Warning(string output)
        {
            LogAtLevel(LogLevel.WARNING, output);
        }

        public void Error(string output)
        {
            LogAtLevel(LogLevel.ERROR, output);
        }

        private void LogAtLevel(LogLevel level, string output)
        {
            // Time
            // Thread ID
            // Level
            // Message
            string formattedOutput = DateTime.Now.ToString("HH:mm:ss.ffffff") +
                " [" + Thread.CurrentThread.ManagedThreadId.ToString() + "]" +
                " [" + _caller + "]" +
                " [" + level.ToString() + "]: " +
                output;
            WriteToLog(formattedOutput);
        }

        private void WriteToLog(string output)
        {
            Console.Out.WriteLine(output);
        }
    }
}
