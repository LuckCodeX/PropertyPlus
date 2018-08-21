
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
        .state('home',
        {
            url: "/",
            templateUrl: "html/home.html",
            data: { pageTitle: 'Trang chủ' }
        })
        .state('blog',
        {
            url: "/blog",
            templateUrl: "html/blog.html",
            data: { pageTitle: 'Blog' }
        })
        .state('apartment',
        {
            url: "/apartment",
            templateUrl: "html/apartment.html",
            data: { pageTitle: 'Apartment' }
        })
        .state('apartmentdetail',
            {
                url: "/apartment-detail",
                templateUrl: "html/apartmentdetail.html",
                data: { pageTitle: 'Apartment' }
            })
        .state('project',
        {
            url: "/project",
            templateUrl: "html/project.html",
            data: { pageTitle: 'Project' }
        });
}


app.config(config)
    .run(function ($rootScope, $state, $stateParams) {
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
        $rootScope.$on('$stateChangeSuccess', function () {
            document.body.scrollTop = document.documentElement.scrollTop = 0;

        });
    });
