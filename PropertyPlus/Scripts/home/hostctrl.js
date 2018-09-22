function HostCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    $sce,
    xhrService,
    $anchorScroll) {
    $scope.data = {};
    $scope.apartments = [{ name: "Apartment", value: "1" }];
    $scope.bedrooms = [
        { name: "1", value: 1 }, { name: "2", value: 2 }, { name: "3", value: 3 }, { name: "4", value: 4 },
        { name: "5", value: 5 }, { name: "6", value: 6 }
    ];
    $scope.bathrooms = [
        { name: "1", value: 1 }, { name: "2", value: 2 }, { name: "3", value: 3 }, { name: "4", value: 4 },
        { name: "5", value: 5 }, { name: "6", value: 6 }
    ];
    $scope.loadStep1_1 = function () {
        $scope.projectList = [];
        xhrService.get("GetAllProject/")
            .then(function (data) {
                    $scope.projectList = data.data;
                    $scope.data.project_id = "-1";
                },
                function (error) {
                    console.log(error.statusText);
                });
        $scope.data.type = "1";
       
    };

    $scope.loadStep1_2 = function() {
        $scope.data.NoBedRoom = "1";
        $scope.data.NoBathRoom = "1";
    };

    $scope.loadStep2_1 = function () {
        if ($scope.images == undefined) {
            $scope.images = [];
            $scope.images.push({ file: null, url: null });
        }
        
    }
    $scope.uploadImg = function (event) {
        if ($scope.images.length < 6) {
            $scope.images.push({ file: null, url: null });
        }
    };

    $scope.disableBtn = "service-basic";

    //$scope.checkValid = function (status) {
    //    if (status)
    //        return ""
    //    else return 'service-basic';
    //}

    $scope.submitStep1_1 = function() {
        $location.url("host/create/step-1-2");
    }

    $scope.loadStep1_2 = function() {
    };

    $scope.submitStep1_3 = function() {
        $location.url("host/create/step-1-3");
    };
    $scope.saveAddress = function(address) {
        $scope.address = address;
        $scope.linkgg = "https://www.google.com/maps/embed/v1/place?key=AIzaSyCWuX4GXgxkGER7KMO5M5NM8npxQ04wxnk&q=" +
            address.lat +
            "," +
            address.lng;
        $scope.linkMap = $sce.trustAsResourceUrl($scope.linkgg);
    }
    $scope.saveStep2 = function (images, banner) {
        $scope.banner_img = banner;
        $scope.images = images;
    }
    $scope.submitAll = function (confirm_img) {
        $scope.confirm_img = confirm_img;
        $scope.data.NoBedRoom = $scope.data.NoBedRoom.value;
        $scope.data.NoBathRoom = $scope.data.NoBathRoom.value;
        $scope.data.Address = $scope.address.formattedAddress;
        $scope.data.Latitude = $scope.address.lat; 
        $scope.data.Longitude = $scope.address.lng;
        $scope.data.City = $scope.address.administrative_area_level_1;
        $scope.data.Type = $scope.apartment.value;
        $scope.data.ProjectId = $scope.project.value;
        $scope.data.ImgList = [];
        if ($scope.banner_img.url != null) {
            $scope.data.ImgList.push({
                "Img_Base64": $scope.banner_img.url,
                "Type": 0
            });
        }
        if ($scope.images.length > 1) {
            if ($scope.images[$scope.images.length-1].url == null) {
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
        console.log($scope.data);
        xhrService.post("CreateApartment", $scope.data).then(function (data) {
            console.log(data);
        }, function (error) {
            $scope.errorText = error.statusText;
        });
    }

    $scope.loadStep1_3 = function () {
        console.log($scope.apartment);
    };

    $scope.test = function() {
        console.log($scope.another_img_1);
    };
}

app.controller('HostCtrl', HostCtrl);