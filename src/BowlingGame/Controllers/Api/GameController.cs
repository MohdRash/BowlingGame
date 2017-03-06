using System;
using System.Linq;
using System.Threading.Tasks;
using BowlingGame.Helpers;
using BowlingGame.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BowlingGame.Controllers.Api
{
    public class GameController : Controller
    {
        private readonly DeferredFrames _df; //Thanks for DI!


        public GameController(DeferredFrames df)
        {
            _df = df;
        }

        [HttpPost("api/game")]
        public  IActionResult Get([FromBody] GameViewModel vm)
        {
            //should I validate old FrameViewModel?


            try
            {
                #region Definitions

                var frameNumber = vm.CurrentFrame.FrameNumber;
                var currentFrameThirdRollKnocks = vm.CurrentFrame.ThirdRollKnocks;
                var currentFrameSecondRollKnocks = vm.CurrentFrame.SecondRollKnocks;
                var currentFrameFirstRollKnocks = vm.CurrentFrame.FirstRollKnocks;

                #endregion

                #region FrameValidationRegion

                if ((frameNumber < 1) || (frameNumber > 10))
                    return BadRequest("The frame number must be between one and ten");
                if (frameNumber == 1) //clear any old history if it is a new game
                    _df.ClearAll();

                #endregion

                #region FirstToNinthFramesRegion

                if ((frameNumber >= 1) && (frameNumber <= 9)) // for frames between 1 and 9
                {
                    #region InputValidation

                    if (currentFrameThirdRollKnocks != null)
                        return
                            BadRequest(
                                "The third knocks input is only allowed in the tenth frame with spare or strike cases");

                    if ((currentFrameFirstRollKnocks > 10) || (currentFrameFirstRollKnocks < 0))
                        return BadRequest("The number of knocked pins in the first round must be between one and ten");

                    if ((currentFrameSecondRollKnocks > 10) || (currentFrameSecondRollKnocks < 0))
                        return BadRequest("The number of knocked pins in the second round must be between one and ten");

                    if (currentFrameFirstRollKnocks + currentFrameSecondRollKnocks > 10)
                        return BadRequest("The total number of knocked pins cannot be greater than ten");

                    #endregion

                    FramesAnalyzer.DetermineCurrentFrameType(vm.CurrentFrame);
                    FramesAnalyzer.ProcessScores(vm,_df);
                    return Ok(vm);
                }

                #endregion

                #region TenthFrameRegion

                #region StrikeOrSpareRegion

                FramesAnalyzer.DetermineCurrentFrameType(vm.CurrentFrame);
                if ((vm.CurrentFrame.Mark == FrameMark.Spare) || (vm.CurrentFrame.Mark == FrameMark.Strike))
                    // You got either strike or spare, you deserve extra bonus

                {
                    #region InputValidationRegion

                    if (currentFrameThirdRollKnocks == null)
                        return BadRequest("You must provide the third round input when you have strike or spare");
                    if ((currentFrameThirdRollKnocks < 0) || (currentFrameThirdRollKnocks > 10))
                        return BadRequest("The number of knocked pins in the third round must be between one and ten");

                    #endregion

                    /* There is a minor trick here:
                                                                                                                                                 *  1) if it is a spare we will need eleventh frame
                                                                                                                                                  *2) if it is a strike we will need eleventh and twelfth frame*/


                    //Process the tenth frame
                    FramesAnalyzer.ProcessScores(vm, _df); 

                    #region EleventhFrame

                    // Create two requests for the eleventh and twelefths frames
                    // Create a request for the eleventh frame
                    // eleventh frame is created by the third input
                    if (vm.CurrentFrame.Mark == FrameMark.Spare)
                    {
                        vm.CurrentFrame = new FrameViewModel
                        {
                            FrameScore = null,
                            FirstRollKnocks = (int) vm.CurrentFrame.ThirdRollKnocks,
                            FrameNumber = 11,
                            SecondRollKnocks = 0
                        };
                        FramesAnalyzer.DetermineCurrentFrameType(vm.CurrentFrame);
                        FramesAnalyzer.ProcessScores(vm, _df);

                        // Dummy frames trick to force processing of 11 if it is strike or spare (to avoid bugs and rewriting logic)

                        #region DummyFrames

                        vm.CurrentFrame = new FrameViewModel
                        {
                            FrameScore = null,
                            FirstRollKnocks = 0,
                            FrameNumber = 12,
                            SecondRollKnocks = 0,
                            Mark = FrameMark.Open
                        };
                        FramesAnalyzer.ProcessScores(vm, _df);
                        vm.CurrentFrame = new FrameViewModel
                        {
                            FrameScore = null,
                            FirstRollKnocks = 0,
                            FrameNumber = 13,
                            SecondRollKnocks = 0,
                            Mark = FrameMark.Open
                        };
                        FramesAnalyzer.ProcessScores(vm, _df);
                        vm.PreviousFrames.RemoveAll(x => (x.FrameNumber == 12) || (x.FrameNumber == 13));
                        vm.CurrentFrame = vm.PreviousFrames.FirstOrDefault(x => x.FrameNumber == 11);

                        #endregion
                    }
                    #endregion
                    #region EleventhAndTwelvethFrames

                    else if (vm.CurrentFrame.Mark == FrameMark.Strike)
                    {
                        // evelventh frame is created by the second input
                        //twelventh frame is created by the third input


                        var secondInput = vm.CurrentFrame.SecondRollKnocks;
                        var thirdInput = (int) vm.CurrentFrame.ThirdRollKnocks;


                        vm.CurrentFrame = new FrameViewModel
                        {
                            FrameScore = null,
                            FirstRollKnocks = secondInput,
                            FrameNumber = 11,
                            SecondRollKnocks = 0
                        };
                        FramesAnalyzer.DetermineCurrentFrameType(vm.CurrentFrame);
                        FramesAnalyzer.ProcessScores(vm, _df);

                        vm.CurrentFrame = new FrameViewModel
                        {
                            FrameScore = null,
                            FirstRollKnocks = thirdInput,
                            FrameNumber = 12,
                            SecondRollKnocks = 0
                        };
                        FramesAnalyzer.DetermineCurrentFrameType(vm.CurrentFrame);
                        FramesAnalyzer.ProcessScores(vm, _df);


                        // Dummy frames trick to force processing of 11 if it is strike or spare (to avoid bugs and rewriting logic)

                        #region DummyFrames

                        vm.CurrentFrame = new FrameViewModel
                        {
                            FrameScore = null,
                            FirstRollKnocks = 0,
                            FrameNumber = 13,
                            SecondRollKnocks = 0,
                            Mark = FrameMark.Open
                        };
                        FramesAnalyzer.ProcessScores(vm, _df);
                        vm.CurrentFrame = new FrameViewModel
                        {
                            FrameScore = null,
                            FirstRollKnocks = 0,
                            FrameNumber = 14,
                            SecondRollKnocks = 0,
                            Mark = FrameMark.Open
                        };
                        FramesAnalyzer.ProcessScores(vm, _df);
                        vm.PreviousFrames.RemoveAll(x => (x.FrameNumber == 13) || (x.FrameNumber == 14));
                        vm.CurrentFrame = vm.PreviousFrames.FirstOrDefault(x => x.FrameNumber == 12);

                        #endregion
                    }

                    #endregion

                    vm.GameCompleted = true;
                    return Ok(vm);
                }

                #endregion

                #region OpenRegion

                #region InputValidationRegion

                if (currentFrameThirdRollKnocks != null)
                    return BadRequest("Third round input is only allowed for spares and strikes");

                if (currentFrameFirstRollKnocks + currentFrameSecondRollKnocks > 10)
                    return BadRequest("The number of knocked pins cannot be greater than ten");

                #endregion

                FramesAnalyzer.DetermineCurrentFrameType(vm.CurrentFrame);
                FramesAnalyzer.ProcessScores(vm, _df);
                vm.GameCompleted = true;
                return Ok(vm);

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                //Though it isn not good practice to send the code in the wire
                return BadRequest(ex);
            }
            return BadRequest("An Error has occured, please try again later");
        }

        // I need unit tests
    

        

       
    }
}