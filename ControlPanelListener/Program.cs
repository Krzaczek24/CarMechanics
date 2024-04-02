using Common;
using UDP;

namespace ControlPanelListener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var listener = UdpListener.Create(10000, ControllerDataDto.FromString);
            listener.OnMessageReceived += Console.WriteLine;

            while (true)
            {
                listener.ReceiveAsync().ConfigureAwait(false);
                Thread.Sleep(5);
            }
        }
    }
}
