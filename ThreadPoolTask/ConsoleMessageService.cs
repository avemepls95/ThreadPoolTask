using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadPoolTask.Interfaces;

namespace ThreadPoolTask
{
    public class ConsoleMessageService : IMessageService
    {
        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
