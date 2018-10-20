function HostmanageCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll,
    $window) {

    $scope.loadHostManage=function(){
        xhrService.get("GetYourListApartment/-1").then(function (data) {
            $scope.yourApartmentlist=data.data;

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
    };

$scope.changeAparment=function(){

        xhrService.get("GetApartmentInformation/"+ $scope.apartmentId).then(function (data) {
            console.log(data);
            $scope.apartmentchoice=data.data;
            // $scope.apartmentchoicepj=data.data.Project;
                        // console.log(data.data.Project);
            $scope.editorOptions = {
                language: 'vi'
            };
            $scope.images = [];
            $scope.banner_img=[];
           //loi di vcl console log 1 kieu hien 1 view 1 kieu :()
            for (var i = 0; i < $scope.apartmentchoice.ImgList.length; i++) {
                if ($scope.apartmentchoice.ImgList[i].Type == 0) {
                         $scope.banner_img.push($scope.apartmentchoice.ImgList[i]);
                    
                }else if($scope.apartmentchoice.ImgList[i].Type == 2 ||
                         $scope.apartmentchoice.ImgList[i].Type == 3 ||
                         $scope.apartmentchoice.ImgList[i].Type == 4){
                    $scope.images.push($scope.apartmentchoice.ImgList[i]);
                }
            };
            
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
        if ($scope.data.Latitude == undefined ||
            $scope.data.Latitude == 0 ||
            $scope.data.Longitude == undefined ||
            $scope.data.Longitude == 0 ||
            $scope.data.Longitude == $scope.oldLong ||
            $scope.data.Latitude == $scope.oldLat) {
            $scope.statusAddress = false;

        } else {
            $scope.oldLat = $scope.data.Latitude;
            $scope.oldLong = $scope.data.Longitude;
            $scope.statusAddress = true;
        }
    }

    $scope.uploadImg = function (event) {
        $scope.$watchCollection('images', function() {
            if (($scope.images[$scope.images.length-1].url != null && $scope.images[$scope.images.length-1].Img == null )||
                ($scope.images[$scope.images.length-1].url == null && $scope.images[$scope.images.length-1].Img != null )) {
                if ($scope.images.length <6) {
                    $scope.images.push({ file: null, url: null });
                    return;
                }
                
            }
         });
        console.log($scope.banner_img);
    };

    $scope.saveHostManage = function(){
        $scope.apartmentchoice.ImgList = [];
        if($scope.banner_img[0].Img != null){
            $scope.apartmentchoice.ImgList.push($scope.banner_img);
        }else if ($scope.banner_img[0].url != null) {
            $scope.apartmentchoice.ImgList.push({
                "Img_Base64": $scope.banner_img[0].url,
                "Type": 0
            });
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
        
        xhrService.post("SaveApartment", $scope.apartmentchoice).then(function (data) {
           
            
        },function (error) {
                $scope.errorText = error.statusText;
            });
    }
}

app.controller('HostmanageCtrl', HostmanageCtrl);