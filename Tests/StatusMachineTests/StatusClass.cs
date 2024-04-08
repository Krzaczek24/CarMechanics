namespace Tests.StatusMachineTests
{
    internal record class StatusClass
    {
        public bool Power { get; set; }
        public bool Light { get; set; }

        public static StatusClass? Undefined => null;
        public static StatusClass None => new();
        public static StatusClass PowerOnly => new() { Power = true };
        public static StatusClass LigthOnly => new() { Light = true };
        public static StatusClass Both => new() { Power = true, Light = true };
    }
}