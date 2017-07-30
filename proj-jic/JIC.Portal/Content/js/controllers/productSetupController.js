define(['angular', 'utils', 'jicServices'], function (angular, utils, jicServices) {

    var productSetupController = function ($scope) {
        var rootShell = $scope.$root.rootShell;
        //fileUploaderApi($scope, api, moduleProvider, onSuccess, onFailure) 
        
        $scope.onPackageSetupUploaderInit = function (api, params, data) {

            var rootShell = $scope.$root.rootShell;
            api.onUploadComplete(function (result, context, setContext) {
                if (result.Success == true) {
                    $scope.safeApply();
                    rootShell.loader.show();

                    var uri = '/api/ProductSetup/Validate';
                    //FileType = application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
                    $scope.fileData = {
                        Id : "",
                        LibraryName: result.FileInfo.LibraryName,
                        FolderPath: result.FileInfo.FolderPath,
                        FilePath: result.FilePath,
                        UploadedFileName: result.FileInfo.Name,
                        OriginFileName: result.FileInfo.Title,
                        FileType: result.FileInfo.Type,
                        Version : ""
                    }
                    rootShell.server.post(uri, $scope.fileData,
                        {
                            success: function (response) {
                                rootShell.loader.hide();
                                if (typeof onSuccess == 'function') {
                                    onSuccess(response);
                                }
                            },
                            error: function (response) {
                                rootShell.loader.hide();
                                rootShell.notification.show('An error has occurred. Kindly check the system log for more information, then try again.');
                                //validation error section
                                $scope.userHasUploadedDocument = true;
                                $scope.IsErrored = true;
                                $scope.data.PackageSetupUrl = response.DocumentUrl;
                                $scope.packageSetupErrorMessages = response.ErrorMessages;
                                angular.forEach(response.Rules, function (r) {
                                    if (r.Value == null) {
                                        r.IsDefined = false;
                                    }
                                });
                                $scope.data.RuleDescriptions = response.Rules;
                                $scope.IsRulesDefined = true;
                                if ($scope.data != null) {
                                    for (var i = 0; i < $scope.data.RuleDescriptions.length; i++) {
                                        if (!$scope.data.RuleDescriptionss[i].IsDefined) {
                                            $scope.IsRulesDefined = false;
                                        }
                                    }
                                }
                                if ($scope.packageSetupErrorMessages.length == 0) {
                                    $scope.IsErrored = false;
                                }
                                //end validation error section
                            }
                        });
                }
                else {
                    rootShell.loader.hide();
                    rootShell.modal.error(result.Error);
                }
            });
        };
        $scope.save = function (callback) {
            rootShell.modal.confirm("Are you sure you want to save changes ?").then(function () {
                rootShell.loader.show();
                var uri = '/api/ProductSetup/Post';
                //FileType = application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
               

                rootShell.server.post(uri, $scope.fileData,
                       {
                           success: function (response) {
                               rootShell.loader.hide();
                               rootShell.modal.notify('Successfully updated').then(function () {
                                   rootShell.navigation.goTo('/page/index.html');
                               });
                               callback();
                           },
                           error: function (response) {
                               rootShell.loader.hide();
                               rootShell.notification.show('An error has occurred. Kindly check the system log for more information, then try again.');
                               //validation error section
                               $scope.userHasUploadedDocument = true;
                               $scope.IsErrored = true;
                               $scope.data.PackageSetupUrl = response.DocumentUrl;
                               $scope.packageSetupErrorMessages = response.ErrorMessages;
                               angular.forEach(response.Rules, function (r) {
                                   if (r.Value == null) {
                                       r.IsDefined = false;
                                   }
                               });
                               $scope.data.RuleDescriptions = response.Rules;
                               $scope.IsRulesDefined = true;
                               if ($scope.data != null) {
                                   for (var i = 0; i < $scope.data.RuleDescriptions.length; i++) {
                                       if (!$scope.data.RuleDescriptionss[i].IsDefined) {
                                           $scope.IsRulesDefined = false;
                                       }
                                   }
                               }
                               if ($scope.packageSetupErrorMessages.length == 0) {
                                   $scope.IsErrored = false;
                               }
                               //end validation error section
                               callback();
                           }
                       });
         
            })
            callback();
        };
        $scope.goBack = function goBack(callback) {
            rootShell.modal.warning("Are you sure you want to cancel? Any changes you might have done will be ignored").then(function () {
                rootShell.navigation.goTo('/page/index.html');
            }, function () {
                callback();
            });
        };
        $scope.openRuleEditor = function (index, module) {
            var ruleCopy = angular.copy($scope.data.RuleDescriptions[index]);
            rootShell.modal.openPage('/page/app/quoteapp/ruleEditor.html', { details: ruleCopy, module: module }, function (updatedRule) {
                $scope.data.RuleDescriptions[index] = updatedRule;
                if ($scope.data.RuleDescriptions[index] && $scope.data.RuleDescriptions[index].Value && $scope.data.RuleDescriptions[index].Value.DisplayValue != null) {
                    $scope.data.RuleDescriptions[index].IsDefined = true;
                }
                else {
                    $scope.data.RuleDescriptions[index].IsDefined = false;
                }
                $scope.IsRulesDefined = true;
                for (var i = 0; i < $scope.data.RuleDescriptions.length; i++) {
                    if (!$scope.data.RuleDescriptions[i].IsDefined) {
                        $scope.IsRulesDefined = false;
                    }
                }
            });
        };

        $scope.rollback = function (callback) {
            rootShell.modal.warning("Are you sure you want to rollback to the previous version?").then(function () {
                rootShell.loader.show();
                rootShell.server.post('/api/SetupFile/Roleback', $scope.data
                ).then(function (result) {
                    rootShell.loader.hide();
                    rootShell.modal.notify('Successfully updated').then(function () {
                        rootShell.navigation.goTo('/page/index.html');
                    });
                    callback();
                });
                callback();
            }, function () {
                //Do nothing if no
                callback();
            });
        }
        $scope.data = {
            AccessDefinitionUrl: null,
            PackageSetupUrl: null,
            ProductMappingUrl: null,
            ModuleName: 'product',
            DocumentUrl: "",
            RuleDescriptions: []
        }

        
        $scope.isValid = function () {
            return $scope.DocumentUrl;
        };
    };

    return productSetupController;
});