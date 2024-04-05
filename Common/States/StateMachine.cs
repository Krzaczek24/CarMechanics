using KrzaqTools.Extensions;
using System;
using System.Collections.Frozen;

namespace Common.States
{
    public delegate void OnStateChangeEvent<T>(T oldState, T newState);

    public interface IStateMachine<T> where T : struct, Enum
    {
        T State { get; }
        bool SendSignal(object signal);
    }

    internal class StateMachine<T> : IStateMachine<T> where T : struct, Enum
    {
        private readonly FrozenDictionary<(T state, object signal), T> stateRelations;
        
        private event OnStateChangeEvent<T> OnStateChange = delegate { };

        public T State { get; private set; }

        public StateMachine(StateMachineBuilder<T> builder)
        {
            State = builder.InitialState;
            stateRelations = builder.Relations.ToFrozenDictionary();
            builder.EventListeners.ForEach(eventListener => OnStateChange += eventListener);
        }

        public bool SendSignal(object signal)
        {
            bool stateToChange = stateRelations.TryGetValue((State, signal), out T newState);
            if (stateToChange)
            {
                OnStateChange(State, newState);
                State = newState;
            }
            return stateToChange;
        }
    }
}
