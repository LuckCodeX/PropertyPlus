function HostmanageCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll,
    $window) {

    $scope.loadHostManage=function(){
        xhrService.get("GetYourListApartment/1").then(function (data) {
            $scope.yourApartmentlist=data.data;

        },
         function (error) {
            console.log(error.statusText);
        });
    };

$scope.changeAparment=function(){
        xhrService.get("GetApartmentInformation/"+ $scope.apartmentId).then(function (data) {
            console.log(data);
$scope.apartmentchoice=data.data;
// $scope.apartmentchoicepj=data.data.Project;
            // console.log(data.data.Project);



        },
         function (error) {
            console.log(error.statusText);
        });
    };
    
    $scope.changeAddress = function () {
        if ($scope.data.Latitude == undefined ||
            $scope.data.Latitude == 0 ||
            $scope.data.Longitude == undefined ||
            $scope.data.Longitude == 0 ||
            $scope.data.Longitude == $scope.oldLong ||
            $scope.data.Latitude == $scope.oldLat) {
            $scope.statusAddress = false;

        } else {
            $scope.oldLat = $scope.data.Latitude;
            $scope.oldLong = $scope.data.Longitude;
            $scope.statusAddress = true;
        }
    }
}

app.controller('HostmanageCtrl', HostmanageCtrl);