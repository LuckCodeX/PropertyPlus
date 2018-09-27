function ApartmentCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    var clean = [0, 40, 65, 90, 115, 135, 150];
    //clean[1] = 40;
    //clean[2] = 65;
    //clean[3] = 90;
    //clean[4] = 115;
    //clean[5] = 135;
    //clean[6] = 150;
    var drink = [0, 3, 6, 9, 11, 13, 15];
    //drink[1] = 3;
    //drink[2] = 6;
    //drink[3] = 9;
    //drink[4] = 11;
    //drink[5] = 13;
    //drink[6] = 15;
    $scope.foreignTv = {};
    $scope.foreignTvs = [
        { name: 'None', value: '0' },
        { name: 'Japanese TV PP1 Package', value: '25' },
        { name: 'Japanese TV PP2 Package', value: '35' },
        { name: 'Japanese TV PP3 Package', value: '50' },
        { name: 'Japanese TV BB max 1', value: '35' },
        { name: 'Japanese TV BB max 2', value: '50' },
        { name: 'Japanese TV BB max 3', value: '70' },
        { name: 'Korean TV', value: '40' },
        { name: 'Vietnamese K+ TV', value: '10' },
        { name: 'An Vien TV', value: '10' }
    ];

    $scope.loadApartment = function () {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        var limit = 8;

        xhrService.get("GetListApartment/" + $scope.bigCurrentPage + "/" + limit)
            .then(function (data) {
                $scope.apartmentList = data.data.data;
                var myLatLng = { lat: $scope.apartmentList[0].Latitude, lng: $scope.apartmentList[0].Longitude };
                var map = new google.maps.Map(document.getElementById('map'),
                    {
                        zoom: 17,
                        center: myLatLng
                    });
                var marker = new google.maps.Marker({
                    position: myLatLng,
                    map: map,
                    title: $scope.apartmentList[0].Address
                });
            },
                function (error) {
                    console.log(error.statusText);
                });
    }

    $scope.loadApartmentDetail = function () {
        $scope.disabled = undefined;
        $scope.searchEnabled = undefined;

        $scope.setInputFocus = function () {
            $scope.$broadcast('UiSelectDemo1');
        };
        $scope.apartmentId = $stateParams.id;
        xhrService.get("GetApartmentDetail/" + $scope.apartmentId)
            .then(function (data) {
                $scope.apartment = data.data;
                for (var i = 0; i < $scope.apartment.ImgList.length; i++) {
                    if ($scope.apartment.ImgList[i].Type == 0) {
                        $scope.apartment.banner = $scope.apartment.ImgList[i].Img;
                        console.log($scope.apartment.banner);
                    }
                }
                var myLatLng = { lat: $scope.apartment.Latitude, lng: $scope.apartment.Longitude };
                var map = new google.maps.Map(document.getElementById('map'),
                    {
                        zoom: 17,
                        center: myLatLng
                    });
                var marker = new google.maps.Marker({
                    position: myLatLng,
                    map: map,
                    title: $scope.apartment.Address
                });
                console.log(data);
            },
                function (error) {
                    console.log(error.statusText);
                });
        xhrService.get("GetListApartment/" + 1 + "/" + 6)
            .then(function (data) {
                $scope.apartmentList = data.data.data;
            },
                function (error) {
                    console.log(error.statusText);
                });

        $timeout(loadApartmentDetail, 1000);

        $scope.someGroupFn = function (item) {

            if (item.name[0] >= 'A' && item.name[0] <= 'M')
                return 'From A - M';

            if (item.name[0] >= 'N' && item.name[0] <= 'Z')
                return 'From N - Z';

        };

        $scope.firstLetterGroupFn = function (item) {
            return item.name[0];
        };

        $scope.reverseOrderFilterFn = function (groups) {
            return groups.reverse();
        };

        $scope.foreignTv.selected = $scope.foreignTvs[1];
        $scope.apartmentPrice = 1500;
        $scope.cleaning = 3;
        $scope.bottle = 4;
        $scope.basic = "service-basic";
        $scope.electricBill = 0;
        $scope.changeService();
    }
    $scope.changeBill = function () {

        if ($scope.electricBill > 999 || isNaN($scope.electricBill.substr($scope.electricBill.length - 1))) {

            $scope.electricBill = $scope.electricBill.slice(0, -1);


        } else if ($scope.electricBill < 0) {
            $scope.electricBill = 0;
        } else if ($scope.electricBill == "") {
            $scope.electricBill = 0;
        }
        if ($scope.electricBill != "0") {
            while ($scope.electricBill.charAt(0) === '0') {
                $scope.electricBill = $scope.electricBill.substr(1);
            }
        }


    }
    $scope.radioService = function () {
        if (document.getElementById("radioBasic").checked) {
            $scope.basic = "service-basic";
            $scope.cleaning = 3;
            $scope.bottle = 4;
            $scope.electricBill = 0;
            $scope.foreignTv.selected = $scope.foreignTvs[1];
            document.getElementById("checkboxInternet").checked = true;
            document.getElementById("checkboxToilet").checked = true;
            document.getElementById("checkboxExtra").checked = false;
            document.getElementById("checkboxFee").checked = true;
        } else {
            $scope.basic = "";
        }
    }
    $scope.changeService = function () {
        var managamentFee = 0;
        var internet = 0;
        var detergent = 0;
        var extra = 1;
        if (document.getElementById("checkboxFee").checked) {
            managamentFee = 50;
        };
        if (document.getElementById("checkboxInternet").checked) {
            internet = 20;
        };
        if (document.getElementById("checkboxToilet").checked) {
            detergent = 10;
        }
        if (document.getElementById("checkboxExtra").checked) {
            extra = 0.9;
        }
        var tv = document.getElementById("foreign-tv");
        var foreignTv = Number($scope.foreignTv.selected.value);
        var laundry = clean[$scope.cleaning];
        var water = drink[$scope.bottle];
        var electricBill = Number($scope.electricBill);
        var servicePrice = managamentFee + internet + foreignTv + laundry + water + detergent + electricBill;
        $scope.servicePrice = servicePrice;
        $scope.totalPrice = (($scope.apartmentPrice + $scope.servicePrice) / extra).toFixed(2);
        $scope.perNightPrice = (($scope.totalPrice / 30) * 1.6).toFixed(2);
    }

}

app.controller('ApartmentCtrl', ApartmentCtrl);