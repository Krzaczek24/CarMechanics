using System.Windows;
using Common;
using Common.Controller;

namespace XboxPanel.Controller
{
    public delegate void StateChangedEvent(ControllerEventSource source, object value);

    public class ControllerData
    {
        public event StateChangedEvent OnStateChange = delegate { };

        public ValueObservable<bool> Connected { get; } = new();

        public ValueObservable<(short X, short Y), Vector> ThumbLeft { get; }

        public ValueObservable<(short X, short Y), Vector> ThumbRight { get; }

        public ValueObservable<byte, double> TriggerLeft { get; }

        public ValueObservable<byte, double> TriggerRight { get; }

        public Buttons Button { get; }

        public ControllerData(ControllerOptions? options = null)
        {
            ThumbLeft = new(thumb => ComputeThumbValue(thumb, options?.Threshold?.Thumb?.Left ?? 0));
            ThumbRight = new(thumb => ComputeThumbValue(thumb, options?.Threshold?.Thumb?.Right ?? 0));
            TriggerLeft = new(trigger => ComputeTriggerValue(trigger, options?.Threshold?.Trigger?.Left ?? 0));
            TriggerRight = new(trigger => ComputeTriggerValue(trigger, options?.Threshold?.Trigger?.Right ?? 0));
            Button = new();

            Connected.OnValueChanged += (value) => OnStateChange(ControllerEventSource.Connection, value);

            ThumbLeft.OnValueChanged += (value) => OnStateChange(ControllerEventSource.LeftThumb, value);
            ThumbRight.OnValueChanged += (value) => OnStateChange(ControllerEventSource.RightThumb, value);

            TriggerLeft.OnValueChanged += (value) => OnStateChange(ControllerEventSource.TriggerLeft, value);
            TriggerRight.OnValueChanged += (value) => OnStateChange(ControllerEventSource.TriggerRight, value);

            Button.A.OnValueChanged += (value) => OnStateChange(ControllerEventSource.A, value);
            Button.B.OnValueChanged += (value) => OnStateChange(ControllerEventSource.B, value);
            Button.X.OnValueChanged += (value) => OnStateChange(ControllerEventSource.X, value);
            Button.Y.OnValueChanged += (value) => OnStateChange(ControllerEventSource.Y, value);

            Button.DPadUp.OnValueChanged += (value) => OnStateChange(ControllerEventSource.DPadUp, value);
            Button.DPadDown.OnValueChanged += (value) => OnStateChange(ControllerEventSource.DPadDown, value);
            Button.DPadLeft.OnValueChanged += (value) => OnStateChange(ControllerEventSource.DPadLeft, value);
            Button.DPadRight.OnValueChanged += (value) => OnStateChange(ControllerEventSource.DPadRight, value);

            Button.Back.OnValueChanged += (value) => OnStateChange(ControllerEventSource.Back, value);
            Button.Start.OnValueChanged += (value) => OnStateChange(ControllerEventSource.Start, value);

            Button.ShoulderLeft.OnValueChanged += (value) => OnStateChange(ControllerEventSource.ShoulderLeft, value);
            Button.ShoulderRight.OnValueChanged += (value) => OnStateChange(ControllerEventSource.ShoulderRight, value);

            Button.ThumbLeft.OnValueChanged += (value) => OnStateChange(ControllerEventSource.ThumbLeftButton, value);
            Button.ThumbRight.OnValueChanged += (value) => OnStateChange(ControllerEventSource.ThumbRightButton, value);
        }

        public void SetValue(ControllerEventSource property, object value)
        {
            (value switch
            {
                bool button => () => SetValue(property, button),
                double trigger => () => SetValue(property, trigger),
                Vector thumb => () => SetValue(property, thumb),
                _ => (Action)(() => { })
            })();
        }

        private void SetValue(ControllerEventSource property, Vector value)
        {
            (property switch
            {
                ControllerEventSource.LeftThumb => () => ThumbLeft.Value = value,
                ControllerEventSource.RightThumb => () => ThumbRight.Value = value,
                _ => (Action)(() => { })
            })();
        }

        private void SetValue(ControllerEventSource property, double value)
        {
            (property switch
            {
                ControllerEventSource.TriggerLeft => () => TriggerLeft.Value = value,
                ControllerEventSource.TriggerRight => () => TriggerRight.Value = value,
                _ => (Action)(() => { })
            })();
        }

        private void SetValue(ControllerEventSource property, bool value)
        {
            (property switch
            {
                ControllerEventSource.Connection => () => Connected.Value = value,
                ControllerEventSource.A => () => Button.A.Value = value,
                ControllerEventSource.B => () => Button.B.Value = value,
                ControllerEventSource.X => () => Button.X.Value = value,
                ControllerEventSource.Y => () => Button.Y.Value = value,
                ControllerEventSource.DPadUp => () => Button.DPadUp.Value = value,
                ControllerEventSource.DPadDown => () => Button.DPadDown.Value = value,
                ControllerEventSource.DPadLeft => () => Button.DPadLeft.Value = value,
                ControllerEventSource.DPadRight => () => Button.DPadRight.Value = value,
                ControllerEventSource.Back => () => Button.Back.Value = value,
                ControllerEventSource.Start => () => Button.Start.Value = value,
                ControllerEventSource.ShoulderLeft => () => Button.ShoulderLeft.Value = value,
                ControllerEventSource.ShoulderRight => () => Button.ShoulderRight.Value = value,
                ControllerEventSource.ThumbLeftButton => () => Button.ThumbLeft.Value = value,
                ControllerEventSource.ThumbRightButton => () => Button.ThumbRight.Value = value,
                _ => (Action)(() => { })
            })();
        }

        private static double ComputeThresholdValue(double value, double threshold) => (value - threshold) / (1 - threshold);

        private static Vector ComputeThumbValue((short X, short Y) thumb, double threshold)
        {
            static double Scale(short axisValue) => (double)axisValue / short.MaxValue;
            var vector = new Vector(Scale(thumb.X), Scale(thumb.Y));
            if (vector.Length < threshold)
                return new();
            double length = ComputeThresholdValue(Math.Min(vector.Length, 1.0), threshold);
            vector.Normalize();
            vector *= length;
            return vector;
        }

        private static double ComputeTriggerValue(byte value, double threshold)
        {
            static double Scale(double value) => (double)value / byte.MaxValue;
            return threshold > 0 ? ComputeThresholdValue(Scale(value), threshold) : Scale(value);
        }
    }

    public class Buttons
    {
        public ValueObservable<bool> A { get; } = new();
        public ValueObservable<bool> B { get; } = new();
        public ValueObservable<bool> X { get; } = new();
        public ValueObservable<bool> Y { get; } = new();

        public ValueObservable<bool> DPadUp { get; } = new();
        public ValueObservable<bool> DPadDown { get; } = new();
        public ValueObservable<bool> DPadLeft { get; } = new();
        public ValueObservable<bool> DPadRight { get; } = new();

        public ValueObservable<bool> Back { get; } = new();
        public ValueObservable<bool> Start { get; } = new();

        public ValueObservable<bool> ShoulderLeft { get; } = new();
        public ValueObservable<bool> ShoulderRight { get; } = new();

        public ValueObservable<bool> ThumbLeft { get; } = new();
        public ValueObservable<bool> ThumbRight { get; } = new();
    }
}
