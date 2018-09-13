function HomeCtrl($scope, $rootScope, $stateParams, $location, $timeout, xhrService, $anchorScroll, $translate) {
    $scope.loadData = function () {
        if (localStorage && localStorage.getItem('language')) {
            $translate.use(localStorage.getItem('language'));
        }
        xhrService.get("GetSlide/0").then(function (data) {
                $scope.slideImg = "Upload/" + data.data.Img;
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
    }
}

app.controller('HomeCtrl', HomeCtrl);