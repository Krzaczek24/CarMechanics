using KrzaqTools.Extensions;
using System.Collections.Generic;

namespace Common.States
{
    public interface IStateMachineBuilder<TState, TSignal>
    {
        IStateMachineBuilder<TState, TSignal> WithInitialState(TState initialState);
        IStateMachineBuilder<TState, TSignal> WithRelation(Relations<TState, TSignal> relations);
        IStateMachineBuilder<TState, TSignal> WithRelation(Relation<TState, TSignal> relation) => WithRelation([relation]);
        IStateMachineBuilder<TState, TSignal> WithComparer(IEqualityComparer<(TState, TSignal)> comparer);
        IStateMachine<TState, TSignal> Build();
    }

    public class StateMachineBuilder<TState, TSignal> : IStateMachineBuilder<TState, TSignal>
    {
        internal protected IEqualityComparer<(TState, TSignal)>? Comparer { get; set; } = null;
        internal protected TState InitialState { get; set; } = default!;
        internal protected Dictionary<(TState, TSignal), TState> Relations { get; set; } = [];

        public IStateMachineBuilder<TState, TSignal> WithInitialState(TState initialState)
        {
            InitialState = initialState;
            return this;
        }

        public IStateMachineBuilder<TState, TSignal> WithRelation(Relations<TState, TSignal> relations)
        {
            relations.ForEach(relation => Relations.TryAdd((relation.SourceState, relation.Signal), relation.TargetState));
            return this;
        }

        public IStateMachineBuilder<TState, TSignal> WithComparer(IEqualityComparer<(TState, TSignal)> comparer)
        {
            Comparer = comparer;
            return this;
        }

        public IStateMachine<TState, TSignal> Build()
        {
            var machine = new StateMachine<TState, TSignal>(this);
            InitialState = default!;
            Relations = [];
            return machine;
        }
    }
}
