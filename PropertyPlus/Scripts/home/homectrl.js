function HomeCtrl($scope, $rootScope, $stateParams, $location, $timeout, xhrService, $anchorScroll, $translate) {
    $scope.loadData = function () {
        if (localStorage && localStorage.getItem('language')) {
            $translate.use(localStorage.getItem('language'));
        }
    }
}

app.controller('HomeCtrl', HomeCtrl);