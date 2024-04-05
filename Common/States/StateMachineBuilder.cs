using KrzaqTools.Extensions;
using System;
using System.Collections.Generic;

namespace Common.States
{
    public interface IStateMachineBuilder<T> where T : struct, Enum
    {
        IStateMachineBuilder<T> WithInitialState(T initialState);
        IStateMachineBuilder<T> WithRelation(T sourceState, object signal, T targetState);        
        IStateMachineBuilder<T> WithEventListener(OnStateChangeEvent<T> eventListener);
        IStateMachine<T> Build();
    }

    public class StateMachineBuilder<T>() : IStateMachineBuilder<T> where T : struct, Enum
    {
        internal protected ICollection<OnStateChangeEvent<T>> EventListeners { get; private set; } = [];
        internal protected T InitialState { get; private set; } = default;
        internal protected Dictionary<(T state, object signal), T> Relations { get; private set; } = [];

        public IStateMachineBuilder<T> WithInitialState(T initialState)
        {
            InitialState = initialState;
            return this;
        }

        public IStateMachineBuilder<T> WithRelation(T sourceState, object signal, T targetState)
        {
            Relations.TryAdd((sourceState, signal), targetState);
            return this;
        }

        public IStateMachineBuilder<T> WithEventListener(OnStateChangeEvent<T> eventListener)
        {
            EventListeners.Add(eventListener);
            return this;
        }

        public IStateMachine<T> Build() => new StateMachine<T>(this);
    }

    public static class StateMachineBuilderExtension
    {
        public static IStateMachineBuilder<T> WithSymetricRelation<T>(this IStateMachineBuilder<T> builder, T stateA, object signal, T stateB) where T : struct, Enum
        {
            return builder
                .WithRelation(stateA, signal, stateB)
                .WithRelation(stateB, signal, stateA);
        }

        public static IStateMachineBuilder<T> WithRelation<T>(this IStateMachineBuilder<T> builder, T stateA, object aToBsignal, T stateB, object bToAsignal) where T : struct, Enum
        {
            return builder
                .WithRelation(stateA, aToBsignal, stateB)
                .WithRelation(stateB, bToAsignal, stateA);
        }

        public static IStateMachineBuilder<T> WithRelation<T>(this IStateMachineBuilder<T> builder, T targetState, object signal, params T[] sourceStates) where T : struct, Enum
        {
            if (sourceStates?.Length > 0)
                sourceStates.ForEach(sourceState => builder.WithRelation(sourceState, signal, targetState));
            else
                Enum.GetValues<T>().ForEach(sourceState => builder.WithRelation(sourceState, signal, targetState));
            return builder;
        }
    }
}
