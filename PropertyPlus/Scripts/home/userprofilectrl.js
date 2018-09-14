function UserProfileCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll,
    $http) {
    var days = [];
    var years = [];
    for (var i = 1; i <= 31; i++) {
        var day = {
            name: i,
            value:i
        }
        days.push(day);
    };
    var d = new Date();
    for (var i = 1900; i <= d.getFullYear(); i++) {
        var year = {
            name: i,
            value: i
        }
        years.push(year);
    }
    $scope.years = years;
    $scope.days = days;
    $scope.date = {};
    $scope.months = [{ name: "January", value: "1" }, { name: "February", value: "2" }, { name: "March", value: "3" }, { name: "April", value: "4" }, { name: "May", value: "5" }, { name: "Jun", value: "6" }, { name: "July", value: "7" }, { name: "August", value: "8" }, { name: "September", value: "9" }, { name: "October", value: "10" }, { name: "November", value: "11" }, { name: "December", value: "12" }]
    $scope.data = {};
    $scope.genders = [{ name: "Male", value: "0" }, { name: "Female", value: "1" }];
    $scope.myImage = '';
    $scope.myCroppedImage = '';
    var handleFileSelect = function (evt) {
        var file = evt.currentTarget.files[0];
        var reader = new FileReader();
        reader.onload = function (evt) {
            $scope.$apply(function ($scope) {
                $scope.myImage = evt.target.result;
            });
        };
        reader.readAsDataURL(file);
    };
    
    setTimeout(function () {
        angular.element(document.querySelector('#testFile')).on('change', handleFileSelect);
        angular.element(document.querySelector('#file1')).on('change', function(evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.data.ImgVerification1_Base64 = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        });
        angular.element(document.querySelector('#file2')).on('change', function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.data.ImgVerification2_Base64 = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
            
        });
    }, 1000);
    
    $scope.userprofile = {
        verification_images: []
    };
    $scope.submitEditProfile = function () {
        $scope.data.Avatar_Base64 = document.getElementById("Avatar_Base64").src;
        var cloneObject = $scope.date;
        $scope.data.Gender = cloneObject.gender.value;
        $scope.data.BirthDay = Math.round((new Date(cloneObject.year.value, Number(cloneObject.month.value - 1), cloneObject.day.value, 0, 0, 0)).getTime() / 1000);
      
        xhrService.post("EditUserProfile", $scope.data).then(function (data) {
            console.log(data);
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
    }

    $scope.loadUserEdit = function() {
        xhrService.get("GetUserProfile/").then(function (data) {
                console.log(data);
            },
                function (error) {
                    console.log(error.statusText);
                });
    }

    $scope.logout = function() {
        localStorage.removeItem('user_profile');
        var scope = angular.element('body[ng-controller="MainCtrl"]').scope();
        scope.userProfile = undefined;
        $timeout(function () {
            scope.$apply();
        }, 0);
        $location.path("/");
    }
}

app.controller('UserProfileCtrl', UserProfileCtrl);