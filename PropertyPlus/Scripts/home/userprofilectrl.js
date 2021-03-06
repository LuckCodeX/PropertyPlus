﻿function UserProfileCtrl($scope,
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
            value: i
        };
        days.push(day);
    };
    var d = new Date();
    for (var i = 1900; i <= d.getFullYear(); i++) {
        var year = {
            name: i,
            value: i
        };
        years.push(year);
    }
    $scope.years = years;
    $scope.days = days;
    $scope.date = {};
    $scope.months = [
        { name: "January", value: "1" }, { name: "February", value: "2" }, { name: "March", value: "3" },
        { name: "April", value: "4" }, { name: "May", value: "5" }, { name: "Jun", value: "6" },
        { name: "July", value: "7" }, { name: "August", value: "8" }, { name: "September", value: "9" },
        { name: "October", value: "10" }, { name: "November", value: "11" }, { name: "December", value: "12" }
    ];

    $scope.genders = [{ name: "Male", value: "0" }, { name: "Female", value: "1" }];
    $scope.userprofile = {
        verification_images: []
    };

    $scope.loadHeaderHost = function () {
        if (!(localStorage && localStorage.getItem('user_profile'))) {
            window.location.href = "/";
        }
    }
    $scope.submitEditProfile = function () {
        if($scope.data.ImgVerification1_Base64) $scope.data.ImgVerification1_Base64 = $scope.data.ImgVerification1_Base64.url;
        if($scope.data.ImgVerification2_Base64) $scope.data.ImgVerification2_Base64 = $scope.data.ImgVerification2_Base64.url;
        var cloneObject = $scope.date;
        $scope.data.Gender = cloneObject.gender.value;
        if (cloneObject.year.value == "1900") {
            $scope.data.BirthDay = null;
        } else {
            $scope.data.BirthDay =
                Math.round((new Date(cloneObject.year.value,
                        Number(cloneObject.month.value - 1),
                        cloneObject.day.value,
                        0,
                        0,
                        0)).getTime() /
                    1000);
        };
        console.log($scope.data);
        xhrService.post("EditUserProfile", $scope.data).then(function (data) {
            localStorage.setItem('user_profile', Base64.encode(JSON.stringify(data.data)));
            var scope = angular.element('body[ng-controller="MainCtrl"]').scope();
            scope.userProfile = data.data;
            $timeout(function () {
                scope.$apply();
            }, 0);
            $anchorScroll();
            toastr.info('Save success');
            // $scope.data = {};
            // $scope.loadUserEdit();
        },function (error) {
                $scope.errorText = error.statusText;
            });
    };

    $scope.loadUserGeneral = function(){
        xhrService.get("GetVisitList").then(function (data) {
            $scope.visitList = data.data;
            console.log($scope.visitList);
        },
        function (error) {
            console.log(error.statusText);
        });
        xhrService.get("GetYourListApartment/-1").then(function (data) {
            $scope.yourApartmentList = data.data;
            console.log($scope.yourApartmentList);
        },
        function (error) {
            console.log(error.statusText);
        });
    }

    $scope.deleteApartmentVisit = function(id){
        xhrService.delete("DeleteVisitList/"+ id).then(function (data) {
            alert('Delete success !');
            $scope.loadUserGeneral();
        },
        function (error) {
            console.log(error.statusText);
        });
    }

    $scope.loadUserSetting = function(){
        $scope.data = {};
    }

    $scope.submitChangePwd = function(){
        xhrService.post("ChangePassword",$scope.data).then(function (data) {
            alert('Change password success !');
        },
        function (error) {
            console.log(error.statusText);
        });
    }

    $scope.loadUserEdit = function () {
        toastr.options = {
          "closeButton": false,
          "debug": false,
          "newestOnTop": true,
          "progressBar": false,
          "positionClass": "toast-bottom-full-width",
          "preventDuplicates": false,
          "onclick": null,
          "showDuration": "300",
          "hideDuration": "1000",
          "timeOut": "3000",
          "extendedTimeOut": "1000",
          "showEasing": "swing",
          "hideEasing": "linear",
          "showMethod": "fadeIn",
          "hideMethod": "fadeOut"
        }
        xhrService.get("GetUserProfile/").then(function (data) {
            $scope.myImage = "";
            $scope.myCroppedImage = "";
            var handleFileSelect = function(evt) {
                var file = evt.currentTarget.files[0];
                var reader = new FileReader();
                reader.onload = function(evt) {
                    $scope.$apply(function($scope) {
                        $scope.myImage = evt.target.result;
                    });
                };
                reader.readAsDataURL(file);
            };
           
            setTimeout(function() {
                angular.element(document.querySelector("#testFile")).on("change", handleFileSelect);
                
            },
            1000);
            if (data.data.ImgVerification1 != null && data.data.ImgVerification1 != "") {
                $('#delete-file1').addClass('active');
            }
            if (data.data.ImgVerification2 != null && data.data.ImgVerification2 != "") {
                $('#delete-file2').addClass('active');
            }
            if (data.data.Avatar_Base64 != null) document.getElementById("Avatar_Base64").src = data.data.Avatar_Base64;
            $scope.data = data.data;
            var currentDate = new Date(Number($scope.data.BirthDay) * 1000);
            if ($scope.data.Gender != 0) {
                for (var i = 0; i < $scope.genders.length; i++) {
                    if ($scope.data.Gender == $scope.genders[i].value) {
                        console.log($scope.genders[i]);
                        $scope.date.gender = $scope.genders[i];
                    }
                }
            } else {
                $scope.date.gender = $scope.genders[0];
            }
            if (currentDate != 0) {
                
                for (var i = 0; i < $scope.days.length; i++) {
                    if (currentDate.getDate() == $scope.days[i].value) {
                        $scope.date.day = $scope.days[i];
                    }
                }
                for (var i = 0; i < $scope.years.length; i++) {
                    if (currentDate.getFullYear() == $scope.years[i].value) {
                        $scope.date.year = $scope.years[i];
                    }
                }
                for (var i = 0; i < $scope.months.length; i++) {
                    if ((currentDate.getMonth() + 1) == $scope.months[i].value) {
                        $scope.date.month = $scope.months[i];
                    }
                }
            } else {
                $scope.date.day = $scope.days[0];
                $scope.date.year = $scope.years[0];
                $scope.date.month = $scope.months[0];
            }

        },
            function (error) {
                console.log(error.statusText);
            });
    };

    $scope.logout = function () {
        localStorage.removeItem("user_profile");
        var scope = angular.element('body[ng-controller="MainCtrl"]').scope();
        scope.userProfile = undefined;
        $timeout(function () {
            scope.$apply();
        },0);
        $location.path("/");
    };
}

app.controller("UserProfileCtrl", UserProfileCtrl);