using Common.Controller;
using System.Windows;
using UDP;

namespace ControlPanelListener
{
    internal class Program
    {
        static void Main()
        {
            var controller = new ControllerData();
            var listener = UdpListener.Create(10000, ControllerDataSerializer.DeserializeEvent);
            listener.OnMessageReceived += (@event) =>
            {
                controller.SetValue(@event.source, @event.value);
            };
            controller.OnStateChange += (source, value) =>
            {
                if (source == ControllerEventSource.Connection && value is bool connected)
                {
                    PrintHeader(connected ? "CONNECTED" : "DISCONNECTED");
                    return;
                }

                string strValue = value switch
                {
                    bool button => button ? "pressed" : "released",
                    double trigger => $"{trigger:0.000}",
                    Vector thumb => $"X=[{thumb.X:+0.000;-0.000}] Y=[{thumb.Y:+0.000;-0.000}]",
                    _ => throw new NotImplementedException()
                };
                Console.WriteLine($"[{source}] -> ({strValue})");
            };

            while (true)
            {
                listener.ReceiveAsync().Wait();
            }
        }

        private static void PrintHeader(string text)
        {
            const int lineLength = 120;
            string line = string.Empty.PadRight(lineLength, '=');
            string textLine = text.PadLeft((lineLength + text.Length) / 2);
            Console.WriteLine(line);
            Console.WriteLine(textLine);
            Console.WriteLine(line);
        }
    }
}
