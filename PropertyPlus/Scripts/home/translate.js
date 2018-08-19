var translationsVI = {
    menu_customer: "Hỗ trợ",
    menu_signup: "Đăng ký",

    btn_register: "Đăng ký",

}

var translationsEN = {
    menu_blog: "Blog";
    menu_customer: "Support",
    menu_signup: "Sign up",
    menu_help: "Help",
    menu_login: "Login",
    menu_visit: "Your visit list",
    menu_host: "Become a host";
    btn_register: "Signup",
   
}

var translationsJP = {
    menu_customer: "サポート",
    menu_signup: "サインアップ",

    btn_register: "サインアップ",

}

app.config(['$translateProvider', function ($translateProvider) {
    // add translation tables
    $translateProvider.translations('en', translationsEN);
    $translateProvider.translations('vi', translationsVI);
    $translateProvider.translations('jp', translationsJP);
    $translateProvider.preferredLanguage('en');
    $translateProvider.fallbackLanguage('vi');
    $translateProvider.fallbackLanguage('jp');
}]);