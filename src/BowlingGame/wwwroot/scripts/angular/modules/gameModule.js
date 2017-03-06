

var gameModule = angular.module("gameModule", ["ngRoute"])
    .config(function($routeProvider) {
            $routeProvider.when("/", { templateUrl: "/html/Templates/Game.html", controller: "gameController" });
            $routeProvider.when("/Game/Results",
                { templateUrl: "/html/Templates/Results.html", controller: "resultsController" });
        }
    ); 

    

