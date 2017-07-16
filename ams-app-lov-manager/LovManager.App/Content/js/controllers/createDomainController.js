

define(['angular', 'utils'], function (angular, utils) {

    var createDomainController = function ($scope, model, $http) {
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


        //Functions
        init();
        $scope.cancel = function () {
            rootShell.navigation.goTo('/page/app/lovmanager/domainList.html');
        };
        $scope.save = function () {
            var formData = new FormData();
            var file = $('#file1')[0];
            formData.append('file', file.files[0]);
            formData.append('name', $scope.lovDomain.Name);
            formData.append('parentDomainId', $scope.parentDomainId);
            var apiUri = "/lovmanager/api/domain/post";
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

    createDomainController.resolve = {
        model: function ($rootScope) {
            var rootShell = $rootScope.rootShell;
            var deferred = rootShell.$q.defer();
        

            deferred.resolve({ Version: 1 });

            return deferred.promise;
        }

    };

    return createDomainController;
});
