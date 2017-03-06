
(function(){
gameModule.service("finalScoreService",
    function () { //to be shared among controllers

        var _finalScore;
        var setFinalScore = function (finalScore) {
            _finalScore = finalScore;
        };

        var getFinalScore = function () {
            return _finalScore;
        };

        return {
            setFinalScore: setFinalScore,
            getFinalScore: getFinalScore
        };

    });

})();
