﻿function BlogCtrl($scope,
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

    $scope.loadData = function () {
        $scope.bigCurrentPage = $stateParams.page === undefined ? 1 : $stateParams.page;
        $scope.type = $stateParams.type === undefined ? -1 : $stateParams.type;
        $scope.search = $stateParams.search === undefined ? '' : $stateParams.search;
        xhrService.get("GetListBlog/" + $scope.bigCurrentPage + "/" + limit + "/" + $scope.type + "/" + $scope.search)
            .then(function (data) {
                $scope.blogList = data.data.data;
            },
                function (error) {
                    console.log(error.statusText);
                });
        xhrService.get("GetSlide/2").then(function (data) {
            $scope.slideImg = "Upload/" + data.data.Img;
        },
            function (error) {
                $scope.errorText = error.statusText;
            });
    };

    $scope.loadDetailData = function () {
        var id = $stateParams.id;
        xhrService.get("GetBlogDetail/" + id).then(function (data) {
            $scope.blog = data.data;
            console.log(data);
        },
            function (error) {
                $scope.errorText = error.statusText;
            });
    };
}

app.controller('BlogCtrl', BlogCtrl);