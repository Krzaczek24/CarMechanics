namespace Common.States
{
    public interface IToggle<TSignal> : IStateMachine<bool, TSignal>
    {

    }

    internal class Toggle<TSignal>(ToggleBuilder<TSignal> builder) : StateMachine<bool, TSignal>(builder), IToggle<TSignal>
    {

    }
}
