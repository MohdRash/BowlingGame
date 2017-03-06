using BowlingGame.ViewModels;

namespace BowlingGame.Helpers
{
    public class DeferredFrames
    {
       

        public DeferredFrames()
        {
            ClearAll();
        }

        public FrameViewModel OldDeferredFrame { get; set; }
        public FrameViewModel OlderDeferredFrame { get; set; }


        public void ClearOldDeferredFrame()
        {
            OldDeferredFrame = null;
        }

        public void ClearOlderDeferredFrame()
        {
            OlderDeferredFrame = null;
        }

        public void ClearAll()
        {
            ClearOldDeferredFrame();
            ClearOlderDeferredFrame();
        }
    }
}