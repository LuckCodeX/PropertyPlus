function HostCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    $sce,
    xhrService,
    $anchorScroll) {
    $scope.loadHeaderHost = function () {
        if (!(localStorage && localStorage.getItem('user_profile'))) {
            window.location.href = "/";
        }

    }
    $scope.apartments = [{ name: "Apartment", value: "1" }];
    $scope.bedrooms = [
        { name: "1", value: "1" }, { name: "2", value: "2" }, { name: "3", value: "3" }, { name: "4", value: "4" },
        { name: "5", value: "5" }, { name: "6", value: "6" }
    ];
    $scope.bathrooms = [
        { name: "1", value: "1" }, { name: "2", value: "2" }, { name: "3", value: "3" }, { name: "4", value: "4" },
        { name: "5", value: "5" }, { name: "6", value: "6" }
    ];
    $scope.loadStep1_1 = function () {
        $scope.data = {
            NoBedRoom: "1",
            NoBathRoom: "1"
        };
        $scope.projectList = [];
        xhrService.get("GetAllProject/")
            .then(function (data) {
                $scope.projectList = data.data;
                $scope.data.ProjectId = "-1";
            },
                function (error) {
                    console.log(error.statusText);
                });
        $scope.data.Type = "1";

    };

    $scope.loadStep1_2 = function () {
        if ($scope.data == undefined) {
            $location.url('/host/listing');
        }
    };

    $scope.loadStep2_1 = function () {
        if ($scope.data == undefined) {
            $location.url('/host/listing');
        }
        if ($scope.images == undefined) {
            $scope.images = [];
            $scope.images.push({ file: null, url: null });
        }

    }
    $scope.loadStep1_3 = function () {
        if ($scope.data == undefined) {
            $location.url('/host/listing');
        }
        var myLatLng = { lat: $scope.data.Latitude, lng: $scope.data.Longitude };
        var map = new google.maps.Map(document.getElementById('map'),
            {
                zoom: 17,
                center: myLatLng
            });
        var marker = new google.maps.Marker({
            position: myLatLng,
            map: map,
            title: $scope.data.Address
        });
    }
    $scope.loadStep4_1 = function () {
        if ($scope.data == undefined) {
            $location.url('/host/listing');
        }
    }

    $scope.loadStep3_1 = function () {
        if ($scope.data == undefined) {
            $location.url('/host/listing');
        }
    }

    $scope.uploadImg = function (event) {
        if ($scope.images.length < 6) {
            $scope.images.push({ file: null, url: null });
        }
    };

    $scope.disableBtn = "service-basic";

    $scope.submitStep1_1 = function() {
        $location.url("host/create/step-1-2");
    };

    $scope.submitStep1_3 = function () {
        $location.url("host/create/step-1-3");
    };

    $scope.saveStep2 = function(images, banner) {
        $scope.banner_img = banner;
        $scope.images = images;
    };

    $scope.submitAll = function (confirm_img) {
        $scope.confirm_img = confirm_img;
        $scope.data.ImgList = [];
        if ($scope.banner_img.url != null) {
            $scope.data.ImgList.push({
                "Img_Base64": $scope.banner_img.url,
                "Type": 0
            });
        }
        if ($scope.images.length > 1) {
            if ($scope.images[$scope.images.length - 1].url == null) {
                $scope.images.splice(-1, 1);
            }
            for (var i = 0; i < $scope.images.length; i++) {
                var item = {
                    "Img_Base64": $scope.images[i].url,
                    "Type": -1
                }
                $scope.data.ImgList.push(item);
            }
        }
        if (confirm_img.url != null) {
            $scope.data.ImgList.push({
                "Img_Base64": confirm_img.url,
                "Type": 2
            });
        }
        xhrService.post("CreateApartment", $scope.data).then(function (data) {
            $scope.data = undefined;
        }, function (error) {
            $scope.errorText = error.statusText;
        });
    }


}

app.controller('HostCtrl', HostCtrl);