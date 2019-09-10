namespace PiCalcClient
{
    internal class PiCalcArgs
    {
        public bool Run { get; set; }
        public int? Precision { get; set; }
        public int? Number { get; set; }

        public long? Stop { get; set; }
        public bool BreakAllTasks { get; set; }
    }
}