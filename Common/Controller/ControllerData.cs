using System;
using System.Windows;

namespace Common.Controller
{
    public delegate void StateChangedEvent(ControllerEventSource source, object value);

    public class ControllerData
    {
        public event StateChangedEvent OnStateChange = delegate { };

        public ValueObservable<bool> Connected { get; } = new();

        public SideValueObservables<(short x, short y), Vector> Thumb { get; }

        public SideValueObservables<byte, double> Trigger { get; }

        public Button Button { get; }

        public ControllerData(ControllerOptions? options = null)
        {
            Thumb = new(thumb => ComputeThumbValue(thumb, options?.Threshold?.Thumb?.Left ?? 0), (thumb) => ComputeThumbValue(thumb, options?.Threshold?.Thumb?.Right ?? 0));
            Trigger = new(trigger => ComputeTriggerValue(trigger, options?.Threshold?.Trigger?.Left ?? 0), (trigger) => ComputeTriggerValue(trigger, options?.Threshold?.Trigger?.Right ?? 0));
            Button = new();

            Connected.OnValueChanged += (value) => OnStateChange(ControllerEventSource.Connection, value);

            Thumb.Left.OnValueChanged += (value) => OnStateChange(ControllerEventSource.LeftThumb, value);
            Thumb.Right.OnValueChanged += (value) => OnStateChange(ControllerEventSource.RightThumb, value);

            Trigger.Left.OnValueChanged += (value) => OnStateChange(ControllerEventSource.LeftTrigger, value);
            Trigger.Right.OnValueChanged += (value) => OnStateChange(ControllerEventSource.RightTrigger, value);

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

            Button.LeftShoulder.OnValueChanged += (value) => OnStateChange(ControllerEventSource.LeftShoulder, value);
            Button.RightShoulder.OnValueChanged += (value) => OnStateChange(ControllerEventSource.RightShoulder, value);

            Button.LeftThumb.OnValueChanged += (value) => OnStateChange(ControllerEventSource.LeftThumbButton, value);
            Button.RightThumb.OnValueChanged += (value) => OnStateChange(ControllerEventSource.RightThumbButton, value);
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
                ControllerEventSource.LeftThumb => () => Thumb.Left.Value = value,
                ControllerEventSource.RightThumb => () => Thumb.Right.Value = value,
                _ => (Action)(() => { })
            })();
        }

        private void SetValue(ControllerEventSource property, double value)
        {
            (property switch
            {
                ControllerEventSource.LeftTrigger => () => Trigger.Left.Value = value,
                ControllerEventSource.RightTrigger => () => Trigger.Right.Value = value,
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
                ControllerEventSource.LeftShoulder => () => Button.LeftShoulder.Value = value,
                ControllerEventSource.RightShoulder => () => Button.RightShoulder.Value = value,
                ControllerEventSource.LeftThumbButton => () => Button.LeftThumb.Value = value,
                ControllerEventSource.RightThumbButton => () => Button.RightThumb.Value = value,
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

    public class Button()
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

        public ValueObservable<bool> LeftShoulder { get; } = new();
        public ValueObservable<bool> RightShoulder { get; } = new();

        public ValueObservable<bool> LeftThumb { get; } = new();
        public ValueObservable<bool> RightThumb { get; } = new();
    }
}
