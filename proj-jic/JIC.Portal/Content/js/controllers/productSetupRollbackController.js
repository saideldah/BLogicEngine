define(['angular', 'utils'], function (angular, utils) {

    var productSetupRollbackController = function ($scope, model, $http) {
        var rootShell = $scope.$root.rootShell;

        var getPreviousVersionList = function () {
            $scope.loading = true;
            var apiUri = "/api/ProductSetup/GetPreviousVersionList";
            rootShell.server.get(apiUri,
             {
                 success: function (response) {
                     $scope.loading = false;
                     $scope.previousVersionFileList = response.Data;
                     for (var i = 0; i < $scope.previousVersionFileList.length; i++) {
                         if ($scope.previousVersionFileList[i].Version == $scope.selectedVersion) {
                             $scope.productSetupFileUrl = "/library/api/LibraryFile/GetFile?path=" + $scope.previousVersionFileList[i].FilePath;
                         }
                     }
                 },
                 error: function (response) {
                     $scope.loading = false;
                     console.log(response);
                 }
             });
        };
        var getCurrentVersion = function () {
            $scope.loading = true;
            var apiUri = "/api/ProductSetup/Get";
            rootShell.server.get(apiUri,
             {
                 success: function (response) {
                     $scope.loading = false;
                     $scope.currentFile = response.Data;
                     $scope.selectedVersion = $scope.currentFile.Version - 1;
                     getPreviousVersionList();
                 },
                 error: function (response) {
                     $scope.loading = false;
                     console.log(response);
                 }
             });
        };
        $scope.changeDownloadUrl = function () {
            for (var i = 0; i < $scope.previousVersionFileList.length; i++) {
                if ($scope.previousVersionFileList[i].Version == $scope.selectedVersion) {
                    $scope.productSetupFileUrl = "/library/api/LibraryFile/GetFile?path=" + $scope.previousVersionFileList[i].FilePath;
                }
            }
        };
        $scope.submit = function () {
            var response = {
                actionType: "submit",
                data: $scope.selectedVersion
            }
            rootShell.modal.closeDialog(response);
        }
        $scope.cancel = function () {
            var response = {
                actionType: "cancel",
                data: ""
            }
            rootShell.modal.closeDialog(response);
        }

        var init = function () {
            
            getCurrentVersion();
        };

        init();
    };

    productSetupRollbackController.resolve = {
        model: function ($rootScope) {
            var rootShell = $rootScope.rootShell;
            var deferred = rootShell.$q.defer();
            var id = rootShell.route.params().id;
            deferred.resolve({ Version: 1 });

            return deferred.promise;
        }

    };

    return productSetupRollbackController;
});
