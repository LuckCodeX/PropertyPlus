function ApartmentCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    $scope.searchWithFilter = {};
    var scope = angular.element('body[ng-controller="MainCtrl"]').scope();
    $timeout(function () {
        scope.$apply();
    }, 0);
    scope.searchForm = function(){
        console.log(1);
        $scope.loadApartment();
    };

    var statusFilter = true;
    $('.list-btn-facilities .group-btn-facility .btn-facilities').click(function(){
        var atrrb = $(this).attr("target-filter");
        $('.list-btn-facilities .group-btn-facility .card-filter').each(function(){
            if($(this).attr("id") != atrrb.replace("#","")){
                $(this).removeClass("active");
                $(this).parent().children('.btn-facilities').removeClass("active");
            } 
        });
        $(this).toggleClass('active');
        $(atrrb).toggleClass('active');

    });
    $(".list-btn-facilities .group-btn-facility .card-filter").mouseup(function(event){
           statusFilter=false;
        });
     $("body").mouseup(function(){ 
        console.log(statusFilter);
        if(statusFilter){
            $('.btn-facilities.active').removeClass("active");
            $('.card-filter.active').removeClass("active");
        }
         statusFilter=true; 
    });
    
    if (scope.searchData) {
        $scope.searchWithFilter = scope.searchData;
        $scope.bedroom = $scope.searchWithFilter.FilterRoom.NoBedRoom;
        $scope.bathroom = $scope.searchWithFilter.FilterRoom.NoBathRoom;
        $scope.priceSlider = {
            minValue: $scope.searchWithFilter.FilterPrice.MinValue,
            maxValue: $scope.searchWithFilter.FilterPrice.MaxValue,
            options: {
              floor: 0,
              ceil: 5000,
              step: 1
            },
          };
        $scope.areaSlider = {
            minValue: $scope.searchWithFilter.FilterArea.MinValue,
            maxValue: $scope.searchWithFilter.FilterArea.MaxValue,
            options: {
              floor: 0,
              step: 0.01,
              ceil: 300,
              precision: 3
            },
          };
        for (var j = 0; j < $scope.searchWithFilter.FilterFacility.FacilityIds.length; j++) {
            for (var i = 0; i < $scope.listFacility.length; i++) {
                if ($scope.searchWithFilter.FilterFacility.FacilityIds[j] == $scope.listFacility[i].Id) {
                    $scope.listFacility[i].Status = true;
                }
            };
        };
    }else{
        $scope.bedroom = 0;
        $scope.bathroom = 0;
        $scope.priceSlider = {
            minValue: 0,
            maxValue: 5000,
            options: {
              floor: 0,
              ceil: 5000,
              step: 1
            },
          };
        $scope.areaSlider = {
            minValue: 0,
            maxValue: 300,
            options: {
              floor: 0,
              step: 0.01,
              ceil: 300,
              precision: 3
            },
          };
    }
     $scope.defaultData = {
            "Page":"1",
            "Limit":"8",
            "Search":"",
            "FilterPrice":{
                "MinValue":"0",
                "MaxValue":"5000"
            },
            "FilterArea":{
                "MinValue":"0",
                "MaxValue":"300"
            },
            "FilterRoom":{
                "NoBedRoom":"0",
                "NoBathRoom":"0"
            },
            "FilterFacility":{
                "FacilityIds":[]
            }
        }
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

    function initData(){
        $timeout(function () {
            scope.$apply();
        }, 0);
        $scope.searchWithFilter.Page = $scope.bigCurrentPage === undefined ? 1 : $scope.bigCurrentPage;
        $scope.searchWithFilter.Limit = 8;
        $scope.searchWithFilter.FilterPrice={
            "MinValue":$scope.priceSlider.minValue,
            "MaxValue":$scope.priceSlider.maxValue
        }
        $scope.searchWithFilter.FilterArea={
            "MinValue":$scope.areaSlider.minValue,
            "MaxValue":$scope.areaSlider.maxValue
        }
        $scope.searchWithFilter.FilterRoom={
            "NoBedRoom":$scope.bedroom,
            "NoBathRoom":$scope.bathroom
        }
        $scope.searchWithFilter.FilterFacility={
            "FacilityIds":[]
        }
        for (var i = 0; i < $scope.listFacility.length; i++) {
            if ($scope.listFacility[i].Status) {
                $scope.searchWithFilter.FilterFacility.FacilityIds.push($scope.listFacility[i].Id)
            }
        }
        $scope.searchWithFilter.Search = scope.txtSearch === undefined ? "" : scope.txtSearch;
    }

    $scope.redirectVisitList = function(){
        if (localStorage && localStorage.getItem('user_profile')) {
            $location.path("/user-profile/general");
        }else{
            document.getElementById('modalLogin').click();
        }
    }

    function getListApartment(){
        xhrService.post("GetListApartment",$scope.searchWithFilter)
            .then(function (data) {
                $scope.totalItems = data.data.total;
                $scope.apartmentList = data.data.data;
                if($scope.apartmentList.length > 0){
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
                }
                
            },
            function (error) {
                console.log(error.statusText);
            });
    }

    $scope.loadApartment = function () {
        xhrService.get("GetAllFacilities").then(function (data) {
            $scope.listFacility = data.data;
            for (var i = 0; i < $scope.listFacility.length; i++) {
                $scope.listFacility[i].Status = false;
            };
            initData();
            getListApartment();
        },
        function (error) {
            $scope.errorText = error.statusText;
        });
        
        
        
    }

    $scope.clearFacility = function(){
        for (var i = 0; i < $scope.listFacility.length; i++) {
            $scope.listFacility[i].Status = false;
        };
    }

    $scope.saveToBookmark = function(apartment){
        if(localStorage.getItem('apartmentList')){
            var apartmentList = JSON.parse(localStorage.getItem('apartmentList'));
            var status = true;
            for (var i = 0; i < apartmentList.length; i++) {
                if (apartmentList[i].Id == apartment.Id) {
                    status = false;
                }
            }
            if(status){
                apartmentList.push(apartment);
                localStorage.setItem('apartmentList', JSON.stringify(apartmentList));
            }
        }else{
            var apartmentList = [];
            apartmentList.push(apartment);
            localStorage.setItem('apartmentList', JSON.stringify(apartmentList));
        }
        
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
                $scope.foreignTv.selected = $scope.foreignTvs[1];
                $scope.apartmentPrice = 1500;
                $scope.cleaning = 3;
                $scope.bottle = 4;
                $scope.basic = "service-basic";
                $scope.electricBill = 0;
                $scope.changeService();
            },
                function (error) {
                    console.log(error.statusText);
                });
        xhrService.get("GetAllFacilities")
            .then(function (data) {
                $scope.allFacility = data.data;
                for (var j = 0; j < $scope.allFacility.length; j++) {
                    for (var i = 0; i < $scope.data.FacilityList.length; i++) {
                        if ($scope.allFacility[j].Id == $scope.data.FacilityList[i].Id) {
                            $scope.allFacilities.push($scope.allFacility[j]);
                        }
                    }
                };
            },
            function (error) {
                console.log(error.statusText);
            });
        xhrService.post("GetListApartment",$scope.defaultData)
            .then(function (data) {
                $scope.apartmentList = data.data.data;
            },
                function (error) {
                    console.log(error.statusText);
                });

        $timeout(loadApartmentDetail, 4000);

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
        $scope.totalPrice = (($scope.apartment.Price + $scope.servicePrice) / extra).toFixed(2);
        $scope.perNightPrice = (($scope.totalPrice / 30) * 1.6).toFixed(2);
    }
    $scope.submitApartment = function(){
         if (localStorage && localStorage.getItem('user_profile')) {
           var dataApartment = {
            "ApartmentId":$scope.apartment.Id,
            "Bill":Number($scope.electricBill),
            "Cleaning":$scope.cleaning,
            "IsDetergent":document.getElementById("checkboxToilet").checked,
            "IsIncludeTax":document.getElementById("checkboxExtra").checked,
            "IsInternetWifi":document.getElementById("checkboxInternet").checked,
            "IsApartmentFee":document.getElementById("checkboxFee").checked,
            "ServicePrice":$scope.servicePrice,
            "TotalPrice":$scope.totalPrice,
            "TvType":$scope.foreignTvs.indexOf($scope.foreignTv.selected),
            "Water":$scope.bottle
        };
        xhrService.post("AddVisitList", dataApartment).then(function (data) {
            alert("Success!");
        },function (error) {
                $scope.errorText = error.statusText;
            });
        }else{
            document.getElementById('modalLogin').click();
        }
        
    }

}

app.controller('ApartmentCtrl', ApartmentCtrl);