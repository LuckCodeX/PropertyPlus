function ProjectCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    xhrService.get("GetSlide/1").then(function (data) {
            $scope.slideImg = "Upload/" + data.data.Img;
        },
        function (error) {
            $scope.errorText = error.statusText;
        });
}

app.controller('ProjectCtrl', ProjectCtrl);