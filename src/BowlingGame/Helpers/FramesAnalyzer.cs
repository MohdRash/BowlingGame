using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowlingGame.ViewModels;

namespace BowlingGame.Helpers
{
    public static class FramesAnalyzer
    {
        public static void DetermineCurrentFrameType(FrameViewModel vmCurrentFrame)
        {
            if (vmCurrentFrame.FirstRollKnocks == 10)
                vmCurrentFrame.Mark = FrameMark.Strike;
            else if (vmCurrentFrame.FirstRollKnocks + vmCurrentFrame.SecondRollKnocks == 10)
                vmCurrentFrame.Mark = FrameMark.Spare;
            else
                vmCurrentFrame.Mark = FrameMark.Open;
        }

        public static int CalculateStrikeFrameScoreFromStrikeAndStrike()
        {
            return 10 + 10 + 10; // strike + strike + strike ; )
        }

        public static int CalculateSpareFrameFromStrikeFrame(GameViewModel vm)
        {
            return 10 + 10; // spare & strike scores
        }

        public static int CalculateStrikeFrameScoreFromStrikeAndSpareFrames(GameViewModel vm)
        {
            return 10 + 10 + vm.CurrentFrame.FirstRollKnocks;
        }

        public static int CalculateSpareFrameFromSpareFrame(GameViewModel vm)
        {
            return 10 + vm.CurrentFrame.FirstRollKnocks;
        }

        public static int CalculateStrikeFrameScorefromStrikeAndOpenFrames(GameViewModel vm)
        {
            return 10 + 10 + vm.CurrentFrame.FirstRollKnocks;
        }

        public static int CalculateStrikeFrameScoreFromOpenFrame(GameViewModel vm)
        {
            return 10 + vm.CurrentFrame.SecondRollKnocks + vm.CurrentFrame.FirstRollKnocks;
        }

        public static int CalculateSpareFrameScoreFromOpenFrame(GameViewModel vm)
        {
            // In the case of the current frame is open, we sum 10 to the value of the first roll of the current frame
            return 10 + vm.CurrentFrame.FirstRollKnocks;
        }

        public static int CalculateOpenFrameScore(GameViewModel vm)
        {
            // This is how we calculate open frames, just sum directly
            //return vm.CurrentFrame.ThirdRollKnocks != null
            //    ? (vm.CurrentFrame.FirstRollKnocks
            //       + vm.CurrentFrame.SecondRollKnocks + (int) vm.CurrentFrame.ThirdRollKnocks)
            //    : (vm.CurrentFrame.FirstRollKnocks + vm.CurrentFrame.SecondRollKnocks);

            return vm.CurrentFrame.FirstRollKnocks + vm.CurrentFrame.SecondRollKnocks;
        }

        public static void ProcessScores(GameViewModel vm, DeferredFrames _df)
        {
            // This logic is somehow complex, kindly refer to ProcessScores.docx for extra insights

            if ((vm.CurrentFrame.Mark == FrameMark.Open) &&
                (_df.OldDeferredFrame == null) &&
                (_df.OlderDeferredFrame == null)) // #1
            {
                vm.CurrentFrame.FrameScore = FramesAnalyzer.CalculateOpenFrameScore(vm);
                vm.TotalScore += (int)vm.CurrentFrame.FrameScore;
                vm.PreviousFrames.Add(vm.CurrentFrame);
                return;
            }
            if ((_df.OldDeferredFrame != null) && (vm.CurrentFrame.Mark == FrameMark.Open) &&
                (_df.OldDeferredFrame.Mark == FrameMark.Spare) && (_df.OlderDeferredFrame == null)) // #4
            {
                vm.CurrentFrame.FrameScore = FramesAnalyzer.CalculateOpenFrameScore(vm);
                _df.OldDeferredFrame.FrameScore = FramesAnalyzer.CalculateSpareFrameScoreFromOpenFrame(vm);
                vm.TotalScore += (int)vm.CurrentFrame.FrameScore;
                vm.TotalScore += (int)_df.OldDeferredFrame.FrameScore;
                vm.PreviousFrames.Add(_df.OldDeferredFrame);
                vm.PreviousFrames.Add(vm.CurrentFrame);
                _df.ClearOldDeferredFrame();
                return;
            }
            //if (vm.CurrentFrame.Mark == FrameMark.Open &&
            //    _df.OldDeferredFrame.Mark == FrameMark.Spare &&
            //    _df.OlderDeferredFrame.Mark == FrameMark.Strike) //#6
            //{

            //    vm.CurrentFrame.FrameScore = calculateOpenFrameScore(vm);
            //    _df.OldDeferredFrame.FrameScore = calculateSpareFrameScoreFromOpenFrame(vm);
            //    _df.OlderDeferredFrame.FrameScore = calculateStrikeFrameScoreFromOpenAndSpareFrames(vm);
            //    vm.PreviousFrames.Add(_df.OlderDeferredFrame);
            //    vm.PreviousFrames.Add(_df.OldDeferredFrame);
            //    vm.PreviousFrames.Add(vm.CurrentFrame);
            //    _df.ClearAll();

            //}
            if ((_df.OldDeferredFrame != null) && (vm.CurrentFrame.Mark == FrameMark.Open) &&
                (_df.OldDeferredFrame.Mark == FrameMark.Strike) && (_df.OlderDeferredFrame == null)) //#7
            {
                vm.CurrentFrame.FrameScore = FramesAnalyzer.CalculateOpenFrameScore(vm);
                _df.OldDeferredFrame.FrameScore = FramesAnalyzer.CalculateStrikeFrameScoreFromOpenFrame(vm);
                vm.TotalScore += (int)vm.CurrentFrame.FrameScore;
                vm.TotalScore += (int)_df.OldDeferredFrame.FrameScore;
                vm.PreviousFrames.Add(_df.OldDeferredFrame);
                vm.PreviousFrames.Add(vm.CurrentFrame);
                _df.ClearOldDeferredFrame();
                return;
            }
            if ((_df.OlderDeferredFrame != null) && (_df.OldDeferredFrame != null) &&
                (vm.CurrentFrame.Mark == FrameMark.Open) && (_df.OldDeferredFrame.Mark == FrameMark.Strike) &&
                (_df.OlderDeferredFrame.Mark == FrameMark.Strike)) //#9
            {
                vm.CurrentFrame.FrameScore = FramesAnalyzer.CalculateOpenFrameScore(vm);
                _df.OldDeferredFrame.FrameScore = FramesAnalyzer.CalculateStrikeFrameScoreFromOpenFrame(vm);
                _df.OlderDeferredFrame.FrameScore = FramesAnalyzer.CalculateStrikeFrameScorefromStrikeAndOpenFrames(vm);
                vm.TotalScore += (int)vm.CurrentFrame.FrameScore;
                vm.TotalScore += (int)_df.OldDeferredFrame.FrameScore;
                vm.TotalScore += (int)_df.OlderDeferredFrame.FrameScore;
                vm.PreviousFrames.Add(_df.OlderDeferredFrame);
                vm.PreviousFrames.Add(_df.OldDeferredFrame);
                vm.PreviousFrames.Add(vm.CurrentFrame);
                _df.ClearAll();
                return;
            }
            if ((vm.CurrentFrame.Mark == FrameMark.Spare) &&
                (_df.OldDeferredFrame == null) &&
                (_df.OlderDeferredFrame == null))
            {
                // #10


                _df.OlderDeferredFrame = _df.OldDeferredFrame;
                _df.OldDeferredFrame = vm.CurrentFrame;
                _df.ClearOlderDeferredFrame();
                return;
            }
            if ((_df.OldDeferredFrame != null) && (vm.CurrentFrame.Mark == FrameMark.Spare) &&
                (_df.OldDeferredFrame.Mark == FrameMark.Spare) && (_df.OlderDeferredFrame == null))
            {
                // #13

                _df.OldDeferredFrame.FrameScore = FramesAnalyzer.CalculateSpareFrameFromSpareFrame(vm);
                vm.TotalScore += (int)_df.OldDeferredFrame.FrameScore;
                vm.PreviousFrames.Add(_df.OldDeferredFrame);
                _df.ClearOldDeferredFrame();
                _df.OldDeferredFrame = vm.CurrentFrame;
                _df.ClearOldDeferredFrame();
                return;
            }
            if ((_df.OldDeferredFrame != null) && (vm.CurrentFrame.Mark == FrameMark.Spare) &&
                (_df.OldDeferredFrame.Mark == FrameMark.Strike) && (_df.OlderDeferredFrame == null))
            {
                //#16

                _df.OlderDeferredFrame = _df.OldDeferredFrame;
                _df.OldDeferredFrame = vm.CurrentFrame;
                return;
            }
            if ((_df.OlderDeferredFrame != null) && (_df.OldDeferredFrame != null) &&
                (vm.CurrentFrame.Mark == FrameMark.Spare) && (_df.OldDeferredFrame.Mark == FrameMark.Strike) &&
                (_df.OlderDeferredFrame.Mark == FrameMark.Strike))
            {
                //#18
                _df.OlderDeferredFrame.FrameScore = FramesAnalyzer.CalculateStrikeFrameScoreFromStrikeAndSpareFrames(vm);
                vm.TotalScore += (int)_df.OlderDeferredFrame.FrameScore;
                vm.PreviousFrames.Add(_df.OlderDeferredFrame);
                _df.OlderDeferredFrame = _df.OldDeferredFrame;
                _df.OldDeferredFrame = vm.CurrentFrame;
                return;
            }
            if ((vm.CurrentFrame.Mark == FrameMark.Strike) &&
                (_df.OldDeferredFrame == null) &&
                (_df.OlderDeferredFrame == null))
            {
                //#19
                _df.OldDeferredFrame = vm.CurrentFrame;
                _df.ClearOlderDeferredFrame();
                return;
            }
            if ((_df.OldDeferredFrame != null) && (vm.CurrentFrame.Mark == FrameMark.Strike) &&
                (_df.OldDeferredFrame.Mark == FrameMark.Spare) && (_df.OlderDeferredFrame == null))
            {
                //#22
                _df.OldDeferredFrame.FrameScore = FramesAnalyzer.CalculateSpareFrameFromStrikeFrame(vm);
                vm.TotalScore += (int)_df.OldDeferredFrame.FrameScore;
                vm.PreviousFrames.Add(_df.OldDeferredFrame);
                _df.OlderDeferredFrame = _df.OldDeferredFrame;
                _df.OldDeferredFrame = vm.CurrentFrame;
                return;
            }
            if ((_df.OldDeferredFrame != null) && (vm.CurrentFrame.Mark == FrameMark.Strike) &&
                (_df.OldDeferredFrame.Mark == FrameMark.Strike) && (_df.OlderDeferredFrame == null))
            {
                // #25
                _df.OlderDeferredFrame = _df.OldDeferredFrame;
                _df.OldDeferredFrame = vm.CurrentFrame;
                return;
            }
            if ((_df.OlderDeferredFrame != null) && (_df.OldDeferredFrame != null) &&
                (vm.CurrentFrame.Mark == FrameMark.Strike) && (_df.OldDeferredFrame.Mark == FrameMark.Strike) &&
                (_df.OlderDeferredFrame.Mark == FrameMark.Strike))
            {
                //#27

                _df.OlderDeferredFrame.FrameScore = FramesAnalyzer.CalculateStrikeFrameScoreFromStrikeAndStrike();
                vm.TotalScore += (int)_df.OlderDeferredFrame.FrameScore;
                vm.PreviousFrames.Add(_df.OlderDeferredFrame);
                _df.OlderDeferredFrame = _df.OldDeferredFrame;
                _df.OldDeferredFrame = vm.CurrentFrame;
            }
        }
    }
}
