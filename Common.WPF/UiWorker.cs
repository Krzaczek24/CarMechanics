using System;
using System.Windows.Threading;

namespace Common.WPF
{
    public class UiWorker(Action action, Dispatcher dispatcher, int interval) : Worker(action, interval)
    {
        protected override void Job(Action action) => dispatcher.Invoke(() => base.Job(action));
    }
}
