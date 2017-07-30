

define(['angular', 'utils'], function (angular, utils) {

    var indexController = function ($scope, model, $http) {


    };
    indexController.resolve = {
        model: function ($rootScope) {
            var rootShell = $rootScope.rootShell;
            var deferred = rootShell.$q.defer();
            var id = rootShell.route.params().id;
            deferred.resolve({ Version: 1 });

            return deferred.promise;
        }

    };

    return indexController;
});
