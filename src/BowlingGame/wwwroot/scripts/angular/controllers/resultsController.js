
(function () {
gameModule.controller("resultsController",
    function($scope, $location, finalScoreService) {
        $scope.score = finalScoreService.getFinalScore();
        // $scope.score = $rootScope.finalScore;
        $scope.playAgain = function() {
            $location.path("/");
        };
    });

})();