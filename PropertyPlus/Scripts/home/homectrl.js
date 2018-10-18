function HomeCtrl($scope, $rootScope, $stateParams, $location, $timeout, xhrService, $anchorScroll, $translate,$state) {
    $scope.loadData = function () {
        $scope.bedroom = 0;
        $scope.bathroom = 0;
        $scope.searchWithFilter = {};
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
        if (localStorage && localStorage.getItem('language')) {
            $translate.use(localStorage.getItem('language'));
        }
        xhrService.get("GetSlide/0").then(function (data) {
                $scope.slideImg = "Upload/" + data.data.Img;
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
        xhrService.get("GetAllFacilities").then(function (data) {
                $scope.listFacility = data.data;
                for (var i = 0; i < $scope.listFacility.length; i++) {
                    $scope.listFacility[i].Status = false;
                };
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
        xhrService.get("GetSlide/3").then(function (data) {
                $scope.slideIntroduct = "Upload/" + data.data.Img;
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
        xhrService.get("GetSlide/4").then(function (data) {
                $scope.slideService = "Upload/" + data.data.Img;
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
        xhrService.post("GetListApartment",$scope.defaultData)
            .then(function (data) {
                    $scope.apartmentList = data.data.data;
                },
                function (error) {
                    console.log(error.statusText);
                });

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
            if(statusFilter){
                $('.btn-facilities.active').removeClass("active");
                $('.card-filter.active').removeClass("active");
            }
            statusFilter=true; 
        });
    }

    $scope.clearFacility = function(){
        for (var i = 0; i < $scope.listFacility.length; i++) {
            $scope.listFacility[i].Status = false;
        };
    }

    $scope.submitSearch = function(){
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
        $scope.searchWithFilter.Page=1;
        $scope.searchWithFilter.Page=8;
        for (var i = 0; i < $scope.listFacility.length; i++) {
            if ($scope.listFacility[i].Status) {
                $scope.searchWithFilter.FilterFacility.FacilityIds.push($scope.listFacility[i].Id)
            }
        }
        var scope = angular.element('body[ng-controller="MainCtrl"]').scope();
        scope.searchData = $scope.searchWithFilter;
        scope.txtSearch = $scope.searchWithFilter.Search;
        $timeout(function () {
            scope.$apply();
        }, 0);
        $state.go('apartment');
    }
}

app.controller('HomeCtrl', HomeCtrl);