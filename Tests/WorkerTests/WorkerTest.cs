using Common;

namespace Tests.WorkerTests
{
    public class WorkerTests
    {
        [Test]
        public void TestFailure()
        {
            // --- Arrange ---
            var worker = new Worker(() =>
            {
                throw new NotImplementedException();
            }, 1);

            // --- Act ---
            worker.Run();
            Thread.Sleep(5);

            // --- Assert ---
            Assert.Multiple(() =>
            {
                Assert.That(worker.IsStarted, Is.EqualTo(true));
                Assert.That(worker.IsPaused, Is.EqualTo(false));
                Assert.That(worker.IsFaulted, Is.EqualTo(true));
                Assert.That(worker.IsCanceled, Is.EqualTo(false));
                Assert.That(worker.IsRunning, Is.EqualTo(false));
            });
        }

        [Test]
        public void TestCanceled()
        {
            // --- Arrange ---
            var worker = new Worker(() => { }, 1);

            // --- Act ---
            worker.Run();
            worker.Cancel().ConfigureAwait(false);
            Thread.Sleep(5);

            // --- Assert ---
            Assert.Multiple(() =>
            {
                Assert.That(worker.IsStarted, Is.EqualTo(true));
                Assert.That(worker.IsPaused, Is.EqualTo(false));
                Assert.That(worker.IsFaulted, Is.EqualTo(false));
                Assert.That(worker.IsCanceled, Is.EqualTo(true));
                Assert.That(worker.IsRunning, Is.EqualTo(false));
            });
        }

        [Test]
        public void TestPause()
        {
            // --- Arrange ---
            var worker = new Worker(() => { }, 1);

            // --- Act ---
            worker.Run();
            worker.Pause();

            // --- Assert ---
            Assert.Multiple(() =>
            {
                Assert.That(worker.IsStarted, Is.EqualTo(true));
                Assert.That(worker.IsPaused, Is.EqualTo(true));
                Assert.That(worker.IsFaulted, Is.EqualTo(false));
                Assert.That(worker.IsCanceled, Is.EqualTo(false));
                Assert.That(worker.IsRunning, Is.EqualTo(false));
            });
        }

        [Test]
        public void TestResume()
        {
            // --- Arrange ---
            var worker = new Worker(() => { }, 1);

            // --- Act ---
            worker.Run();
            worker.Pause();
            worker.Run();
            Thread.Sleep(5);

            // --- Assert ---
            Assert.Multiple(() =>
            {
                Assert.That(worker.IsStarted, Is.EqualTo(true));
                Assert.That(worker.IsPaused, Is.EqualTo(false));
                Assert.That(worker.IsFaulted, Is.EqualTo(false));
                Assert.That(worker.IsCanceled, Is.EqualTo(false));
                Assert.That(worker.IsRunning, Is.EqualTo(true));
            });
        }
    }
}