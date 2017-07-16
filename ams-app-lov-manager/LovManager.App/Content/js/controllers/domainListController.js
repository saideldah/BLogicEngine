

define(['angular', 'utils'], function (angular, utils) {

    var domainListController = function ($scope, model, $http) {
        var rootShell = $scope.$root.rootShell;

        $scope.goToDomain = function (domainId) {
            if (domainId == null) {
                rootShell.navigation.goTo('/page/app/lovmanager/createDomain.html');
            }
            else {
                rootShell.navigation.goTo('/page/app/lovmanager/editDomain.html?id='+domainId);
            }
        };

        var rootShell = $scope.$root.rootShell;
        var isUpdate = false;

        //init
        var getAllDomains = function () {
            $scope.loading = true;
            var apiUri = "/lovmanager/api/Domain/Get";
            rootShell.server.get(apiUri,
             {
                 success: function (response) {
                     $scope.loading = false;
                     $scope.domainList = response.Data;
                 },
                 error: function (response) {
                     console.log(response);
                 }
             });
        };
        var init = function () {
            $scope.parentDomainId = "";
            var id = rootShell.route.params().id;

            if (id != null && id != "" && typeof (id) != "undefined") {
                isUpdate = true;
            }
            if (isUpdate) {
                $scope.downloadBtnTitle = "Download LOV File";
                $scope.hideLovTable = false;
                $scope.downloadFileUri = "/lovmanager/api/Template/Get";
            }
            else {
                $scope.downloadBtnTitle = "Download LOV Template";
                $scope.hideLovTable = true;
                $scope.downloadFileUri = "/lovmanager/api/Template/Get";
            }


            getAllDomains();
        };
        init();
    };
    domainListController.resolve = {
        model: function ($rootScope) {
            var rootShell = $rootScope.rootShell;
            var deferred = rootShell.$q.defer();
            var id = rootShell.route.params().id;
            deferred.resolve({ Version: 1 });

            return deferred.promise;
        }

    };

    return domainListController;
});
