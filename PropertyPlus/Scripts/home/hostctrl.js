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
            NoBathRoom: "1",
            Type: "1",
            ProjectId: "",
            FacilityList: []
        };
        $scope.projectList = [];
        xhrService.get("GetAllProject")
            .then(function (data) {
                $scope.projectList = data.data;
            }, function (error) {
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

    $scope.loadStep1_2 = function () {
        if ($scope.data == undefined) {
            $location.url('/host/listing');
        }
        $scope.oldLat = undefined;
        $scope.oldLong = undefined;
        $scope.statusAddress = true;
    };

    $scope.loadStep2_1 = function () {
        if ($scope.data == undefined) {
            $location.url('/host/listing');
        }
        if ($scope.images == undefined) {
            $scope.images = [];
            $scope.images.push({ file: null, url: null });
        }
        $scope.oldLengthImg = 1;
    }

    $scope.editorOptions = {
        language: 'vi'
        // uiColor: '#000000'
    };

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
        if ($scope.imagesConfirm == undefined) {
            $scope.imagesConfirm = [];
            $scope.imagesConfirm.push({ file: null, url: null });
        }
    }

    $scope.loadStep3_1 = function () {
        if ($scope.data == undefined) {
            $location.url('/host/listing');
        }
    }

    $scope.uploadImages = function(img){

    }

    $scope.uploadImg = function (event) {
        $scope.$watchCollection('images', function() {
            if ($scope.images[$scope.images.length-1].url != null && $scope.images.length <6) {
                $scope.images.push({ file: null, url: null });
                return;
            }
         });
    };

    $scope.uploadConfirmImg = function (event) {
        $scope.$watchCollection('imagesConfirm', function() {
            if ($scope.imagesConfirm[$scope.imagesConfirm.length-1].url != null && $scope.imagesConfirm.length <5) {
                $scope.imagesConfirm.push({ file: null, url: null });
                return;
            }
         });
    };

    $scope.deleteBanner = function(){
        $scope.banner_img.url = null;
    }

    $scope.disableBtn = "service-basic";

    $scope.submitStep1_1 = function () {
        $location.url("host/create/step-1-2");
    };

    $scope.submitStep1_3 = function () {
        $location.url("host/create/step-1-3");
    };

    $scope.saveStep2 = function (images, banner) {
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
        if ($scope.confirm_img.length > 1) {
            if ($scope.confirm_img[$scope.confirm_img.length - 1].url == null) {
                $scope.confirm_img.splice(-1, 1);
            }
            for (var i = 0; i < $scope.confirm_img.length; i++) {
                var item = {
                    "Img_Base64": $scope.confirm_img[i].url,
                    "Type": 1
                }
                $scope.data.ImgList.push(item);
            }
        }
        xhrService.post("CreateApartment", $scope.data).then(function (data) {
            $scope.data = undefined;
            $location.url("host/create/finish");
        }, function (error) {
            $scope.errorText = error.statusText;
        });
    }


}

app.controller('HostCtrl', HostCtrl);