define(['angular', 'utils', 'jicServices'], function (angular, utils, jicServices) {

    var productSetupController = function ($scope) {
        var rootShell = $scope.$root.rootShell;
        //fileUploaderApi($scope, api, moduleProvider, onSuccess, onFailure) 
        var getCurrentproductSetupFile = function () {
            rootShell.loader.show();
            var apiUri = "/api/ProductSetup/Get";
            rootShell.server.get(apiUri,
             {
                 success: function (response) {
                     rootShell.loader.hide();
                     $scope.currentProductSetupFile = response.Data;
                     if ($scope.currentProductSetupFile === null) {
                         $scope.hideDownloadButton = true;
                     }
                     else {
                         $scope.hideDownloadButton = false;
                         $scope.productSetupFileUrl = "/library/api/LibraryFile/GetFile?path=" + $scope.currentProductSetupFile.FilePath;
                         if ($scope.currentProductSetupFile.Version > 1) {
                             $scope.showRollbackBtn = true;
                         }
                     }
                 },
                 error: function (response) {
                     console.log(response);
                 }
             });
        };

        getCurrentproductSetupFile();
        $scope.onPackageSetupUploaderInit = function (api, params, data) {

            var rootShell = $scope.$root.rootShell;
            api.onUploadComplete(function (result, context, setContext) {
                if (result.Success === true) {
                    $scope.safeApply();
                    rootShell.loader.show();

                    var uri = '/api/ProductSetup/Validate';
                    //FileType = application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
                    $scope.productSetupFile = {
                        Id: "",
                        LibraryName: result.FileInfo.LibraryName,
                        FolderPath: result.FileInfo.FolderPath,
                        FilePath: result.FilePath,
                        UploadedFileName: result.FileInfo.Name,
                        OriginFileName: result.FileInfo.Title,
                        FileType: result.FileInfo.Type,
                        CreatedDate: "",
                        Version: ""
                    }
                    rootShell.server.post(uri, $scope.productSetupFile,
                        {
                            success: function (response) {
                                rootShell.loader.hide();
                                $scope.userHasUploadedDocument = true;
                                $scope.IsErrored = false;
                                $scope.IsRulesDefined = true;
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
                                    if (r.Value === null) {
                                        r.IsDefined = false;
                                    }
                                });
                                $scope.data.RuleDescriptions = response.Rules;
                                $scope.IsRulesDefined = true;
                                if ($scope.data !== null) {
                                    for (var i = 0; i < $scope.data.RuleDescriptions.length; i++) {
                                        if (!$scope.data.RuleDescriptionss[i].IsDefined) {
                                            $scope.IsRulesDefined = false;
                                        }
                                    }
                                }
                                if ($scope.packageSetupErrorMessages.length === 0) {
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


                rootShell.server.post(uri, $scope.productSetupFile,
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
                                   if (r.Value === null) {
                                       r.IsDefined = false;
                                   }
                               });
                               $scope.data.RuleDescriptions = response.Rules;
                               $scope.IsRulesDefined = true;
                               if ($scope.data !== null) {
                                   for (var i = 0; i < $scope.data.RuleDescriptions.length; i++) {
                                       if (!$scope.data.RuleDescriptionss[i].IsDefined) {
                                           $scope.IsRulesDefined = false;
                                       }
                                   }
                               }
                               if ($scope.packageSetupErrorMessages.length === 0) {
                                   $scope.IsErrored = false;
                               }
                               //end validation error section
                               callback();
                           }
                       });

            })
            callback();
        };
        $scope.cancel = function() {
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
                if ($scope.data.RuleDescriptions[index] && $scope.data.RuleDescriptions[index].Value && $scope.data.RuleDescriptions[index].Value.DisplayValue !== null) {
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

        $scope.rollback = function () {

            var pageUrl = '/page/productSetupRollback.html';
            var extraParams = {
            };
            var callback = function (response) {
                if (response.actionType == "submit") {
                    rootShell.loader.show();

                    var url = "/api/ProductSetup/Rollback";
                    var selectedversion = response.data;
                    var data =
                        {
                            "version": selectedversion
                        };

                    rootShell.server.post(url,
                               data,
                               {
                                   success: function (response) {
                                       rootShell.loader.hide();
                                       rootShell.modal.notify('rollback done successfully').then(function () {
                                           rootShell.navigation.goTo('/page/index.html');
                                       });
                                   },
                                   error: function (response) {
                                       rootShell.loader.hide();
                                       console.log(response);
                                   }
                               });
                }
            };
            rootShell.modal.openPage(pageUrl, extraParams, callback);
        };

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