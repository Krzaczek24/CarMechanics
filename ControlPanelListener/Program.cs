using Common;
using Common.States;
using UDP;

namespace ControlPanelListener
{
    internal enum Gears
    {
        Neutral,
        NeutralLeft,
        NeutralRight,
        FirstGear,
        SecondGear,
        ThirdGear,
        FourthGear,
        FifthGear,
        ReverseGear
    }

    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Reset,
    }

    //internal class Toggles
    //{
    //    private readonly Dictionary<string, bool> values = [];

    //    public bool this[string key]
    //    {
    //        get => values[key];
    //    }

    //    public void Switch(string key)
    //    {
    //        if (values.TryGetValue(key, out bool value))
    //            values[key] = !value;
    //        else
    //            values.Add(key, true);
    //    }
    //}

    internal class Program
    {
        static void Main(string[] args)
        {
            bool currentResetValue = false;
            bool currentUpValue = false;
            bool currentDownValue = false;
            bool currentLeftValue = false;
            bool currentRightValue = false;

            var stateMachine = new StateMachineBuilder<Gears>()
                .WithInitialState(Gears.Neutral)
                .WithRelation(Gears.Neutral, Direction.Left, Gears.NeutralLeft, Direction.Right)
                .WithRelation(Gears.Neutral, Direction.Right, Gears.NeutralRight, Direction.Left)
                .WithRelation(Gears.Neutral, Direction.Up, Gears.ThirdGear, Direction.Down)
                .WithRelation(Gears.Neutral, Direction.Down, Gears.FourthGear, Direction.Up)
                .WithRelation(Gears.NeutralLeft, Direction.Up, Gears.FirstGear, Direction.Down)
                .WithRelation(Gears.NeutralLeft, Direction.Down, Gears.SecondGear, Direction.Up)
                .WithRelation(Gears.NeutralRight, Direction.Up, Gears.FifthGear, Direction.Down)
                .WithRelation(Gears.NeutralRight, Direction.Down, Gears.ReverseGear, Direction.Up)
                .WithRelation(Gears.Neutral, Direction.Reset, Gears.NeutralLeft, Gears.NeutralRight, Gears.FirstGear, Gears.SecondGear, Gears.ThirdGear, Gears.FourthGear, Gears.FifthGear, Gears.ReverseGear)
                .WithEventListener((_, newState) => Console.WriteLine(newState))
                .Build();

            //var toggles = new Toggles();

            var listener = UdpListener.Create(10000, ControllerDataDto.FromString);
            listener.OnMessageReceived += (data) =>
            {
                if (data != null)
                {
                    if (!currentUpValue && data.DPad.Up)
                        stateMachine.SendSignal(Direction.Up);
                    currentUpValue = data.DPad.Up;

                    if (!currentDownValue && data.DPad.Down)
                        stateMachine.SendSignal(Direction.Down);
                    currentDownValue = data.DPad.Down;

                    if (!currentLeftValue && data.DPad.Left)
                        stateMachine.SendSignal(Direction.Left);
                    currentLeftValue = data.DPad.Left;

                    if (!currentRightValue && data.DPad.Right)
                        stateMachine.SendSignal(Direction.Right);
                    currentRightValue = data.DPad.Right;

                    if (!currentResetValue && data.Buttons.A)
                        stateMachine.SendSignal(Direction.Reset);
                    currentResetValue = data.Buttons.A;
                }
            };

            while (true)
            {
                listener.ReceiveAsync().Wait();
            }
        }
    }
}
