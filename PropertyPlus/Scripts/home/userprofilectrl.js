function UserProfileCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    $scope.userprofile = {
        verification_images: []
    };
}

app.controller('UserProfileCtrl', UserProfileCtrl);