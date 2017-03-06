namespace BowlingGame.ViewModels
{
    public class FrameViewModel
    {
        public int FrameNumber { get; set; }
        public FrameMark Mark { get; set; }
        public int? FrameScore { get; set; } // Nullable: If it is spare or strike, we may defer the calculation
        public int FirstRollKnocks { get; set; }
        public int SecondRollKnocks { get; set; }
        public int? ThirdRollKnocks { get; set; } // Nullable: Definitely, since not granted in all cases
    }

    public enum FrameMark
    {
        Strike,
        Spare,
        Open
    }
}