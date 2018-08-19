
function config($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, $locationProvider, IdleProvider) {

    // Configure Idle settings
    IdleProvider.idle(5); // in seconds
    IdleProvider.timeout(120); // in seconds

    $urlRouterProvider.otherwise("/");

    $ocLazyLoadProvider.config({
        // Set to true if you want to see what and when is dynamically loaded
        debug: false
    });
    $locationProvider.html5Mode(true);

    $stateProvider
        .state('home', {
            url: "/",
            templateUrl: "html/home.html",
            data: { pageTitle: 'Trang chủ' }
        })
        .state('blog', {
            url: "/blog",
            templateUrl: "html/blog.html",
            data: { pageTitle: 'Blog' }
        })
        .state('detail', {
            url: "/detail/:id",
            templateUrl: "html/detail.html",
            data: { pageTitle: 'Xác nhận đơn hàng' }
        });
}


app.config(config);
