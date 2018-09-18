function ApartmentCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    var clean = {};
    clean[1] = 40;
    clean[2] = 65;
    clean[3] = 90;
    clean[4] = 115;
    clean[5] = 135;
    clean[6] = 150;
    var drink = {};
    drink[1] = 3;
    drink[2] = 6;
    drink[3] = 9;
    drink[4] = 11;
    drink[5] = 13;
    drink[6] = 15;
    $scope.loadApartmentDetail = function () {
        $scope.apartmentPrice = 1500;
        $scope.cleaning = 3;
        $scope.bottle = 4;
        $scope.basic = "service-basic";
        $scope.electricBill = 0;
        document.getElementById("foreign-tv").value = 25;
        $scope.changeService();
    }
    $scope.changeBill = function() {
        if ($scope.electricBill > 999 || $scope.electricBill < 0) {
            $scope.electricBill = 0;
        }
    }
    $scope.radioService = function () {
        if (document.getElementById("radioBasic").checked) {
            $scope.basic = "service-basic";
            $scope.cleaning = 3;
            $scope.bottle = 4;
            $scope.electricBill = 0;
            document.getElementById("foreign-tv").value = 25;
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
        if (document.getElementById("checkboxFee").checked) {
            managamentFee = 50;
        };
        if (document.getElementById("checkboxInternet").checked) {
            internet = 20;
        };
        if (document.getElementById("checkboxToilet").checked) {
            detergent = 10;
        }
        var tv = document.getElementById("foreign-tv");
        var foreignTv = Number(tv.options[tv.selectedIndex].value);
        var laundry = clean[$scope.cleaning];
        var water = drink[$scope.bottle];
        var electricBill = Number($scope.electricBill);
        var servicePrice = managamentFee + internet + foreignTv + laundry + water + detergent + electricBill;
        $scope.servicePrice = servicePrice;
        $scope.totalPrice = $scope.apartmentPrice + $scope.servicePrice;
        $scope.perNightPrice = (($scope.totalPrice / 30) * 1.6).toFixed(2);
    }
}

app.controller('ApartmentCtrl', ApartmentCtrl);