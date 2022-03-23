using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading; 

namespace fly.log
{
    class Lumberjack
    {
        private enum LogLevel { INFO = 0, WARNING, ERROR };
        readonly private string _caller = "";

        public static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        public static LumberjackThread thread = null;
        public static Object lockObject = new Object();

        public Lumberjack(string caller)
        {
            _caller = caller;
            // TODO: Milestones
            // Milestone 1: Write to the console (done)
            // Milestone 2: Create a static thread/thread pool and send the Write requests to the thread to unblock the caller (done)

            if(thread == null)
            {
                lock(lockObject)
                {
                    if(thread == null)
                    {
                        thread = new LumberjackThread();
                        thread.StartThread();
                    }
                }
            }
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
            queue.Enqueue(output);
        }
    }
}
