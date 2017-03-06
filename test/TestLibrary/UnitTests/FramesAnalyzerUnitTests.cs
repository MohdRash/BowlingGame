using BowlingGame.Helpers;
using BowlingGame.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestLibrary.Helpers;

namespace TestLibrary.UnitTests
{
    [TestClass]
    public class FramesAnalyzerUnitTests
    {


        // Generate mocks by factory?
       
        [TestMethod]
         public void TestDetermineCurrentFrameType_Open()
        {
            var mock = UnitTestsHelper.getMockOpenFrame();
            FramesAnalyzer.DetermineCurrentFrameType(mock);
            //Assert.AreEqual(true,true);
               Assert.AreEqual(FrameMark.Open, mock.Mark);
        }

        [TestMethod]
        public void TestDetermineCurrentFrameType_Spare()
        {
             var mock = UnitTestsHelper.getMockSpareFrame();
              FramesAnalyzer.DetermineCurrentFrameType(mock);
          //  Assert.AreEqual(true, true);
             Assert.AreEqual(FrameMark.Spare, mock.Mark);
        }

        [TestMethod]
        public void TestDetermineCurrentFrameType_Strike()
        {
               var mock = UnitTestsHelper.getMockStrikeFrame();
             FramesAnalyzer.DetermineCurrentFrameType(mock);
           // Assert.AreEqual(true, true);
            Assert.AreEqual(FrameMark.Strike, mock.Mark);
        }

        [TestMethod]
        public void TestCalculateStrikeFrameScoreFromStrikeAndStrike()
        {
          //  var mock = getMockSpareFrame();
            var result = FramesAnalyzer.CalculateStrikeFrameScoreFromStrikeAndStrike();

            Assert.AreEqual(20, result);
        }
   
        ////TODO: The rest of the methods go in a similar pattern
        //  [TestMethod]
        //public void TestCalculateSpareFrameFromStrikeFrame()
        //{
        //}
        //[TestMethod]
        //public void TestCalculateStrikeFrameScoreFromStrikeAndSpareFrames()
        //{

        //}
        //[TestMethod]
        //public void TestCalculateSpareFrameFromSpareFrame()
        //{
        //}
        //[TestMethod]
        //public void TestCalculateStrikeFrameScorefromStrikeAndOpenFrames()
        //{
        //}
        //[TestMethod]
        //public void TestCalculateStrikeFrameScoreFromOpenFrame()
        //{
        //}
        //[TestMethod]
        //public void TestCalculateSpareFrameScoreFromOpenFrame()
        //{
        //}
        //[TestMethod]
        //public void TestCalculateOpenFrameScore()
        //{
        //}

    }
}
