
(function(){
gameModule.controller("gameController",
    function($scope, GameService, finalScoreService, $location) {


        var clearAll = function() {
            $scope.currentFrame = {
                frameNumber: "1",
                mark: "0", // a dummy value, will be overwritten in the controller logic
                frameScore: null,
                firstRollKnocks: null,
                secondRollKnocks: null,
                thirdRollKnocks: null


            };
            $scope.totalScore = "0";
            $scope.previousFrames = [];
            $scope.gameCompleted = false;
            $scope.min = 0;
            $scope.max = 10;
            $scope.serverErrorMessage = null;
            $scope.showErrors = false;
        };

        clearAll();

        $scope.playAgain = function() {
            clearAll();

            window.location.pathname = "/Game/Index";
        };
        $scope.playGame = function() {
            GameService.postGame(JSON.stringify({
                    currentFrame: $scope.currentFrame,
                    totalScore: $scope.totalScore,
                    previousFrames: $scope.previousFrames
                }),
                function(successResult) {

                    if (successResult.gameCompleted) {
                        finalScoreService.setFinalScore(successResult.totalScore);

                        // I will give you a chance to see what you have done before I redirect you!
                        //  setTimeout(function () { $location.path('/Game/Results') }, 3000);


                        $location.path("/Game/Results");
                    }

                    $scope.showErrors = false;
                    $scope.previousFrames = successResult.previousFrames;
                    $scope.totalScore = successResult.totalScore;
                    $scope.currentFrame = {
                        frameNumber: (successResult.currentFrame.frameNumber + 1),
                        mark: "0", // a dummy value, will be overwritten in the controller logic
                        frameScore: null,
                        firstRollKnocks: null,
                        secondRollKnocks: null,
                        thirdRollKnocks: null


                    };
                },
                function(failureResult) {

                    //  alert("Failure " + JSON.stringify(result));
                    $scope.showErrors = true;
                    $scope.serverErrorMessage = failureResult;
                });
        };
    });

})();