require.config({
    paths: {
        'common': '/cdn/ams/scripts/common',
        'siteModule': '/content/js/siteModule',
        'amsConfiguration': '/content/js/amsConfiguration',
        'accountDirectives': '~/content/account/accountDirectives',
        'accountController': '~/content/account/accountController'
    }
});

//First import the AMS framework
require(['common'], function () {
    //bootstrap the angular application (with module name 'application').
    require(['angular', 'angular-ui', 'siteModule', 'angular-ui-shiv'], function (angular) {
        //Second import the installed applications configuration
        $.get('/api/ApplicationList/GetApplications', function (applications) {
            var applicationModules = [];
            var applicationCommonPaths = [];
            var applicationNames = ['site1']; // Change 'site1' to the name of your site module
            angular.forEach(applications, function (application) {
                var applicationName = application.Name.charAt(0).toLowerCase() + application.Name.slice(1);
                applicationNames.push(applicationName + 'App');
                applicationCommonPaths.push('/' + application.Name + '/Content/js/config.js');
                applicationModules.push(applicationName + 'AppModule');

            });

            require(applicationCommonPaths, function () {
                require(applicationModules, function () {
                    angular.bootstrap($('html'), applicationNames);
                }, function (err) {
                    var failedId = err.requireModules && err.requireModules[0];
                    requirejs.undef(failedId);
                    throw new Error("'{applicationName}AppModule.js' was not found under 'Content > js' in your application! Make sure your module name is '{applicationName}App'");
                });
            }, function (err) {
                var failedId = err.requireModules && err.requireModules[0];
                requirejs.undef(failedId);
                throw new Error("'config.js' was not found under 'Content > js' in your application");
            });

        });
    });
});