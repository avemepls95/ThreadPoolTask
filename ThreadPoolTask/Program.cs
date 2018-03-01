using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoolTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageService = new ConsoleMessageService();
            MyThreadPool threadPool = new MyThreadPool(5, messageService);

            var random = new Random();
            Int32 tasksCount = 15;
            var tasks = new MyTask[tasksCount];
            var priorities = new Priority[tasksCount];

            for (int i = 0; i < tasksCount; ++i) {
                priorities[i] = (Priority)random.Next(0, 3);
                tasks[i] = new MyTask(i + 1);
                Console.WriteLine($"Created task {i + 1} with {priorities[i].ToString()} priority");
            }

            Parallel.For(0, tasksCount, (i) => {
                threadPool.Execute(tasks[i], priorities[i]);
            });

            threadPool.Stop();
            Console.WriteLine("Stopped");
            Console.ReadKey();
        }
    }
}
