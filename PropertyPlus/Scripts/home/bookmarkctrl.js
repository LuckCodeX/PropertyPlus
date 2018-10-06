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
}
app.controller('BookmarkCtrl', BookmarkCtrl);