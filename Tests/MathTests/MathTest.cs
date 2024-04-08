namespace Tests.MathTests
{
    internal class MathTest
    {
        [Test]
        public void TestDividing()
        {
            for (double threshold = 0; threshold <= 1.0; threshold += 0.01)
            {
                for (double value = threshold; value <= 1.0; value += 0.0000001)
                {
                    double result = ComputeThresholdValue(value, threshold);
                    if (result > 1.0)
                    {
                        Assert.Fail($"V:[{value}] T:[{threshold}] R:[{result}]");
                    }
                }
            }
        }

        private static double ComputeThresholdValue(double value, double threshold) => (value - threshold) / (1 - threshold);
    }
}
