using System;

namespace Common.Controller
{
    public class SideValueObservables<TRawValue, TValue>(Func<TRawValue, TValue> leftConverter, Func<TRawValue, TValue> rightConverter)
        where TRawValue : struct
        where TValue : struct
    {
        public ValueObservable<TRawValue, TValue> Left { get; } = new(leftConverter);

        public ValueObservable<TRawValue, TValue> Right { get; } = new(rightConverter);

        public SideValueObservables(Func<TRawValue, TValue> converter) : this(converter, converter)
        {
            Left = new(converter);
            Right = new(converter);
        }
    }
}
