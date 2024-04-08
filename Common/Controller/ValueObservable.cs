using System;

namespace Common.Controller
{
    public delegate void OnValueChangedEvent<TValue>(TValue value) where TValue : struct;

    public class ValueObservable<TValue>()
        where TValue : struct
    {
        public event OnValueChangedEvent<TValue> OnValueChanged = delegate { };

        protected TValue value;
        public TValue Value
        {
            get => value;
            set
            {
                if (!this.value.Equals(value))
                    OnValueChanged(this.value = value);
            }
        }

        public ValueObservable(TValue initialValue) : this()
        {
            value = initialValue;
        }
    }

    public class ValueObservable<TRawValue, TValue>(Func<TRawValue, TValue> converter) : ValueObservable<TValue>
        where TRawValue : struct
        where TValue : struct
    {
        public event OnValueChangedEvent<TRawValue> OnRawValueChanged = delegate { };

        protected TRawValue rawValue;
        public TRawValue RawValue
        {
            get => rawValue;
            set
            {
                if (!rawValue.Equals(value))
                {
                    OnRawValueChanged(rawValue = value);
                    Value = converter(rawValue);
                }
            }
        }

        public ValueObservable(Func<TRawValue, TValue> converter, TRawValue initialValue) : this(converter)
        {
            value = converter(rawValue = initialValue);
        }
    }
}
