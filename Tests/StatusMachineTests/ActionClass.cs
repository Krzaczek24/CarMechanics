namespace Tests.StatusMachineTests
{
    internal record class ActionClass
    {
        public bool Power { get; set; }
        public bool Light { get; set; }

        public static ActionClass? TotalShutdown => null;
        public static ActionClass None => new();
        public static ActionClass PowerOnly => new() { Power = true };
        public static ActionClass LightOnly => new() { Light = true };
        public static ActionClass Both => new() { Power = true, Light = true };
    }
}
