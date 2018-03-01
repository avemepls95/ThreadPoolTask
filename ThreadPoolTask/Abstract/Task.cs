using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoolTask.Abstract
{
    public abstract class Task
    {
        public Int32 Id { get; }

        public Task(Int32 id)
        {
            Id = id;
        }

        public abstract void Execute();
    }
}
