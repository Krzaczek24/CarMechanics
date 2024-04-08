using System.Collections.Frozen;

namespace Common.States
{
    public delegate void OnStateChangeEvent<TState>(TState previousState, TState currentState);

    public interface IStateMachine<TState, TSignal>
    {
        TState State { get; }
        bool SendSignal(TSignal signal);
        event OnStateChangeEvent<TState> OnStateChange;
    }

    internal class StateMachine<TState, TSignal> : IStateMachine<TState, TSignal>
    {
        protected readonly FrozenDictionary<(TState state, TSignal signal), TState> stateRelations;

        public event OnStateChangeEvent<TState> OnStateChange = delegate { };

        public TState State { get; protected set; }

        internal StateMachine(StateMachineBuilder<TState, TSignal> builder)
        {
            State = builder.InitialState;
            stateRelations = builder.Relations.ToFrozenDictionary(builder.Comparer);
        }

        public bool SendSignal(TSignal signal)
        {
            bool stateToChange = stateRelations.TryGetValue((State, signal), out TState? newState);
            if (stateToChange)
                OnStateChange(State, State = newState!);
            return stateToChange;
        }
    }
}
