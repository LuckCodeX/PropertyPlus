function DetailCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    $scope.id = $stateParams.id;
}

app.controller('DetailCtrl', DetailCtrl);