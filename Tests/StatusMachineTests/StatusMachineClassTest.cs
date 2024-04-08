using Common.States;

namespace Tests.StatusMachineTests
{
    internal class StatusMachineClassTest
    {
        private IStateMachine<StatusClass, ActionClass> machine;
        private (StatusClass previous, StatusClass current) eventParams;

        [OneTimeSetUp]
        public void Setup()
        {
            machine = new StateMachineBuilder<StatusClass, ActionClass>()
                .WithInitialState(StatusClass.None)
                .WithRelation(Relation.Create(StatusClass.None, StatusClass.PowerOnly, ActionClass.PowerOnly, ActionClass.Both)) // bidirectional with separarated signals
                .WithRelation(Relation.Create(StatusClass.PowerOnly, StatusClass.Both, ActionClass.LightOnly, biDirectional: true)) // bidirectional with the same signal
                .WithRelation(Relation.Create(StatusClass.None, StatusClass.Both, ActionClass.Both)) // single directional
                .WithRelation(Relation.Create(StatusClass.None, ActionClass.PowerOnly, StatusClass.Both, StatusClass.PowerOnly)) // multiple sources
                .Build();

            machine.OnStateChange += (previousState, newState) => eventParams = (previousState, newState);
        }

        [Test]
        [Order(0)]
        public void TestNoRelation() => TestSendSignal(ActionClass.LightOnly, false, StatusClass.None, null!);

        [Test]
        [Order(1)]
        public void TestBiDirectionalRelation_A() => TestSendSignal(ActionClass.PowerOnly, true, StatusClass.PowerOnly, StatusClass.None);

        [Test]
        [Order(2)]
        public void TestBiDirectionalRelation_B() => TestSendSignal(ActionClass.Both, true, StatusClass.None, StatusClass.PowerOnly);

        [Test]
        [Order(3)]
        public void TestSingleRelation() => TestSendSignal(ActionClass.Both, true, StatusClass.Both, StatusClass.None);

        [Test]
        [Order(4)]
        public void TestSymetricRelation_A() => TestSendSignal(ActionClass.LightOnly, true, StatusClass.PowerOnly, StatusClass.Both);

        [Test]
        [Order(5)]
        public void TestSymetricRelation_B() => TestSendSignal(ActionClass.LightOnly, true, StatusClass.Both, StatusClass.PowerOnly);

        [Test]
        [Order(6)]
        public void TestMultipleSorcesRelation() => TestSendSignal(ActionClass.PowerOnly, true, StatusClass.None, StatusClass.Both);

        private void TestSendSignal(ActionClass action, bool statusShouldChange, StatusClass expectedState, StatusClass expectedPreviousState)
        {
            // --- Act ---
            bool changed = machine.SendSignal(action);

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
