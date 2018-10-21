function BookmarkCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    $sce,
    xhrService,
    $anchorScroll) {
	$scope.loadBookmark = function(){
		if (localStorage.getItem('apartmentList')) {
			$scope.apartmentList = JSON.parse(localStorage.getItem('apartmentList'));
		}
	}
	$scope.deleteApartment = function(index){
		$scope.apartmentList.splice(index,1);
		localStorage.setItem('apartmentList', JSON.stringify($scope.apartmentList));
	}
	$scope.sendVisitList = function(){
		var data = {
			Items:[]
		};
		for (var i = 0; i < $scope.apartmentList.length; i++) {
			data.Items.push($scope.apartmentList[i].service)
		}
		xhrService.post("AddVisitList",data)
        .then(function (data) {
            localStorage.removeItem('apartmentList');
			$scope.apartmentList=[];
			$location.path("/user-profile/general");
        },function (error) {
                console.log(error.statusText);
            });
		
	}
}
app.controller('BookmarkCtrl', BookmarkCtrl);