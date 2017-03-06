
(function(){
gameModule.factory("GameService",
    function($http, $q) {
        return {
            postGame: function(postDetail, passCallback, failureCallBack) {
                //$http({
                //        method: "POST",
                //        headers: { 'Content-Type': 'application/json' },
                //        url: "/api/game",
                //        data: postDetail
                //    })
                //    .success(callback);

                $http({
                        method: "POST",
                        url: "/api/game",
                        headers: { 'Content-Type': "application/json" },
                        data: postDetail
                    })
                    .then(function successCallback(response) {
                            // this callback will be called asynchronously
                            // when the response is available
                            passCallback(response.data);
                            //  alert("Success: " + JSON.stringify(response.data));
                        },
                        function errorCallback(response) {
                            // called asynchronously if an error occurs
                            // or server returns response with an error status.
                            failureCallBack(response.data);
                            // alert("Fail: " + JSON.stringify(response.data));
                        });
            }
        };
    });

})();