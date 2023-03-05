using System;
using System.Threading;
using System.Threading.Tasks;

namespace iLeif.Extensions.Multitasking
{
    public class ProcessingTask
    {
        public Task ProcessTask { get; }
        public CancellationToken CancellationToken { get; }
        public bool IsWasRunned { get; private set; } = false;
        public bool IsStopped { get; private set; } = true;
        public bool IsRunning => IsWasRunned && !IsStopped;


        private CancellationTokenSource _tokenSource;

        private Action<CancellationToken> _action;

        public ProcessingTask(Action<CancellationToken> action)
        {
            _tokenSource = new CancellationTokenSource();
            _action = action;

            var token = _tokenSource.Token;
            ProcessTask = new Task(() => { _action(token); }, token);
        }

        ~ProcessingTask()
        {
            Stop();
            _tokenSource?.Dispose();
        }

        public void Start()
        {
            if (IsWasRunned)
            {
                return;
            }

            IsWasRunned = true;
            IsStopped = false;
            ProcessTask.ContinueWith((task) => Stop());
            ProcessTask.Start();
        }

        public void Stop()
        {
            if (IsStopped)
            {
                return;
            }

            _tokenSource.Cancel();

            IsStopped = true;
        }
    }
}


