using System;
using System.Windows;

namespace Common.Controller
{
    public static class ControllerDataSerializer
    {
        private const char dataSeparator = '|';
        private const char vectorSeparator = '#';

        private static string ToStrWithSign(double value) => value.ToString("+0.000;-0.000")[^6..];
        private static double ParseDouble(string value) => double.Parse(value);

        public static string SerializeButtonValue(bool value) => value ? "1" : "0";
        public static bool DeserializeButtonValue(char value) => value == '1';

        public static string SerializeTriggerValue(double value) => value.ToString("F3");
        public static double DeserializeTriggerValue(string value) => double.Parse(value);

        public static string SerializeThumbValue(Vector value) => ToStrWithSign(value.X) + vectorSeparator + ToStrWithSign(value.Y);
        public static Vector DeserializeThumbValue(string value)
        {
            var values = value.Split(vectorSeparator);
            return new Vector(ParseDouble(values[0]), ParseDouble(values[1]));
        }

        public static string Serialize(ControllerData data) => string.Join(dataSeparator, [
            SerializeButtonValue(data.Connected.Value),
            SerializeThumbValue(data.Thumb.Left.Value),
            SerializeThumbValue(data.Thumb.Right.Value),
            SerializeTriggerValue(data.Trigger.Left.Value),
            SerializeTriggerValue(data.Trigger.Right.Value),
            string.Join(string.Empty, [
                SerializeButtonValue(data.Button.A.Value),
                SerializeButtonValue(data.Button.B.Value),
                SerializeButtonValue(data.Button.X.Value),
                SerializeButtonValue(data.Button.Y.Value),
                SerializeButtonValue(data.Button.DPadUp.Value),
                SerializeButtonValue(data.Button.DPadDown.Value),
                SerializeButtonValue(data.Button.DPadLeft.Value),
                SerializeButtonValue(data.Button.DPadRight.Value),
                SerializeButtonValue(data.Button.Back.Value),
                SerializeButtonValue(data.Button.Start.Value),
                SerializeButtonValue(data.Button.LeftShoulder.Value),
                SerializeButtonValue(data.Button.RightShoulder.Value),
                SerializeButtonValue(data.Button.LeftThumb.Value),
                SerializeButtonValue(data.Button.RightThumb.Value)
            ])
        ]);

        public static ControllerData Deserialize(string serializedData)
        {
            var data = serializedData.Split(dataSeparator);
            var controller = new ControllerData();
            controller.Connected.Value = DeserializeButtonValue(data[0][0]);
            controller.Thumb.Left.Value = DeserializeThumbValue(data[1]);
            controller.Thumb.Right.Value = DeserializeThumbValue(data[2]);
            controller.Trigger.Left.Value = DeserializeTriggerValue(data[3]);
            controller.Trigger.Right.Value = DeserializeTriggerValue(data[4]);
            controller.Button.A.Value = DeserializeButtonValue(data[5][0]);
            controller.Button.B.Value = DeserializeButtonValue(data[5][1]);
            controller.Button.X.Value = DeserializeButtonValue(data[5][2]);
            controller.Button.Y.Value = DeserializeButtonValue(data[5][3]);
            controller.Button.DPadUp.Value = DeserializeButtonValue(data[5][4]);
            controller.Button.DPadDown.Value = DeserializeButtonValue(data[5][5]);
            controller.Button.DPadLeft.Value = DeserializeButtonValue(data[5][6]);
            controller.Button.DPadRight.Value = DeserializeButtonValue(data[5][7]);
            controller.Button.Back.Value = DeserializeButtonValue(data[5][8]);
            controller.Button.Start.Value = DeserializeButtonValue(data[5][9]);
            controller.Button.LeftShoulder.Value = DeserializeButtonValue(data[5][10]);
            controller.Button.RightShoulder.Value = DeserializeButtonValue(data[5][11]);
            controller.Button.LeftThumb.Value = DeserializeButtonValue(data[5][12]);
            controller.Button.RightThumb.Value = DeserializeButtonValue(data[5][13]);
            return controller;
        }

        public static string SerializeEvent((ControllerEventSource source, object value) @event)
        {
            return @event.source.ToString() + dataSeparator + @event.value switch
            {
                bool button => SerializeButtonValue(button),
                double trigger => SerializeTriggerValue(trigger),
                Vector thumb => SerializeThumbValue(thumb),
                _ => throw new NotImplementedException()
            };
        }

        public static (ControllerEventSource source, object value) DeserializeEvent(string @event)
        {
            var data = @event.Split(dataSeparator);
            var source = Enum.Parse<ControllerEventSource>(data[0]);
            object value = source switch
            {
                var x when ControllerEventSource.Connection.HasFlag(x) => DeserializeButtonValue(data[1][0]),
                var x when ControllerEventSource.Buttons.HasFlag(x) => DeserializeButtonValue(data[1][0]),
                var x when ControllerEventSource.Triggers.HasFlag(x) => DeserializeTriggerValue(data[1]),
                var x when ControllerEventSource.Thumbs.HasFlag(x) => DeserializeThumbValue(data[1]),
                _ => throw new NotImplementedException($"Source=[{source}] Data=[{data[1]}]")
            };
            return (source, value);
        }
    }
}
