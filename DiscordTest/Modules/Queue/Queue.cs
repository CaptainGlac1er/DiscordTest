using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordTest
{
    abstract class Queue
    {
        public Queue()
        {
            if (random == null)
                random = new Random();
        }
        protected static Random random;
        protected TimeSpan delay;
        protected String name;
        protected Thread queueThread;
        protected Boolean running;
        public void Stop()
        {
            running = false;
        }
        public bool isRunning()
        {
            return running;
        }
        public void Start()
        {
            running = true;
            queueThread.Start();
        }
        public string getName()
        {
            return name;
        }
        public string getDelay()
        {
            StringBuilder builder = new StringBuilder();
            if (delay.TotalHours > 1)
                builder.Append((int)delay.TotalHours + " hr/s ");
            if (delay.Minutes > 0)
                builder.Append(delay.Minutes + " min/s ");
            return builder.ToString();
        }
    }
}
