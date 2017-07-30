define('SiteModule', ['angular', 'AmsModule', 'accountDirectives', 'jicServices'], function (angular, $) {
    var Module = angular.module('SiteModule', ['AmsModule', 'account.directives'], function() {
    });
    Module.run(function ($rootScope, $window, $location) {
        var rootShell = $rootScope.rootShell;
        rootShell.bankingProvider = $window.bankingProvider;
        $rootScope.login = function () {
            if ($rootScope.rootShell.identity.Client && $rootScope.rootShell.identity.Client[1]) {
                $window.location = '/SignOut';
            } else {
                $window.location = '/SignIn';
            }
        };
        $rootScope.$on('IdentityInitialized', function () {
            var url = $location.url();

            rootShell.navigation.initialize(function () {
                if (!url || url == '/') {
                    rootShell.navigation.goTo('/page/index.html');
                }
            }, true);
        });
    });
    return Module;
});

