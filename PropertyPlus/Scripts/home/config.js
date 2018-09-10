
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
                url: "/blog?page&limit&type&search",
                templateUrl: "html/blog.html",
                data: { pageTitle: 'Blog' }
            })
        .state('blogdetail',
            {
                url: "/blog-detail/:id/:title",
                templateUrl: "html/blogdetail.html",
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
            })
        .state('userprofile',
            {
                url: "/user-profile",
                abstract: true,
                templateUrl: "html/userprofile.html",
                data: { pageTitle: 'User Profile' }
            })
        .state('userprofile.general',
            {
                url: "/general",
                templateUrl: "html/userprofilegeneral.html",
                data: { pageTitle: 'User Profile' }
            })
        .state('userprofile.manage',
            {
                url: "/manage",
                templateUrl: "html/userprofilemanage.html",
                data: { pageTitle: 'User Profile' }
            })
        .state('userprofile.edit',
            {
                url: "/edit",
                templateUrl: "html/userprofileedit.html",
                data: { pageTitle: 'User Profile' }
            })
        .state('userprofile.setting',
            {
                url: "/setting",
                templateUrl: "html/userprofilesetting.html",
                data: { pageTitle: 'User Profile' }
            })
        .state('userprofile.refer',
            {
                url: "/refer",
                templateUrl: "html/userprofilerefer.html",
                data: { pageTitle: 'User Profile' }
            })
        .state('host',
            {
                url: "/host",
                abstract: true,
                templateUrl: "html/host.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.dashboard',
            {
                url: "/dashboard",
                templateUrl: "html/hostdashboard.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.listing',
            {
                url: "/listing",
                templateUrl: "html/hostlisting.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create',
            {
                url: "/create",
                abstract: true,
                templateUrl: "html/hostcreate.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create.step11',
            {
                url: "/step-1-1",
                templateUrl: "html/hostcreatestep11.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create.step12',
            {
                url: "/step-1-2",
                templateUrl: "html/hostcreatestep12.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create.step13',
            {
                url: "/step-1-3",
                templateUrl: "html/hostcreatestep13.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create.step21',
            {
                url: "/step-2-1",
                templateUrl: "html/hostcreatestep21.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create.step22',
            {
                url: "/step-2-2",
                templateUrl: "html/hostcreatestep22.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create.step31',
            {
                url: "/step-3-1",
                templateUrl: "html/hostcreatestep31.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create.step32',
            {
                url: "/step-3-2",
                templateUrl: "html/hostcreatestep32.html",
                data: { pageTitle: 'Host' }
            })
        .state('host.create.step4',
            {
                url: "/step-4",
                templateUrl: "html/hostcreatestep4.html",
                data: { pageTitle: 'Host' }
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
