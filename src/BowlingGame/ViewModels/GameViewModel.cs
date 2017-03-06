using System.Collections.Generic;

namespace BowlingGame.ViewModels
{
    public class GameViewModel
    {
        public FrameViewModel CurrentFrame { get; set; }
        public int TotalScore { get; set; }
        public List<FrameViewModel> PreviousFrames { get; set; }

        public bool? GameCompleted { get; set; }
    }
}