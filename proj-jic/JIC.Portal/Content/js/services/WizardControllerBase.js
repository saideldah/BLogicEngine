define('wizardControllerBase', ['angular', 'actionInjectorService', 'jicServices', 'JicWizardHelper', 'utils'], function (angular, ActionInjectorService, jicServices, JicWizardHelper, utils) {
    function WizardControllerBase(wizardControllerName, $scope, server, $filter, $injector, $rootScope, $location, insuredClientService, $sce) {

        var
            savedQuoteRequest = null,
            shouldGoToLastStep = false,
            withCoverNotes = false,
            withPolicy = false,
            sequenceNumber;
        exitWizard = function (returnUrl) {
            $scope.wizard.close();
            $scope.$root.rootShell.sharedContext.remove('client');
            if (returnUrl) {
                $scope.$root.rootShell.navigation.goTo(returnUrl);
            } else {
                $scope.$root.rootShell.navigation.goBack();
            }
        },
        rootShell = $scope.$root.rootShell,
        resources = {},

        getProductEvaluationRequest = function () {
            return jicServices.getProductEvaluationRequest($scope.wizard);
        },
        saveSmi = function (extraParameters, continueEditing) {
            var deferred = $scope.$root.rootShell.$q.defer();

            var quoteRequest = savedQuoteRequest;

            var actionService = new ActionInjectorService("Quote", $scope, $injector);
            if (!quoteRequest) {

                //These will be removed when we hide steps ( so we save them to the validate action)
                quoteRequest = getProductEvaluationRequest();
                //if single
                if (quoteRequest.Model.PaymentFrequency == 4) {
                    quoteRequest.Model.PaymentDuration = 0;
                }
                quoteRequest.ProductId = $scope.wizard.description.Model.SelectedProductId;
                savedQuoteRequest = quoteRequest;
            }

            quoteRequest.IsUpdate = $scope.wizard.description.Model.QuoteId ? true : false;
            quoteRequest.QuoteId = $scope.wizard.description.Model.QuoteId;
            quoteRequest.ExtraParameters = extraParameters;
            var extraClientSearchData = rootShell.sharedContext.get('ClientSearchExtraData');
            if (extraClientSearchData != null) {
                quoteRequest.Model.T24ClientReference = quoteRequest.Model.JointInsured ? extraClientSearchData.ClientId : quoteRequest.Model.Client.ClientId;
            }
            $scope.$root.rootShell.loader.isVisible = true;
            //console.log(quoteRequest.Model);
            var result = actionService.execute("Quote", quoteRequest); // A promise is returned.
            result.then(
                function (resp) {
                    $scope.$root.rootShell.loader.isVisible = false;
                    $scope.actionState.viewMode = !continueEditing ? true : false;
                    $scope.wizard.description.QuoteStatus = resp.StatusName; //returns the current state of the quote
                    quoteRequest.Model.QuoteId = resp.Id;
                    $scope.wizard.description.Model.QuoteId = resp.Id;
                    deferred.resolve(resp);
                },
                function (err) { $scope.$root.rootShell.loader.isVisible = false; deferred.reject(err); });
            return deferred.promise;
        },
        //convertCheckedRequiredItemsToArray = function (items) {
        //    angular.forEach(items, function (item) {
        //        if (item.Checked === true || item.Checked === 'true') {
        //            item.Checked = ['true'];
        //        }
        //    });
        //},
        updateQuestionnaireVisibility = function () {
            if ($scope.wizard && $scope.wizard.initialized) {
                var request = getProductEvaluationRequest();
                if (request.Model.SumInsured == null) {
                    request.Model.SumInsured = 0;
                }
                server.post('/quoteapp/api/Package/EvaluateQuestionnaire', request)
                    .then(function (r) {
                        $rootScope.questionnaireVisibility = r;
                        $scope.wizard.description.questionnaireVisibility = r;
                        $scope.wizard.description.Model.Client.Summary = r.PrimarySummary;
                        if ($scope.wizard.description.Model.JointInsured) {
                            $scope.wizard.description.Model.JointInsured.Summary = r.SecondarySummary;
                        }
                    }, function () {
                        $scope.$root.rootShell.notification.show('Error while evaluating the questionnaire conditions', 'error');
                    });
            }
        };
        angular.extend(resources, rootShell.localization.common);
        $scope.config = {
            controllerUrl: '/quoteapp/api/' + wizardControllerName,
            locationService: {
                getCurrentLocation: function () {
                    return {
                        Id: 'C01C0557-2557-4920-BE28-41DA80A7F0BD',
                        Code: 'LB',
                        Title: 'Lebanon'
                    };
                }
            },
            server: server,
            filterService: $filter
        };

        $scope.packageId = $scope.$root.rootShell.route.params().packageId;

        $scope.cancel = function () {
            if ($scope.actionState.canSimulate && $scope.wizard.currentStep.Name == "productSelection" && $scope.wizard.description.Model.SelectedProductId) {
                $scope.$root.rootShell.modal.confirm("Do you want to save this simulation?").then(function (result) {
                    $scope.simulate();
                },
                function () {
                    exitWizard();
                });
            }
            else {
                exitWizard();
            }
        };

        //$scope.goHome = function () {
        //    exitWizard('/page/index.html');
        //};

        $scope.simulate = function () {
            saveSmi().then(function (result) {
                $scope.$root.rootShell.modal.notify(resources.QuoteSimulation + result.AdditionalData.SimulationNumber + resources.SavedSuccessfully)
                    .then(function (result) {
                        exitWizard('/page/app/quoteapp/simulationList.html');
                    });
            }, function (error) {
                $scope.$root.rootShell.notification.show("An error has occured", 'error');
            });
        };

        $scope.saveProposal = function () {
            var params = { IsProposal: true, updateTo: "pending", newProposalNumber: true };
            saveSmi(params).then(function (result) {

                $scope.wizard.description.Model.QuoteId = result.Id;
                $scope.$root.rootShell.modal.notify(resources.QuoteProposal + result.AdditionalData.ProposalNumber + resources.SavedSuccessfully);
            }, function (error) {
                $scope.$root.rootShell.notification.show("An error has occured", 'error');
            });
        };

        $scope.validate = function () {
            var jointMatricule = $scope.wizard.description.Model.Client.JointInsured ? $scope.wizard.description.Model.Client.Matricule : null
            jicServices.getPoliciesWithNoSignature($scope, $scope.wizard.description.Model.Client.Matricule, jointMatricule, null, true).then(function (data) {
                var params = { IsProposal: true, updateTo: "validated", newProposalNumber: $scope.wizard.description.QuoteStatus == "pending" ? false : true };
                saveSmi(params).then(function (result) {
                    withCoverNotes = result.AdditionalData.WithCoverNotes;
                    proposalNumber = result.AdditionalData.proposalNumber;
                    withPolicy = result.AdditionalData.WithPolicy;
                    var indexOfSlash = result.AdditionalData.ProposalNumber.lastIndexOf('/');
                    sequenceNumber = parseInt(result.AdditionalData.ProposalNumber.substring(indexOfSlash + 1, result.AdditionalData.ProposalNumber.length));
                    $scope.wizard.description.Model.QuoteId = result.Id;
                    jicServices.showCustomMessageAfterValidation($scope, result.AdditionalData.WithCoverNotes, result.AdditionalData.WithPolicy, result.AdditionalData.ProposalNumber);

                }, function (error) {
                    $scope.$root.rootShell.notification.show("An error has occured", 'error');
                });

            });
        };

        $scope.print = function () {
            var pageUrl = '/page/app/quoteapp/PrintingLanguageSelection.html';
            var extraParams = {
            };
            var callback = function (LCID) {
                if (LCID != "Cancel") {
                    saveSmi(null, true).then(function (result) {
                        //exitWizard('/page/app/quoteapp/simulationList.html');
                        window.open('/api/printout/get?id=' + result.Id + '__JicPrintout__' + LCID + '', '_blank');
                    }, function (error) {
                        $scope.$root.rootShell.notification.show("An error has occured", 'error');
                    });
                }
            };
            $scope.$root.rootShell.modal.openPage(pageUrl, extraParams, callback);
        };


        $scope.printProposal = function () {
            jicServices.printHelper($scope, angular, '/api/printout/getHTML?id=' + $scope.wizard.description.Model.QuoteId + '__JicProposalPrintout__', withCoverNotes, null, withPolicy, null).then(function () {
                if (withPolicy) {
                    $scope.$root.rootShell.server.get('/quoteapp/api/PolicyList/CheckPolicyPrintout?boCode=' + $scope.wizard.description.SelectedProduct.Premium.CustomData.CombinedPremium.BackOfficeProductCode + '&isAutomaticPolicy=' + true + '&sumInsured=' + $scope.wizard.description.Model.SumInsured + '&printoutDate=' + null)
                        .then(function (response) {
                            if (response && response.Template) {
                                $scope.$root.rootShell.modal.notify($scope.$root.rootShell.localization.common.PolicyPrintPDF).then(function () {
                                    jicServices.checkPdfGenerated($scope, sequenceNumber, 5000, response.Template, "");
                                })
                            }
                        }, function (error) {
                            $scope.$root.rootShell.notification.show($scope.$root.rootShell.localization.common.ContactSGKL, 'error');
                        });
                };
                if (withCoverNotes) {
                    $scope.$root.rootShell.server.get('/quoteapp/api/ProposalList/CoverNotePrintoutDate?proposalNumber=' + sequenceNumber);
                }
            });
        };

        $scope.$on('WizardDestroyed', function () {
            $scope.wizard.description.Model.JointInsured = null;
        });

        $scope.$root.remoteAgenceRequestDataMapper = function (query, page) {
            var agent = '';
            if ($scope.wizard && $scope.wizard.description && $scope.wizard.description.Model) {
                agent = $scope.wizard.description.Model.Agent;
            }
            return {
                query: query,
                page: page,
                preLoader: agent,
            };
        };
        $scope.$root.remoteApporteurRequestDataMapper = function (query, page) {
            var apporteur = '';
            if ($scope.wizard && $scope.wizard.description && $scope.wizard.description.Model) {
                apporteur = $scope.wizard.description.Model.Apporteur;
            }
            return {
                query: query,
                page: page,
                preLoader: apporteur,
            };
        };

        $scope.$on('WizardInitialized', function () {

            $scope.actionState = {};
            var isViewMode = $scope.$root.rootShell.route.params().viewMode == 'true';
            var canSimulate = $scope.$root.rootShell.route.params().canSimulate == 'true';
            $scope.wizard.description.Model.canSimulate = $scope.$root.rootShell.route.params().canSimulate;
            //edit mode
            if (!isViewMode) {
                $scope.wizard.description.QuoteStatus = "new";
                savedQuoteRequest = null;
            }

            jicServices.defineProperty($scope.actionState, 'validateProposal', function () {
                return !isViewMode && $scope.wizard.description.QuoteStatus != 'validated' && !(($scope.wizard.description.Model && $scope.wizard.description.Model.DelayReason) || (savedQuoteRequest && savedQuoteRequest.Model.DelayReason));
            });

            jicServices.defineProperty($scope.actionState, 'printProposal', function () {
                var printWithoutValidation = $scope.wizard.description.PackageParameters["Print without Validation"];

                if ($scope.wizard.description.QuoteStatus)
                    return (!printWithoutValidation[0].Value && $scope.wizard.description.QuoteStatus == "validated")
                        || (printWithoutValidation[0].Value && ($scope.wizard.description.QuoteStatus == "pending" || $scope.wizard.description.QuoteStatus == "validated"));
                return false;
            });

            jicServices.defineProperty($scope.actionState, 'saveProposal', function () {
                if ($scope.wizard.description.QuoteStatus)
                    return !isViewMode && $scope.wizard.description.QuoteStatus != "validated" && $scope.wizard.description.QuoteStatus != "pending";
                return false;
            });

            jicServices.defineProperty($scope.actionState, 'saveSimulation', function () {

                return canSimulate && !$scope.wizard.description.IsProposal;
            });

            jicServices.defineProperty($scope.actionState, 'changeSimulation', function () {

                return !$scope.wizard.description.Model.QuoteId;
            });

            if (!$scope.wizard.description.Model.AvailableInsuredClients) {
                var clients = $scope.$root.rootShell.sharedContext.get('AvailableInsuredClients');
                if (clients) {
                    $scope.wizard.description.Model.AvailableInsuredClients = clients.slice();
                }
            }

            var firstTimeInitInEdit = $scope.wizard.description.Model.QuoteId ? true : false;

            var destroyWizardInitWatch = $scope.$watch('wizard.initialized', function (newValue, oldValue) {
                if (newValue != oldValue && newValue) {
                    if (shouldGoToLastStep) {
                        $scope.wizard.showLast();
                        shouldGoToLastStep = false;
                        destroyWizardInitWatch();
                    }
                }


            });

            $scope.$watch('wizard.description.Model.JointInsured', function (newItem, oldItem) {
                if (newItem != oldItem) {
                    //if joint insured has only one property (summary) which is null just empty the joint insured
                    if ($scope.wizard.description.Model.JointInsured && !$scope.wizard.description.Model.JointInsured.Summary && Object.keys($scope.wizard.description.Model.JointInsured).length == 1) {
                        $scope.wizard.description.Model.JointInsured = null;
                    }
                    else {
                        //first time flag is used to skip emptying the selected account on opening simulation/proposal
                        if (!firstTimeInitInEdit) {
                            $scope.wizard.description.Model.SelectedAccount = null;
                        }
                        firstTimeInitInEdit = false;
                    }
                    if ($scope.wizard.description.Model.JointInsured) {

                    }
                    $scope.$broadcast('jointInsuredUpdated');
                    updateQuestionnaireVisibility();

                }

            });
            //if ($scope.wizard.description.Model.ClientRequiredItems) {
            //    convertCheckedRequiredItemsToArray($scope.wizard.description.Model.ClientRequiredItems);
            //    if ($scope.wizard.description.Model.JointInsured && $scope.wizard.description.Model.JointInsuredRequiredItems) {
            //        convertCheckedRequiredItemsToArray($scope.wizard.description.Model.JointInsuredRequiredItems);
            //    }
            //}

            if ($scope.wizard.description.Model.Client) {
                $scope.$root.rootShell.sharedContext.set('client', $scope.wizard.description.Model.Client);
            } else {
                $scope.wizard.description.Model.Client = $scope.$root.rootShell.sharedContext.get('client');
            }

            if ($scope.$root.rootShell.route.params().viewMode == 'true') {
                $scope.$root.rootShell.modal.notify('Please be aware that you are opening this proposal in view mode. Any changes will not be taken into account when printing');
            }
            if ($scope.wizard.description.Model.ProposalStatusText == 'OutdatedPackage') {
                $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.errorMessages.OutdatedPackage)
                    .then(exitWizard);
            }
            else if ($scope.wizard.description.Model.ProposalStatusText == 'MappingFailedSimulation') {
                $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.errorMessages.MappingFailedSimulation)
                    .then(exitWizard);
            }
            else if ($scope.wizard.description.Model.ProposalStatusText == 'MappingFailedProposal') {
                $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.errorMessages.MappingFailedProposal)
                    .then(exitWizard);
            }
            else if ($scope.wizard.description.Model.ProposalStatusText == 'PackageVersionChanged') {
                $scope.$root.rootShell.modal.notify($scope.$root.rootShell.localization.errorMessages.PackageVersionChanged);
            }

            insuredClientService.data = $scope.wizard.description;

            if ($scope.wizard.description.Model.QuoteId && $scope.wizard.description.IsProposal && !$scope.wizard.description.Model.RenderOnFirstStep) {
                shouldGoToLastStep = true;
            }
        });

        $scope.$on('WizardStepChanged', function (event, params) {

            //check if evaluatequestionnaire is already called before 
            if ($scope.wizard.description.ApplicableQuestionnaire && (params.to == 'paymentSummary' || !$scope.wizard.description.questionnaireVisibility)) {
                updateQuestionnaireVisibility();
            }

            if (params.from == 'productSelection') {
                var isViewMode = $scope.$root.rootShell.route.params().viewMode == 'true';
                if (!isViewMode) {
                    $scope.wizard.description.QuoteStatus = "new";
                    savedQuoteRequest = null;
                }
            }
            if (params.wizardName == $scope.wizard.description.Name) {
                if (params.to == 'requirements') {
                    var request = getProductEvaluationRequest();
                    //request.Covers = $scope.wizard.description.SelectedProduct.Premium.CustomData.CombinedPremium.CoverDetails;
                    request.Premium = $scope.wizard.description.SelectedProduct.Premium.CustomData.CombinedPremium;
                    var url = '/quoteapp/api/Package/EvaluateRequiredItems';
                    $scope.$root.rootShell.loader.isVisible = true;
                    JicWizardHelper.evaluateRequiredItems(url, request, $scope)
                        .then(function () {
                            $scope.$root.rootShell.loader.isVisible = false;
                        }, function () {
                            $scope.$root.rootShell.loader.isVisible = false;
                        });
                }
            }
        });

        $scope.init();
    }
    return WizardControllerBase;
});