var app = angular.module('propertyplus', [
    'ngCookies',
    'ui.router',                    // Routing
    'oc.lazyLoad',                  // ocLazyLoad
    'ui.bootstrap',                 // Ui Bootstrap
    'ngIdle',                       // Idle timer
    //'ui.select',
    'ngSanitize',                   // ngSanitize
    //'ngCsv',
    'ngAnimate',
    //'ngclipboard',
    'pascalprecht.translate',
    'ngImgCrop'            //ngImgCrop
]);

var API = "http://localhost:10918/api/propertyplus/";