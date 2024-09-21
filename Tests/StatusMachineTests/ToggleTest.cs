using Common.States;

namespace Tests.StatusMachineTests
{
    internal class ToggleTest
    {
        private IToggle<bool> toggle;
        private (bool previous, bool current) eventParams;

        [OneTimeSetUp]
        public void Setup()
        {
            toggle = new ToggleBuilder<bool>()
                .WithInitialState(true)
                .WithRelation(Relation.Create(stateA: false, stateB: true, aToBsignal: true, bToAsignal: false))
                .Build();

            toggle.OnStateChange += (args) => eventParams = (args.OldState, args.NewState);
        }

        [Test]
        [Order(0)]
        public void TestNoRelation() => TestSendSignal(true, false, true, default);

        [Test]
        [Order(1)]
        public void TestRelation() => TestSendSignal(false, true, false, true);

        private void TestSendSignal(bool action, bool statusShouldChange, bool expectedState, bool expectedPreviousState)
        {
            // --- Act ---
            bool changed = toggle.SendSignal(action);

            // --- Assert ---
            Assert.Multiple(() =>
            {
                Assert.That(changed, Is.EqualTo(statusShouldChange));
                Assert.That(toggle.State, Is.EqualTo(expectedState));
                if (changed)
                    Assert.That(eventParams, Is.EqualTo((expectedPreviousState, expectedState)));
            });
        }
    }
}
