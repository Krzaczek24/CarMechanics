using Common.Extensions;

namespace Tests
{
    public class ScaleTests
    {
        [Test]
        [TestCase(short.MinValue, -1)]
        [TestCase(short.MaxValue, 1)]
        [TestCase(short.MaxValue / 2, 0.5)]
        [TestCase(0, 0)]
        public void TestScale(short value, double expectedValue)
        {
            // --- Arrange ---

            // --- Act ---
            double result = ((double)value).Scale(short.MinValue, short.MaxValue, -1, 1);

            // --- Assert ---
            Assert.That(Math.Round(result, 4), Is.EqualTo(expectedValue));
        }
    }
}
