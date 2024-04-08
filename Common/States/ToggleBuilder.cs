using System.Collections.Generic;

namespace Common.States
{
    public interface IToggleBuilder<TSignal>
    {
        IToggleBuilder<TSignal> WithInitialState(bool initialState);
        IToggleBuilder<TSignal> WithRelation(Relations<bool, TSignal> relations);
        IToggleBuilder<TSignal> WithRelation(Relation<bool, TSignal> relation) => WithRelation([relation]);
        IToggleBuilder<TSignal> WithComparer(IEqualityComparer<(bool, TSignal)> comparer);
        IToggle<TSignal> Build();
    }

    public class ToggleBuilder<TSignal> : StateMachineBuilder<bool, TSignal>, IToggleBuilder<TSignal>
    {
        public new IToggleBuilder<TSignal> WithComparer(IEqualityComparer<(bool, TSignal)> comparer) => (IToggleBuilder<TSignal>)base.WithComparer(comparer);

        public new IToggleBuilder<TSignal> WithInitialState(bool initialState) => (IToggleBuilder<TSignal>)base.WithInitialState(initialState);

        public new IToggleBuilder<TSignal> WithRelation(Relations<bool, TSignal> relation) => (IToggleBuilder<TSignal>)base.WithRelation(relation);

        public new IToggle<TSignal> Build()
        {
            var toggle = new Toggle<TSignal>(this);
            InitialState = default!;
            Relations = [];
            return toggle;
        }
    }
}
