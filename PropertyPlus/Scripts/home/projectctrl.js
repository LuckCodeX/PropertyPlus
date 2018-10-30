function ProjectCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll) {
    $scope.loadProject = function(){
        xhrService.get("GetSlide/1").then(function (data) {
                $scope.slideImg = "Upload/" + data.data.Img;
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
        xhrService.get("GetAllProject").then(function (data) {
            console.log(data);
                $scope.listProject = data.data;
                $scope.listIdProject = {};
                $scope.listProjectFeature = [];
                for (var i = 0; i <$scope.listProject.length ; i++){
                    xhrService.get("GetListApartmentByProjectId/"+$scope.listProject[i].Id).then(function (data) {
                        var id = Number(getSecondPart('GetListApartmentByProjectId/',data.config.url));
                            $scope.listIdProject[id]= data.data.length;
                        },
                        function (error) {
                            $scope.errorText = error.statusText;
                        });
                    if($scope.listProject[i].Type == 1){
                        $scope.listProjectFeature.push($scope.listProject[i]);
                    }
                }
                $(document).ready(function(){
                    $('#nghia2').owlCarousel({
                        loop: true,
                        margin:15,
                        nav: true,
                        items: 2
                    });
                    $("#nghia").owlCarousel({
                        loop: true,
                        nav: true,
                        margin:10,
                        items: 1
                    });
                    $("#nghia3").owlCarousel({
                        loop: true,
                        nav: true,
                        margin:10,
                        items: 4
                    });
                });
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
    }

    function getApartmentByProject(id){
        return
    }

    function getSecondPart(reg,str) {
        return str.split(reg)[1];
    }

    $scope.loadProjectDetail = function () {
        xhrService.get("GetProjectDetail/"+$stateParams.id).then(function (data) {
                $scope.project = data.data;
                var myLatLng = { lat: $scope.project.Latitude, lng: $scope.project.Longitude };
                var map = new google.maps.Map(document.getElementById('map'),
                    {
                        zoom: 17,
                        center: myLatLng
                    });
                var marker = new google.maps.Marker({
                    position: myLatLng,
                    map: map,
                    title: $scope.project.Address
                });
                xhrService.get("GetListApartmentByProjectId/"+$stateParams.id).then(function (data) {
                    console.log(data);
                        $scope.listApartmentListing= data.data;
                        $(document).ready(function(){
                            $('#listing.owl-carousel').owlCarousel({
                                margin: 30,
                                autoplay: true,
                                autoplayTimeout: 2500,
                                autoplayHoverPause: true,
                                loop: true,
                                items: 4,
                                nav: true,
                                animateOut: 'fadeOut'
                            });
                            $('#projectSlide.owl-carousel').owlCarousel({
                                margin: 30,
                                autoplay: true,
                                autoplayTimeout: 2500,
                                autoplayHoverPause: true,
                                loop: true,
                                items: 1,
                                nav: true,
                                animateOut: 'fadeOut'
                            });
                            $('.owl-nav').removeClass('disabled');
                        });
                    },
                    function (error) {
                        $scope.errorText = error.statusText;
                    });
            },
            function (error) {
                $scope.errorText = error.statusText;
            });
    }
}

app.controller('ProjectCtrl', ProjectCtrl);