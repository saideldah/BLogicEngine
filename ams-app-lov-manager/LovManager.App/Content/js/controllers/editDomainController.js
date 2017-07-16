

define(['angular', 'utils'], function (angular, utils) {

    var editDomainController = function ($scope, model, $http) {
        var rootShell = $scope.$root.rootShell;
        var id = rootShell.route.params().id;


        //init
        var getDomain = function () {
            $scope.loading = true;
            var apiUri = "/lovmanager/api/Domain/Get?id=" + id;
            rootShell.server.get(apiUri,
             {
                 success: function (response) {
                     $scope.loading = false;
                     $scope.domain = response.Data;
                     if ($scope.domain.ParentDomain.Code == null) {
                         $scope.parentDomain = "No Parent Domain";
                     }
                     else {
                         $scope.parentDomain = $scope.domain.ParentDomain.Name;
                     }
                 },
                 error: function (response) {
                     console.log(response);
                 }
             });
        };
        var init = function () {
            $scope.parentDomainId = "";
            $scope.downloadBtnTitle = "Download LOV File";
            $scope.hideLovTable = false;
            $scope.downloadFileUri = "/lovmanager/api/ListOfValue/GetFile?domainId=" + id;
            getDomain();
        };


        //Functions
        init();
        $scope.cancel = function () {
            rootShell.navigation.goTo('/page/app/lovmanager/domainList.html');
        };
        $scope.save = function () {
            var formData = new FormData();
            var file = $('#file1')[0];
            formData.append('file', file.files[0]);
            formData.append('domainName', $scope.domain.Name);
            formData.append('domainId', $scope.domain.Id);
            formData.append('domainCode', $scope.domain.Code);

            formData.append('parentDomainId', $scope.domain.ParentDomain.Id);
            var apiUri = "/lovmanager/api/domain/put";
            $.ajax({
                url: apiUri,
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (d) {
                    $('#updatePanelFile').addClass('alert-success').html('<strong>Success!</strong>').show();
                    $('#file1').val(null);
                },
                error: function (d) {
                    $('#updatePanelFile').addClass('alert-error').html('<strong>Failed!</strong>').show();
                }
            });
        };
        $scope.downloadLovFile = function () {

        };

    };

    editDomainController.resolve = {
        model: function ($rootScope) {
            var rootShell = $rootScope.rootShell;
            var deferred = rootShell.$q.defer();
            deferred.resolve({ Version: 1 });
            return deferred.promise;
        }

    };

    return editDomainController;
});
