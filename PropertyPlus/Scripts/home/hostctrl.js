function HostCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    $scope.apartment = {};
    $scope.apartments = [{ name: "Time City", value: "1" }, { name: "Royal City", value: "2s" }];
    $scope.cities = [{ name: "Ha Noi", value: "1" }, { name: "Ho Chi Minh", value: "2" }];
    $scope.names = [{ name: "Time City", value: "1" }, { name: "Royal City", value: "2" }];
    $scope.loadStep1_1 = function() {
        $scope.apartment.apartment = $scope.apartments[0];
        $scope.apartment.city = $scope.cities[0];
        $scope.apartment.name = $scope.names[0];
        $scope.another_img_1 = '';
        $scope.another_img_2 = '';
        $scope.another_img_3 = '';
    };

    $scope.submitStep1_1 = function() {
        $location.url("host/create/step-1-2");
    }

    $scope.loadStep1_2 = function() {
        console.log($scope.apartment);
    };

    $scope.submitStep1_3 = function() {
        $location.url("host/create/step-1-3");
    };

    $scope.loadStep1_3 = function () {
        console.log($scope.apartment);
    };

    $scope.test = function() {
        console.log($scope.another_img_1);
    };
}

app.controller('HostCtrl', HostCtrl);