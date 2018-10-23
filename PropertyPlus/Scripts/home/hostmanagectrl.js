function HostmanageCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll,
    $window,
    $state,
    shareData) {

    $scope.loadHostManage=function(){
        
        xhrService.get("GetYourListApartment/-1").then(function (data) {
            $scope.yourApartmentlist=data.data;
            $scope.apartmentId = shareData.get('apartmentIdVisit');
            $scope.changeAparment();
        },
         function (error) {
            console.log(error.statusText);
        });

        xhrService.get("GetAllProject").then(function (data) {
            $scope.projectList=data.data;

        },
         function (error) {
            console.log(error.statusText);
        });

        xhrService.get("GetUserProfile/").then(function (data) {
            $scope.userProfile=data.data;

        },
         function (error) {
            console.log(error.statusText);
        });




    };
$scope.isDisabled = true;
$scope.changeAparment=function(){

        xhrService.get("GetApartmentInformation/"+ $scope.apartmentId).then(function (data) {
            // console.log(data);
            $scope.apartmentchoice=data.data;
            $scope.isDisabled = false;
            $scope.editorOptions = {
                language: 'vi'
            };
            $scope.images = [];
            $scope.banner_img=[];
            $scope.uploadImg();
            for (var i = 0; i < $scope.apartmentchoice.ImgList.length; i++) {
                if ($scope.apartmentchoice.ImgList[i].Type == 0) {
                         $scope.banner_img.push($scope.apartmentchoice.ImgList[i]);
                    
                }else{
                    $scope.images.push($scope.apartmentchoice.ImgList[i]);
                }
            };
            var myLatLng = { lat: $scope.apartmentchoice.Latitude, lng: $scope.apartmentchoice.Longitude };
            if (document.getElementById('map')) {
                var map = new google.maps.Map(document.getElementById('map'),
                {
                    zoom: 17,
                    center: myLatLng
                });
                var marker = new google.maps.Marker({
                    position: myLatLng,
                    map: map,
                    title: $scope.apartmentchoice.Address
                });
            }
        
            
        },
         function (error) {
            console.log(error.statusText);
        });
    };
    
    $scope.loadPhoto = function(){
        if ($scope.images == undefined) {
            $scope.images = [];
            $scope.images.push({ file: null, url: null });
        }
        $scope.oldLengthImg = 1;
    }

    $scope.changeAddress = function () {
        if ($scope.apartmentchoice.Latitude == undefined ||
            $scope.apartmentchoice.Latitude == 0 ||
            $scope.apartmentchoice.Longitude == undefined ||
            $scope.apartmentchoice.Longitude == 0 ||
            $scope.apartmentchoice.Longitude == $scope.oldLong ||
            $scope.apartmentchoice.Latitude == $scope.oldLat) {
            $scope.statusAddress = false;

        } else {
            $scope.oldLat = $scope.apartmentchoice.Latitude;
            $scope.oldLong = $scope.apartmentchoice.Longitude;
            $scope.statusAddress = true;
        }
    };

    $scope.loadlocation = function () {
        $scope.changeAparment();
        
    }

    $scope.uploadImg = function (event) {
        $scope.$watchCollection('images', function() {
            if($scope.images.length==0){
                $scope.images.push({ file: null, url: null });
            }else if (($scope.images[$scope.images.length-1].url != null && $scope.images[$scope.images.length-1].Img == null )||
                ($scope.images[$scope.images.length-1].url == null && $scope.images[$scope.images.length-1].Img != null )) {
                if ($scope.images.length <6) {
                    $scope.images.push({ file: null, url: null });
                    return;
                }
                
            }
         });
    };

    $scope.saveHostManage = function(){
        $scope.apartmentchoice.ImgList = [];
        if($scope.banner_img.length > 0){
            if($scope.banner_img[0].Img != null){
                $scope.apartmentchoice.ImgList.push($scope.banner_img[0]);
            }else if ($scope.banner_img[0].url != null) {
                $scope.apartmentchoice.ImgList.push({
                    "Img_Base64": $scope.banner_img[0].url,
                    "Type": 0
                });
            }
        }
        
       
        if ($scope.images.length > 1) {
            if ($scope.images[$scope.images.length - 1].url == null && $scope.images[$scope.images.length - 1].Img == null) {
                $scope.images.splice(-1, 1);
            }
            for (var i = 0; i < $scope.images.length; i++) {
                if ($scope.images[i].Img != null) {
                    $scope.apartmentchoice.ImgList.push($scope.images[i]);
                }else{
                    var item = {
                        "Img_Base64": $scope.images[i].url,
                        "Type": -1
                    }
                    $scope.apartmentchoice.ImgList.push(item);
                }
                
            }
        };
        // console.log($scope.apartmentchoice);
        xhrService.post("SaveApartment", $scope.apartmentchoice).then(function (data) {
           
            
        },function (error) {
                $scope.errorText = error.statusText;
            });
    }
}

app.controller('HostmanageCtrl', HostmanageCtrl);