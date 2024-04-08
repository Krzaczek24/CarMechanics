namespace Common.Controller
{
    public class ControllerOptions
    {
        public Threshold? Threshold { get; set; }
    }

    public class Threshold
    {
        public Sides? Trigger { get; set; }
        public Sides? Thumb { get; set; }
    }

    public class Sides
    {
        public double? Left { get; set; }
        public double? Right { get; set; }
    }
}
