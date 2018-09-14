function HostCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    $scope.apartment = {};
    $scope.apartments = [{ name: "Time City", value: "1" }, { name: "Royal City", value: "1" }];

}

app.controller('HostCtrl', HostCtrl);