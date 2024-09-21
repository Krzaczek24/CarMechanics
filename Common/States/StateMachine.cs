using System.Collections.Frozen;

namespace Common.States
{
    public readonly struct StateChangeEventArgs<TState, TSignal>
    {
        public TState NewState { get; init; }
        public TState OldState { get; init; }
        public TSignal Signal { get; init; }
    }

    public delegate void StateChangeEvent<TState, TSignal>(StateChangeEventArgs<TState, TSignal> eventArgs);

    public interface IStateMachine<TState, TSignal>
    {
        TState State { get; }
        bool SendSignal(TSignal signal);
        event StateChangeEvent<TState, TSignal> OnStateChange;
    }

    internal class StateMachine<TState, TSignal> : IStateMachine<TState, TSignal>
    {
        protected readonly FrozenDictionary<(TState state, TSignal signal), TState> stateRelations;

        public event StateChangeEvent<TState, TSignal> OnStateChange = delegate { };

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
            {
                OnStateChange(new StateChangeEventArgs<TState, TSignal>()
                {
                    OldState = State,
                    NewState = State = newState!,
                    Signal = signal
                });
            }
            return stateToChange;
        }
    }
}
