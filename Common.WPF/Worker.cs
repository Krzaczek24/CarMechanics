using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Common.WPF
{
    public class Worker : IDisposable
    {
        public Exception? Exception => task.Exception;

        public bool IsStarted { get; private set; } = false;
        public bool IsPaused { get; private set; } = false;
        public bool IsFaulted => task.IsFaulted;
        public bool IsCanceled => task.IsCanceled;
        public bool IsRunning => IsStarted && !IsPaused && !IsFaulted && !IsCanceled;

        private bool disposed;
        private readonly Task task;
        private readonly CancellationTokenSource cts;

        public Worker(Action action, int interval)
        {
            cts = new CancellationTokenSource();
            task = new Task(() =>
            {
                while (!task!.IsFaulted)
                {
                    if (!IsPaused)
                    {
                        Job(action);
                    }
                    Task.Delay(interval);
                }
            }, cts.Token);
        }

        protected virtual void Job(Action action)
        {
            cts.Token.ThrowIfCancellationRequested();
            action();
        }

        public void Run()
        {
            if (IsStarted)
            {
                IsPaused = false;
                return;
            }

            task.Start();
            IsStarted = true;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public async Task Cancel()
        {
            cts.Cancel();
            await task;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (IsStarted)
                {
                    Cancel().ConfigureAwait(false);
                }

                task.Dispose();
                cts.Dispose();

                disposed = true;
            }
        }
    }

    public class UiWorker(Action action, Dispatcher dispatcher, int interval) : Worker(action, interval)
    {
        protected override void Job(Action action) => dispatcher.Invoke(() => base.Job(action));
    }
}
