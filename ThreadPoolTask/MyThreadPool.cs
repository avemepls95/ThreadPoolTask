using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ThreadPoolTask.Abstract;
using ThreadPoolTask.Interfaces;

namespace ThreadPoolTask
{
    public class MyThreadPool
    {
        private bool _isStopped = false;
        private Thread[] _threads;
        private readonly BlockingCollection<KeyValuePair<Priority, Task>> _tasks;
        protected readonly IMessageService _messageService;

        public MyThreadPool(Int32 threadsCount, IMessageService messageService)
        {
            _threads = new Thread[threadsCount];
            _messageService = messageService;

            _tasks = new BlockingCollection<KeyValuePair<Priority, Task>>(new TaskPriorityCollection());

            for (int i = 0; i < threadsCount; ++i) {
                var thread = new Thread(RunThread) { Name = $"Thread {i}" };
                _threads[i] = thread;
                thread.Start();
            }
        }

        private void RunThread()
        {
            while (!_tasks.IsCompleted) {
                if (_tasks.TryTake(out var result)) {
                    var message = $"Executing {result.Value.Id} task " +
                        $"with {result.Key} priority by {Thread.CurrentThread.Name}";
                    _messageService.SendMessage(message);
                    result.Value.Execute();
                }
            }
        }

        public bool Execute(Task task, Priority priority)
        {
            if (_isStopped) {
                return false;
            }

            _tasks.Add(new KeyValuePair<Priority, Task>(priority, task));
            return true;
        }

        public void Stop()
        {
            if (_isStopped) {
                return;
            }

            _isStopped = true;
            _tasks.CompleteAdding();

            foreach (var t in _threads) {
                t.Join();
            }
        }
    }
}
