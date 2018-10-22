function MainCtrl($scope,
    $rootScope,
    $stateParams,
    $location,
    $timeout,
    xhrService,
    $anchorScroll,
    $window) {

    $scope.user = {};

    $scope.redirectVisitList = function(){
        if (localStorage && localStorage.getItem('user_profile')) {
            $location.path("/user-profile/general");
        }else{
            document.getElementById('modalLogin').click();
        }
    }

    $scope.redirectHost = function(){
        if (localStorage && localStorage.getItem('user_profile')) {
            $location.path("/host/listing");
        }else{
            document.getElementById('modalLogin').click();
        }
    }

    $scope.submitSearch = function(txtSearch){
        $location.path("/apartment");
        $scope.searchForm();   
    }

    $scope.changeSearchTxt = function(txtSearch){
        $scope.txtSearch = txtSearch;
    }

    $scope.loadData = function () {
        if (localStorage && localStorage.getItem('language')) {
            $translate.use(localStorage.getItem('language'));
        }
        if (localStorage && localStorage.getItem('user_profile')) {
            $scope.userProfile = JSON.parse(Base64.decode(localStorage.getItem('user_profile')));
        }

        var statusFilter = true;
        $('body').on('click','.list-btn-facilities .group-btn-facility .btn-facilities',function(){
            var atrrb = $(this).attr("target-filter");
            $('.list-btn-facilities .group-btn-facility .card-filter').each(function(){
                if($(this).attr("id") != atrrb.replace("#","")){
                    $(this).removeClass("active");
                    $(this).parent().children('.btn-facilities').removeClass("active");
                } 
            });
            $(this).toggleClass('active');
            $(atrrb).toggleClass('active');
            statusFilter=false;
        });
        $('body').on("mouseup",".list-btn-facilities .group-btn-facility .card-filter",function(event){
           statusFilter=false;
        });
         $("body").mouseup(function(){ 
            if(statusFilter){
                $('.btn-facilities.active').removeClass("active");
                $('.card-filter.active').removeClass("active");
            }
             statusFilter=true; 
        });

        //xhrService.get("GetListBlog/1/6/-1/").then(function (data) {
        //    $scope.blogList = data.data.data;
        //}, function (error) { });
    };

    $scope.replaceString = function (str) {
        if (!str)
            return null;
        str = str.toLowerCase();
        str = str.replace(/\ /g, "-");
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/\”|\“|\"|\[|\]|\?/g, "");
        return str;
    };

    $scope.logout = function () {
        localStorage.removeItem('user_profile');
        $scope.userProfile = undefined;
        $location.path("/");
    };

    $scope.register = function () {
        if ($scope.user.password !== $scope.user.confirm_password) {
            $scope.errorText = "Confirm password does not match";
            return;
        }
        xhrService.post("Register", $scope.user).then(function (data) {
            localStorage.setItem('user_profile', Base64.encode(JSON.stringify(data.data)));
            $scope.userProfile = data.data;
            $('#myModal-email').modal('hide');
        },function (error) {
                $scope.errorText = error.statusText;
            });
    };

    $scope.login = function () {
        xhrService.post("Login", $scope.user).then(function (data) {
            localStorage.setItem('user_profile', Base64.encode(JSON.stringify(data.data)));
            $scope.userProfile = data.data;
            $('#myModal-login').modal('hide');
        },function (error) {
                $scope.errorText = error.statusText;
            });
    };

    $scope.submitGoogle = function(){
        $scope.profileGoogle = $window.profileGoogle;
        xhrService.post("LoginGoogle", $scope.profileGoogle).then(function (data) {
            localStorage.setItem('user_profile', Base64.encode(JSON.stringify(data.data)));
            $scope.userProfile = data.data;
            $('#myModal-signup').modal('hide');
            $('#myModal-login').modal('hide');
        },function (error) {
                $scope.errorText = error.statusText;
            });
    };


    $scope.submitFacebook = function(){
        $scope.profileFacebook = $window.profileFacebook;
         // console.log($scope.profileFacebook);
        xhrService.post("LoginFacebook", $scope.profileFacebook).then(function (data) {
            console.log(data);
            localStorage.setItem('user_profile', Base64.encode(JSON.stringify(data.data)));
            $scope.userProfile = data.data;
            $('#myModal-signup').modal('hide');
            $('#myModal-login').modal('hide');
        },function (error) {
                $scope.errorText = error.statusText;
            });
    }

}

app.controller('MainCtrl', MainCtrl);