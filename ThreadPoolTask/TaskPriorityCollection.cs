using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ThreadPoolTask.Abstract;

namespace ThreadPoolTask
{
    public class TaskPriorityCollection : IProducerConsumerCollection<KeyValuePair<Priority, Task>>
    {
        private readonly IDictionary<Priority, ConcurrentQueue<KeyValuePair<Priority, Task>>> _queues;

        public Int32 _tasksCount;
        public Int32 Count => _tasksCount;

        private const Int32 HighPriorityTasksOnNormalCount = 3;
        private Int32 _highPriorityTasksCounter = 0;

        public TaskPriorityCollection()
        {
            _queues = new Dictionary<Priority, ConcurrentQueue<KeyValuePair<Priority, Task>>> {
                { Priority.High, new ConcurrentQueue<KeyValuePair<Priority, Task>>() },
                { Priority.Normal, new ConcurrentQueue<KeyValuePair<Priority, Task>>() },
                { Priority.Low, new ConcurrentQueue<KeyValuePair<Priority, Task>>() }
            };
        }

        public bool TryAdd(KeyValuePair<Priority, Task> item)
        {
            _queues[item.Key].Enqueue(item);
            Interlocked.Increment(ref _tasksCount);
            return true;
        }

        public bool TryTake(out KeyValuePair<Priority, Task> item)
        {
            lock (_queues) {
                bool highPriorityTasksCountIsMaxOrZero = 
                    _highPriorityTasksCounter == HighPriorityTasksOnNormalCount ||
                    _queues[Priority.High].Count == 0;

                if (_queues[Priority.Normal].Count != 0 && highPriorityTasksCountIsMaxOrZero) {
                    if (_queues[Priority.Normal].TryDequeue(out item)) {
                        Interlocked.Decrement(ref _tasksCount);
                        _highPriorityTasksCounter = 0;
                        return true;
                    }
                } else if (_queues[Priority.High].Count != 0) {
                    if (_queues[Priority.High].TryDequeue(out item)) {
                        Interlocked.Decrement(ref _tasksCount);
                        Interlocked.Increment(ref _highPriorityTasksCounter);
                        return true;
                    }
                } else if (_queues[Priority.Low].Count != 0) {
                    if (_queues[Priority.Low].TryDequeue(out item)) {
                        Interlocked.Decrement(ref _tasksCount);
                        return true;
                    }
                }

                item = default(KeyValuePair<Priority, Task>);
                return false;
            }
        }

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public void CopyTo(KeyValuePair<Priority, Task>[] array, int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<Priority, Task>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<Priority, Task>[] ToArray()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
