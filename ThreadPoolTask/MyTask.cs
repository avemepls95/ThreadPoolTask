using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadPoolTask.Abstract;

namespace ThreadPoolTask
{
    public class MyTask : Task
    {
        public MyTask(int id) : base(id)
        {
        }

        public override void Execute()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
}
