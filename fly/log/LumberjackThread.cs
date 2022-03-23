using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace fly.log
{
    class LumberjackThread
    {
        readonly Thread thread;
        public LumberjackThread()
        {
            // Milestone 1: make this a thread that writes to the output (done)
            // Milestone 2: Make this an event, rather than a busy-wait
            // Milestone 3: Write to a log file
            thread = new Thread(RunThread);
        }

        public void StartThread()
        {
            thread.Start();
        }

        private void RunThread()
        {
            while (true)
            {
                string logstring;
                if (Lumberjack.queue.TryDequeue(out logstring))
                {
                    Console.Out.WriteLine(logstring);
                }
                Thread.Sleep(1);
            }

        }


    }
}
