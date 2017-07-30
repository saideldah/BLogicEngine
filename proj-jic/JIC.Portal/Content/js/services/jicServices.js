define('jicServices', ['utils'], function (utils) {

    // private Services for internal use
    var _requiredItemsPrintout = function ($scope, requiredItems) {
        var listOfRequiredDocuments = { Title: [], Name: [], ItemUrl: [] };
        var reqItems = null;
        var jointreqItems = null;
        if ($scope.wizard) {
            reqItems = $scope.wizard.description.Model.ClientRequiredItems;
            if ($scope.wizard.description.Model.JointInsured != null) {
                jointreqItems = $scope.wizard.description.Model.JointInsuredRequiredItems;
                angular.forEach(jointreqItems, function (requiredDoc) {
                    if (requiredDoc.RequiredOnPrinting && requiredDoc.VisibleIf) {
                        listOfRequiredDocuments.Title.push(requiredDoc.Title);
                        listOfRequiredDocuments.ItemUrl.push(requiredDoc.ItemURL);
                    }
                })
            }
            angular.forEach(reqItems, function (requiredDoc) {
                if (requiredDoc.RequiredOnPrinting && requiredDoc.VisibleIf) {
                    listOfRequiredDocuments.Title.push(requiredDoc.Title);
                    listOfRequiredDocuments.ItemUrl.push(requiredDoc.ItemURL);
                }
            })
        }
        else {
            reqItems = requiredItems;
            angular.forEach(reqItems, function (requiredDoc) {
                if (requiredDoc.RequiredOnPrinting && requiredDoc.VisibleIf == "True") {
                    listOfRequiredDocuments.Title.push(requiredDoc.Title);
                    listOfRequiredDocuments.ItemUrl.push(requiredDoc.ItemURL);
                }
            })
        }
        if (listOfRequiredDocuments.Title.length > 0) {
            $scope.$root.rootShell.modal.openPage('/page/app/quoteapp/RequiredDocumentModal.html', { details: listOfRequiredDocuments.Title }, function (result) {
                var listOfRequiredDocString = listOfRequiredDocuments.ItemUrl.join();
                window.open('/quoteapp/api/Package/RequiredDocumentPrintouts?info=' + listOfRequiredDocString);
            })
        }
    };
    var _showMessageAndPrint = function ($scope, currentPage, printingMessage, withCoverNotes, quoteId, withPolicy, requiredItems) {
        var deferred = $scope.$root.rootShell.$q.defer();

        while ($('#page-' + currentPage).length == 0 && currentPage <= 8) {
            currentPage++;
        }
        if (currentPage > 8) {
            if (withCoverNotes) {
                $scope.$root.rootShell.modal.notify($scope.$root.rootShell.localization.common.CoverNotePrint).then(function () {
                    setTimeout(function () {
                        window.open('/api/printout/get?id=' + quoteId + '__CoverNotePrintout__1036__false', '_blank');
                    }, 1000);
                    _requiredItemsPrintout($scope, requiredItems);
                });
            }
            deferred.resolve();
            return deferred.promise;
        }
        _switchClasses(true, currentPage);
        var rootShell = $scope.$root.rootShell;
        var msg = rootShell.localization.clientIdentification.PleaseInsert;
        msg += currentPage > 4 ? rootShell.localization.clientIdentification.secondary : rootShell.localization.clientIdentification.primary;
        msg += printingMessage[currentPage - 1 - (currentPage > 4 ? 4 : 0)] + rootShell.localization.clientIdentification.PressOk;
        $scope.$root.rootShell.modal.notify(msg).then(function () {
            window.print();
            _switchClasses(false, currentPage);
            setTimeout(function () {
                _showMessageAndPrint($scope, ++currentPage, printingMessage, withCoverNotes, quoteId, withPolicy, requiredItems).then(function () { deferred.resolve(); });
            }, 1000);
        });
        return deferred.promise;
    };
    var _switchClasses = function (activate, currentPage) {
        $('#page-' + currentPage).addClass(activate ? 'active' : 'inactive');
        $('#page-' + currentPage).removeClass(activate ? 'inactive' : 'active');
    };
    var _adaptPostData = function (request) {
        if (request.Model.SumInsured != null) {
            if (typeof request.Model.SumInsured == "object") {
                request.Model.SumInsured = request.Model.SumInsured[0];
            }
        }
        _fixRequiredItemCheckedProperty(request.Model.ClientRequiredItems);
        if (request.Model.JointInsured) {
            _fixRequiredItemCheckedProperty(request.Model.JointInsuredRequiredItems);
        }
        if (!request.Model.FillTheMedicalQuestionnaireOnSystem) {
            request.Model.Questionnaire = null;
        }
    };
    var _fixRequiredItemCheckedProperty = function (items) {
        angular.forEach(items, function (item) {
            if (angular.isArray(item.Checked)) {
                var val = item.Checked[0];
                item.Checked = val === true || val === 'true';
            }
        });
    };
    var _recognizePlaceholdersFormula = new RegExp('<@([a-z0-9_]+)>', 'ig');

    // end Private Services



    // start Public Services
    var _defineProperty = function (obj, key, get, set) {
        if (!obj.hasOwnProperty(key)) {
            Object.defineProperty(obj, key, {
                get: get,
                set: set,
                enumerable: true,
                configurable: true
            });
        }
    };
    var _roundByCurrency = function (currency, value) {
        if (currency == '90' || currency == '4') {
            if (value % 1 != 0) {
                value = parseInt(value) + 1;
            }
        } else if (currency == '1') {
            if (value % 500 != 0) {
                value = (parseInt(value / 500) + 1) * 500;
            }
        }
        return value;
    };
    var _roundActualValue = function ($scope, propertyPath) {
        $scope.$watch(propertyPath, function (newVal, oldVal) {
            if ($scope.model.Currency == '90' || $scope.model.Currency == '4') {
                if (newVal % 1 != 0) {
                    newVal = parseInt(newVal) + 1;
                }
            } else if ($scope.model.Currency == '1') {
                if (newVal % 500 != 0) {
                    newVal = (parseInt(newVal / 500) + 1) * 500;
                }
            }
            utils.assignObjectProperty($scope, propertyPath, newVal);
        });
    };
    var _getProductEvaluationRequest = function (api) {
        var request = {
            PackageId: api.description.PackageId,
            Model: angular.copy(api.description.Model),
            Policy: api.description.Policy
        };
        _adaptPostData(request);
        return request;
    };
    var _formatter = function (value) {
        if (isNaN(value)) {
            return value;
        }
        if (value == null) {
            return value;
        }
        var parts = value.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        var result = parts.join(".");

        return result;
    };
    var _formatOptionTitle = function ($scope, item, propertyName, replacePlaceholdersWithEmptyValues) {
        if (typeof replacePlaceholdersWithEmptyValues != 'boolean') {
            replacePlaceholdersWithEmptyValues = false;
        }
        var result = item[propertyName].replace(_recognizePlaceholdersFormula, function (match, p1) {
            var tentativeReplacement;
            if (replacePlaceholdersWithEmptyValues === true) {
                tentativeReplacement = '';
            } else if (p1 != 'Currency') {
                var affectedProperty = null;
                if (item.Properties && item.Properties.length) {
                    affectedProperty = utils.find(item.Properties, 'InternalName', p1.toLowerCase());
                }
                var affectedPropertyValue = null;
                if (affectedProperty) {
                    affectedPropertyValue = affectedProperty.Value;
                }
                tentativeReplacement = item[p1] || affectedPropertyValue || $scope.model[p1] || '';
                var tentativeFloat = window.parseFloat(tentativeReplacement);
                if (!window.isNaN(tentativeFloat)) {
                    tentativeReplacement = _formatter(tentativeFloat);
                }
            } else {
                tentativeReplacement = _getCurrencyTitle($scope);
            }

            return tentativeReplacement;
        });
        return result;
    };
    var _checkUserProfile = function (rootShell, profile) {
        var claims = rootShell.identity.getClaimValues('http://schemas.ams.com/organizationunit');
        return claims.indexOf(profile) > -1;
    };
    var _getCurrencyTitle = function (scope) {
        var result = utils.arrayFirst(scope.api.description.OptionSets.Currency, function (item) {
            return item.Name == scope.api.description.Model.Currency;
        });
        return result ? result.Title : null;
    };
    var _getPaymentFrequencyTitle = function (scope) {
        var result = utils.arrayFirst(scope.api.description.OptionSets.PaymentFrequency, function (item) {
            return item.Name == scope.api.description.Model.PaymentFrequency;
        });
        return result ? result.Title : null;
    };
    var _checkDiscrepancy = function ($scope, client, $sce) {

        var rootShell = $scope.$root.rootShell;
        var deferred = rootShell.$q.defer();
        rootShell.loader.isVisible = true;
        rootShell.server.post('/quoteapp/api/client/SearchFOGetDifference', client, {
            success: function (response) {
                rootShell.loader.isVisible = false;
                //var isAgencyDirector = _checkUserProfile(rootShell, 'agencydirector');
                var propName = null;
                if (response && response.length) {
                    var textToDisplay = "";

                    //if (isAgencyDirector) {
                    //    textToDisplay = "<h3>" + rootShell.localization.clientIdentification.ClientChangedWithUpdate + "</h3>";
                    // } else {
                    textToDisplay = "<h3>" + rootShell.localization.clientIdentification.ClientChanged + "</h3>";
                    // }   
                    textToDisplay += "<table class=\"table table-striped\"><thead><tr><th></th><th>" + rootShell.localization.clientIdentification.Old + "</th><th>" + rootShell.localization.clientIdentification.New + "</th></tr></thead><tbody> "
                    for (var i = 0; i < response.length; i++) {
                        propName = rootShell.localization.clientIdentification[response[i].Property];
                        if (!propName) {
                            propName = (response[i].FProperty) || (response[i].Property);
                        }
                        textToDisplay += "<tr>";
                        textToDisplay += "<td>" + propName + " </td>";
                        textToDisplay += "<td>" + response[i].FormatedOldValue + " </td>";
                        textToDisplay += "<td>" + response[i].FormatedNewValue + " </td></tr>";
                    }
                    textToDisplay += "</tbody></table>";
                    var notifyOptions = {
                        actionButtonText: 'OK',
                        action: 'info',
                        htmlBind: true,
                        bodyText: $sce.trustAsHtml(textToDisplay),
                        glyphicon: 'glyphicon-info-sign'
                    };
                    rootShell.modal.custom({}, notifyOptions).then(
                        function () {
                            deferred.resolve();
                        });
                    //  }
                }
                else {
                    deferred.resolve();
                }
            },
            error: function (result) {
                rootShell.loader.isVisible = false;
                rootShell.modal.error(result.Message).then(function () {
                    deferred.reject();
                });
            }
        });
        return deferred.promise;
    };
    var _checkDuplicates = function ($scope, client) {
        $scope.$root.rootShell.loader.isVisible = true;
        return $scope.$root.rootShell.server.post('/quoteapp/api/client/CheckDuplicates', client)
            .then(function (r1) {
                $scope.$root.rootShell.loader.isVisible = false;
                return r1;
            }, function (r1) {
                $scope.$root.rootShell.loader.isVisible = false;
                return r1;
            });
    };
    var _getFormattedDate = function (date) {
        var dateObject = new Date(date);
        var dd = dateObject.getDate();
        var mm = dateObject.getMonth() + 1;

        var yyyy = dateObject.getFullYear();
        if (dd < 10) {
            dd = '0' + dd
        }
        if (mm < 10) {
            mm = '0' + mm
        }
        return dd + '/' + mm + '/' + yyyy;
    };
    var _setupFormattedDisplayTitle = function ($scope, item, replacePlaceholdersWithEmptyValues) {
        _defineProperty(item, 'FormattedDisplayText', function () {
            return _formatOptionTitle($scope, item, 'Title', replacePlaceholdersWithEmptyValues);
        });
    };
    var _getUrlParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    };
    var _checkPdfGenerated = function ($scope, policyNumber, milliseconds, template, boCode) {
        $scope.$root.rootShell.server.get('/quoteapp/api/PolicyList/CheckPdfGenerated?policyNumber=' + policyNumber).then(function (response) {
            $scope.$root.rootShell.loader.show();
            if (response == false && milliseconds >= 120000) {
                $scope.$root.rootShell.loader.hide();
                $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.common.ContactSGKL);
            }
            if (response == true) {
                _printPolicy(policyNumber, template, boCode);
                $scope.$root.rootShell.loader.hide();
            } else if (milliseconds < 120000) {
                setTimeout(function () {
                    _checkPdfGenerated($scope, policyNumber, milliseconds + 5000, template, boCode);
                }, 5000);
            }
        }, function (error) {
            $scope.$root.rootShell.loader.hide();
            $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.common.ContactSGKL);
        })
    };
    var _printPolicy = function (number, template, boCode) {
        window.open('/quoteapp/api/PolicyList/PrintPolicy?policyNumber=' + number + '&templateName=' + template + '&boCode=' + boCode + '&type=automaticpolicyissued', '_blank');
    };
    var _printHelper = function ($scope, angularService, printUrl, withCoverNotes, requiredItems, withPolicy, printoutDate) {
        var deferred = $scope.$root.rootShell.$q.defer();
        var quoteId = printUrl.split("id=")[1].split("__JicProposalPrintout__")[0];
        $scope.$root.rootShell.loader.isVisible = true;
        var listOfRequiredDocuments = { Title: [], Name: [], ItemUrl: [] };
        var reqItems = null;
        var jointreqItems = null;
        $scope.$root.rootShell.server.get(printUrl)
                .then(function (result) {
                    var rootShell = $scope.$root.rootShell;
                    $scope.$root.rootShell.loader.isVisible = false;
                    var body = angularService.element(document.querySelector('body'));
                    result = result.split("\n").slice(1).join("\n")
                    var angularElement = angularService.element(result);
                    body.append(angularElement)
                    var printingMessage = [rootShell.localization.clientIdentification.proposalpaper, rootShell.localization.clientIdentification.questionnaire1, rootShell.localization.clientIdentification.questionnaire2, rootShell.localization.clientIdentification.declarationgoodhealt];

                    _showMessageAndPrint($scope, 1, printingMessage, withCoverNotes, quoteId, withPolicy, requiredItems).then(function () {
                        angularElement.remove();

                    }).then(function () {
                        if (!withCoverNotes) {
                            _requiredItemsPrintout($scope, requiredItems);
                        }
                    });

                    deferred.resolve();
                }, function () {
                    $scope.$root.rootShell.loader.isVisible = false;

                });

        return deferred.promise;
    };
    var _filterUsingListManager = function (lm, filter, callback, rootShell) {
        return lm.filter(filter, callback)
            .then(function (r) {
                return r;
            }, function (rd) {
                if (rd && rd.Message) {
                    rootShell.modal.error(rd.Message);
                }
                return rootShell.$q.reject(rd);
            });
    };
    var _fileUploaderApi = function ($scope, api, moduleProvider, onSuccess, onFailure) {
        var rootShell = $scope.$root.rootShell;
        api.onUploadComplete(function (result, context, setContext) {
            if (result.Success == true) {
                var module = moduleProvider;
                if (typeof moduleProvider == 'function') {
                    module = moduleProvider();
                }
                $scope.safeApply();
                rootShell.loader.show();
                rootShell.server.post('/api/SetupFile/UploadDocument', {
                    DocumentUrl: result.FilePath,
                    ModuleName: module
                }, {
                    success: function (response) {
                        rootShell.loader.hide();
                        if (typeof onSuccess == 'function') {
                            onSuccess(response);
                        }
                    },
                    error: function (response) {
                        rootShell.loader.hide();
                        if (typeof onFailure == 'function') {
                            onFailure(response);
                        }
                        rootShell.notification.show('An error has occurred. Kindly check the system log for more information, then try again.');
                    }
                });
            }
            else {
                rootShell.loader.hide();
                rootShell.modal.error(result.Error);
            }
        });

    };
    var _showCustomMessageAfterValidation = function ($scope, withCoverNotes, withPolicy, proposalNumber) {
        var message = $scope.resources.QuoteProposal + proposalNumber + $scope.resources.ValidatedSuccessfully;
        if (withCoverNotes) {
            message += $scope.resources.CoverPrint;
        }
        if (withPolicy) {
            message += $scope.resources.PolicyPrint;
        }

        $scope.$root.rootShell.modal.notify(message, true);
    };
    var _getPoliciesWithNoSignature = function ($scope, clientCode, jointClientCode, policyNumber, showMessage) {
        var deferred = $scope.$root.rootShell.$q.defer();
        var url;
        if (clientCode == null && jointClientCode == null) {
            url = '/quoteapp/api/PolicyList/GetWithNoSignatureForEndorsement';
            url = utils.addUrlParameter(url, 'policyNumber', arguments[3]);
        } else {
            url = '/quoteapp/api/PolicyList/GetWithNoSignature';
            url = utils.addUrlParameter(url, 'clientId', arguments[1]);
            url = utils.addUrlParameter(url, 'jointClientId', arguments[2]);
        }

        $scope.$root.rootShell.server.get(url)
                .then(function (result) {
                    if (!showMessage) {
                        deferred.resolve(result);
                    }
                    else {
                        if (result != null && result.length > 0) {
                            var message = '<div><strong>The signature is not yet received yet for: </strong></div>';
                            message += '<div><table class="table table-condensed"><thead><tr><th>Proposal number</th><th>Product code</th><th>Issue date</th></tr></thead>';

                            angular.forEach(result, function (item) {
                                message += '<tr><td>' + item.PolicyNumber + '</td><td>' + item.ProductCode + '</td><td>' + item.FormattedIssueDate + '</td></tr>';
                            });
                            message += '</table></div>';
                            $scope.$root.rootShell.modal.notify(message, true).then(function (r) {
                                deferred.resolve(result);
                            });
                        }
                        else {
                            deferred.resolve();
                        }

                    }

                });
        return deferred.promise;
    };
    var _getEndorsementListAction = function ($scope) {
        return {
            view: function (id, urlPath) {
                $scope.$root.rootShell.server.get('/quoteapp/api/EndorsementList/AllowAction?entityId=' + id + '&entityName=EndorsementRequest&action=view')
                .then(function (result) {
                    if (result.canAccess) {
                        //var url = a.Data.UrlPath;
                        url = utils.addUrlParameter(urlPath, 'viewMode', true);
                        $scope.$root.rootShell.navigation.goTo('/page/app/quoteapp/endorsementView.html?id=' + id);
                    }
                    else {
                        $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.common.AccessDenied);
                    }
                }, function (result) { $scope.$root.rootShell.modal.error(result.Message); });
            },
            modify: function (id, urlPath) {
                $scope.$root.rootShell.server.get('/quoteapp/api/EndorsementList/AllowAction?entityId=' + id + '&entityName=EndorsementRequest&action=modify')
                .then(function (result) {
                    if (result.canAccess) {
                        //var url = a.Data.UrlPath;
                        $scope.$root.rootShell.navigation.goTo(urlPath);
                    }
                    else {
                        $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.common.AccessDenied);
                    }
                }, function (result) { $scope.$root.rootShell.modal.error(result.Message); });
            },
            print: function (id) {

                $scope.$root.rootShell.server.get('/quoteapp/api/EndorsementList/AllowAction?entityId=' + id + '&entityName=EndorsementRequest&action=print')
                    .then(function (result) {
                        if (result.canAccess) {
                            window.open('/quoteapp/api/EndorsementPrintout/EndorsementRequest?printoutId=' + id);
                        }
                        else {
                            $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.common.AccessDenied);
                        }
                    }, function (result) { $scope.$root.rootShell.modal.error(result.Message); });
            },
            validate: function (id, policyNumber, callback) {
                var deferred = $scope.$root.rootShell.$q.defer();
                _getPoliciesWithNoSignature($scope, null, null, policyNumber, true).then(function () {
                    $scope.$root.rootShell.modal.confirm($scope.resources.Validation).then(function (result) {

                        $scope.$root.rootShell.server.get('/quoteapp/api/EndorsementList/Validate?entityId=' + id)
                        .then(function (result) {
                            deferred.resolve(result);
                            if (typeof callback === "function") {
                                callback(result);
                            }
                        }, function (result) {
                            $scope.$root.rootShell.modal.error(result.Message);
                            deferred.reject(result);
                        });

                    }, function () { deferred.reject(); });
                });

                return deferred.promise;
            },
            invalidate: function (id, callback) {
                var deferred = $scope.$root.rootShell.$q.defer();
                $scope.$root.rootShell.modal.confirm($scope.resources.InValidation).then(function (result) {

                    $scope.$root.rootShell.server.get('/quoteapp/api/EndorsementList/Invalidate?entityId=' + id)
                    .then(function (result) {
                        deferred.resolve(result);
                        if (typeof callback === "function") {
                            callback(result);
                        }
                    }, function (result) {
                        $scope.$root.rootShell.modal.error(result.Message);
                        deferred.reject(result);
                    });

                });
                return deferred.promise;
            }
        };
    };
    // end Public Services

    var jicServices = {
        formatter: _formatter,
        formatOptionTitle: _formatOptionTitle,
        roundByCurrency: _roundByCurrency,
        roundActualValue: _roundActualValue,
        getProductEvaluationRequest: _getProductEvaluationRequest,
        getCurrencyTitle: _getCurrencyTitle,
        defineProperty: _defineProperty,
        checkUserProfile: _checkUserProfile,
        checkDiscrepancy: _checkDiscrepancy,
        checkDuplicates: _checkDuplicates,
        getFormattedDate: _getFormattedDate,
        getPaymentFrequencyTitle: _getPaymentFrequencyTitle,
        setupFormattedDisplayTitle: _setupFormattedDisplayTitle,
        getUrlParameterByName: _getUrlParameterByName,
        printHelper: _printHelper,
        filterUsingListManager: _filterUsingListManager,
        getEndorsementListAction: _getEndorsementListAction,
        fileUploaderApi: _fileUploaderApi,
        showCustomMessageAfterValidation: _showCustomMessageAfterValidation,
        getPoliciesWithNoSignature: _getPoliciesWithNoSignature,
        printPolicy: _printPolicy,
        checkPdfGenerated: _checkPdfGenerated
    };
    return jicServices;
})