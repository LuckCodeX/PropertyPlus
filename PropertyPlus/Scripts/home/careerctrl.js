function CareerCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {

	$scope.pageChanged = function () {
        $location.path("/career").search({ page: $scope.bigCurrentPage, limit: $scope.limit, type: $scope.type, search: $scope.search });
    };


// marketing
    $scope.loadData = function () {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        $scope.type = $stateParams.type === undefined ? 0 : $stateParams.type;
        $scope.search = $stateParams.search === undefined ? '' : $stateParams.search;

        xhrService.get("GetListCareer/" + $scope.bigCurrentPage + "/" + $scope.limit + "/" + $scope.type  + "/" + $scope.search)
            .then(function (data) {
            	console.log(data);
            	$scope.careerList = data.data.data;                
                
            },
                function (error) {
                    console.log(error.statusText);
                });


       
    };
//sale
    $scope.loadData1 = function () {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        $scope.type = $stateParams.type === undefined ? 1 : $stateParams.type;
        $scope.search = $stateParams.search === undefined ? '' : $stateParams.search;

        xhrService.get("GetListCareer/" + $scope.bigCurrentPage + "/" + $scope.limit + "/" + $scope.type  + "/" + $scope.search)
            .then(function (data) {
            	// console.log(data);
            	$scope.careerList1 = data.data.data;                
                
            },
                function (error) {
                    console.log(error.statusText);
                });


       
    };
//accounting
    $scope.loadData2 = function () {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        $scope.type = $stateParams.type === undefined ? 2 : $stateParams.type;
        $scope.search = $stateParams.search === undefined ? '' : $stateParams.search;

        xhrService.get("GetListCareer/" + $scope.bigCurrentPage + "/" + $scope.limit + "/" + $scope.type  + "/" + $scope.search)
            .then(function (data) {
            	// console.log(data);
            	$scope.careerList2 = data.data.data;                
                
            },
                function (error) {
                    console.log(error.statusText);
                });


       
    };

    // development
$scope.loadData3 = function () {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        $scope.type = $stateParams.type === undefined ? 3 : $stateParams.type;
        $scope.search = $stateParams.search === undefined ? '' : $stateParams.search;

        xhrService.get("GetListCareer/" + $scope.bigCurrentPage + "/" + $scope.limit + "/" + $scope.type  + "/" + $scope.search)
            .then(function (data) {
                // console.log(data);
                $scope.careerList3 = data.data.data;                
                
            },
                function (error) {
                    console.log(error.statusText);
                });


       
    };

//management
$scope.loadData4 = function () {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        $scope.type = $stateParams.type === undefined ? 4 : $stateParams.type;
        $scope.search = $stateParams.search === undefined ? '' : $stateParams.search;

        xhrService.get("GetListCareer/" + $scope.bigCurrentPage + "/" + $scope.limit + "/" + $scope.type  + "/" + $scope.search)
            .then(function (data) {
                // console.log(data);
                $scope.careerList4 = data.data.data;                
                
            },
                function (error) {
                    console.log(error.statusText);
                });


       
    };
//developer

$scope.loadData5 = function () {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        $scope.type = $stateParams.type === undefined ? 5 : $stateParams.type;
        $scope.search = $stateParams.search === undefined ? '' : $stateParams.search;

        xhrService.get("GetListCareer/" + $scope.bigCurrentPage + "/" + $scope.limit + "/" + $scope.type  + "/" + $scope.search)
            .then(function (data) {
                // console.log(data);
                $scope.careerList5 = data.data.data;                
                
            },
                function (error) {
                    console.log(error.statusText);
                });


       
    };

$scope.limit = 6;

$scope.loadDetailData = function () {
        var id = $stateParams.id;
        xhrService.get("GetCareerDetail/" + id).then(function (data) {
        	// console.log(data);
            $scope.career = data.data;
            
        },
            function (error) {
                $scope.errorText = error.statusText;
            });
    };






}

app.controller('CareerCtrl', CareerCtrl);