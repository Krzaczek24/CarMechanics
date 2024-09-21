using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.States
{
    public class Relations<TState, TSignal>() : IEnumerable<Relation<TState, TSignal>>
    {
        internal List<Relation<TState, TSignal>> StateRelations { get; } = [];

        public IEnumerator<Relation<TState, TSignal>> GetEnumerator() => StateRelations.GetEnumerator();

        internal void Add(Relation<TState, TSignal> relation)
        {
            StateRelations.Add(relation);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Relation<TState, TSignal>
    {
        public TState SourceState { get; }
        public TState TargetState { get; }
        public TSignal Signal { get; }

        internal Relation(TState sourceState, TState targetState, TSignal signal)
        {
            SourceState = sourceState;
            TargetState = targetState;
            Signal = signal;
        }
    }

    public static class Relation
    {
        public static Relations<TState, TSignal> Create<TState, TSignal>(TState sourceState, TState targetState, TSignal signal, bool biDirectional = false)
        {
            return biDirectional ? [new(sourceState, targetState, signal), new(targetState, sourceState, signal)] : [new(sourceState, targetState, signal)];
        }

        public static Relations<TState, TSignal> Create<TState, TSignal>(TState stateA, TState stateB, TSignal aToBsignal, TSignal bToAsignal)
        {
            return [new(stateA, stateB, aToBsignal), new(stateB, stateA, bToAsignal)];
        }

        public static Relations<TState, TSignal> Create<TState, TSignal>(TState targetState, TSignal signal, params TState[] sourceStates)
        {
            return [.. sourceStates?.Select(sourceState => new Relation<TState, TSignal>(sourceState, targetState, signal))];
        }

        public static Relations<TState, TSignal> Create<TState, TSignal>(TState targetState, TSignal signal) where TState : struct, Enum
        {
            return Create(targetState, signal, Enum.GetValues<TState>());
        }
    }
}
