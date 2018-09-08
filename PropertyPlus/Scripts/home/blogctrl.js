function BlogCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    var limit = 12;

    $scope.pageChanged = function () {
        $location.path("/blog").search({ page: $scope.bigCurrentPage, limit: limit, type: $scope.type, search: $scope.search });
    }

    $scope.loadData = function() {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        $scope.type = $stateParams.type === undefined ? -1 : $stateParams.type;
        $scope.search = $stateParams.search === undefined ? '' : $stateParams.search;
        xhrService.get("GetListBlog/" + $scope.bigCurrentPage + "/" + limit + "/" + $scope.type + "/" + $scope.search)
            .then(function(data) {
                    $scope.blogList = data.data.data;
                },
                function(error) {
                    console.log(error.statusText);
                });
    };

    $scope.loadDetail = function(id) {
        $scope.blog = $scope.blogList.filter((p) => p.Id == id)[0];
    };
}

app.controller('BlogCtrl', BlogCtrl);