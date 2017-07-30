define('accountDirectives', ['angular', 'jQuery', 'accountController', 'utils'], function (angular, $, AccountController, utils) {
    angular.module('account.directives', [])
        .directive('amsTitle', function () {
            return {
                restrict: "A",
                link: function (scope, element, attrs) {
                    scope.$watch(attrs.amsTitle, function (value) {
                        element.attr('title', value);
                    });
                }
            }
        })
        .directive('crazyLogin', function ($http) {
             return {
                 restrict: "E",
                 templateUrl: '/content/templates/login.html',
                 replace: true,
                 transclude: false,
                 controller: AccountController,
                 link: function (scope, element, attrs) {
                     scope.loginModel = scope.loginModel || {};

                     scope.errorMessage = !utils.isEmpty(attrs.errorMessage) ? scope.$eval(attrs.errorMessage) || attrs.errorMessage : 'Invalid credentials.';
                     var errorMessage = !utils.isEmpty(attrs.showErrorMessage) ? scope.$eval(attrs.showErrorMessage) || attrs.showErrorMessage : false;
                     scope.loginModel.username = !utils.isEmpty(attrs.username) ? scope.$eval(attrs.username) || attrs.username : '';
                     scope.siteData.Title = !utils.isEmpty(attrs.title) ? scope.$eval(attrs.title) || attrs.title : '';
                     scope.siteData.WelcomeNote = !utils.isEmpty(attrs.welcomeNote) ? scope.$eval(attrs.welcomeNote) || attrs.welcomeNote : '';
                     scope.siteData.Providers = !utils.isEmpty(attrs.providers) ? scope.$eval(attrs.providers) || JSON.parse(attrs.providers) : [];
                     scope.siteData.LogoUrl = !utils.isEmpty(attrs.logoUrl) ? scope.$eval(attrs.logoUrl) || attrs.logoUrl : '/CDN/ams/images/logo.png';
                     scope.siteData.PostUrl = !utils.isEmpty(attrs.postUrl) ? scope.$eval(attrs.postUrl) || attrs.postUrl : '/Account/Login';
                     scope.siteData.ReturnUrl = !utils.isEmpty(attrs.returnUrl) ? scope.$eval(attrs.returnUrl) || attrs.returnUrl : '';
                     //scope.siteId = !utils.isEmpty(attrs.siteId) ? scope.$eval(attrs.siteId) || attrs.siteId : null;
                     scope.additionalData = !utils.isEmpty(attrs.additionalData) ? scope.$eval(attrs.additionalData) || attrs.additionalData : null;

                     if (errorMessage) {
                         scope.showError(errorMessage);
                     }

                     var stylesPath = '/styles';
                     if (!$('link[href="' + stylesPath + '"]').length) {
                         var style = document.createElement("link");
                         style.setAttribute("type", "text/css");
                         style.setAttribute("rel", "stylesheet");
                         style.setAttribute("href", stylesPath);
                         jQuery("head")[0].appendChild(style);
                     }

                     if (utils.isEmpty(scope.siteData.Title) || utils.isEmpty(scope.siteData.Providers)) {
                         $http.get('/Account/GetSiteLoginData/')
                             .success(function (response) {
                                 scope.siteData = response;
                                 scope.siteData.Providers = scope.siteData.SupportedIdentityProviderTypes;
                                 scope.setOtherProviders();
                             });
                     } else {
                         scope.setOtherProviders();
                     }
                 }
             }
         })
});