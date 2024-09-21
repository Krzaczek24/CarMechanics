using Common.States;

namespace Tests.StatusMachineTests
{
    internal class StatusMachineStructTest
    {
        private IStateMachine<StatusEnum, ActionEnum> machine;
        private (StatusEnum previous, StatusEnum current) eventParams;

        [OneTimeSetUp]
        public void Setup()
        {
            machine = new StateMachineBuilder<StatusEnum, ActionEnum>()
                .WithInitialState(StatusEnum.PowerOff_LightOff)
                .WithRelation(Relation.Create(StatusEnum.PowerOff_LightOff, StatusEnum.PowerOn_LightOff, ActionEnum.PowerSwitch, ActionEnum.BothSwitches)) // bidirectional with separarated signals
                .WithRelation(Relation.Create(StatusEnum.PowerOn_LightOff, StatusEnum.PowerOn_LightOn, ActionEnum.LightSwitch, biDirectional: true)) // bidirectional with the same signal
                .WithRelation(Relation.Create(StatusEnum.PowerOff_LightOff, StatusEnum.PowerOn_LightOn, ActionEnum.BothSwitches)) // single directional
                .WithRelation(Relation.Create(StatusEnum.PowerOff_LightOff, ActionEnum.PowerSwitch, StatusEnum.PowerOn_LightOn, StatusEnum.PowerOn_LightOff)) // multiple sources
                .WithRelation(Relation.Create(StatusEnum.Undefined, ActionEnum.TotalShutdown)) // no sources
                .Build();

            machine.OnStateChange += (args) => eventParams = (args.OldState, args.NewState);
        }

        [Test]
        [Order(0)]
        public void TestNoRelation() => TestSendSignal(ActionEnum.LightSwitch, false, StatusEnum.PowerOff_LightOff, default);

        [Test]
        [Order(1)]
        public void TestBiDirectionalRelation_A() => TestSendSignal(ActionEnum.PowerSwitch, true, StatusEnum.PowerOn_LightOff, StatusEnum.PowerOff_LightOff);

        [Test]
        [Order(2)]
        public void TestBiDirectionalRelation_B() => TestSendSignal(ActionEnum.BothSwitches, true, StatusEnum.PowerOff_LightOff, StatusEnum.PowerOn_LightOff);

        [Test]
        [Order(3)]
        public void TestSingleRelation() => TestSendSignal(ActionEnum.BothSwitches, true, StatusEnum.PowerOn_LightOn, StatusEnum.PowerOff_LightOff);

        [Test]
        [Order(4)]
        public void TestSymetricRelation_A() => TestSendSignal(ActionEnum.LightSwitch, true, StatusEnum.PowerOn_LightOff, StatusEnum.PowerOn_LightOn);

        [Test]
        [Order(5)]
        public void TestSymetricRelation_B() => TestSendSignal(ActionEnum.LightSwitch, true, StatusEnum.PowerOn_LightOn, StatusEnum.PowerOn_LightOff);

        [Test]
        [Order(6)]
        public void TestMultipleSorcesRelation() => TestSendSignal(ActionEnum.PowerSwitch, true, StatusEnum.PowerOff_LightOff, StatusEnum.PowerOn_LightOn);

        [Test]
        [Order(7)]
        public void TestNoSorcesRelation() => TestSendSignal(ActionEnum.TotalShutdown, true, StatusEnum.Undefined, StatusEnum.PowerOff_LightOff);

        private void TestSendSignal(ActionEnum direction, bool statusShouldChange, StatusEnum expectedState, StatusEnum expectedPreviousState)
        {
            // --- Act ---
            bool changed = machine.SendSignal(direction);

            // --- Assert ---
            Assert.Multiple(() =>
            {
                Assert.That(changed, Is.EqualTo(statusShouldChange));
                Assert.That(machine.State, Is.EqualTo(expectedState));
                if (changed)
                    Assert.That(eventParams, Is.EqualTo((expectedPreviousState, expectedState)));
            });
        }
    }
}
