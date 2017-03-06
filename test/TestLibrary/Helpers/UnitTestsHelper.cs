using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BowlingGame.ViewModels;

namespace TestLibrary.Helpers
{
    public static class UnitTestsHelper
    {

        public static FrameViewModel getMockOpenFrame()
        {
            return new FrameViewModel()
            {

                FirstRollKnocks = 3,
                SecondRollKnocks = 6
            };
        }
        public static FrameViewModel getMockSpareFrame()
        {
            return new FrameViewModel()
            {

                FirstRollKnocks = 4,
                SecondRollKnocks = 6
            };
        }
        public static FrameViewModel getMockStrikeFrame()
        {
            return new FrameViewModel()
            {

                FirstRollKnocks = 10,
                SecondRollKnocks = 0
            };
        }
    }
}
