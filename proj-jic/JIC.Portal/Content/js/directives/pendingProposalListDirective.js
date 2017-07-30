define('pendingProposalListDirective', ['angular', 'utils', 'jicServices'], function (angular, utils, jicServices) {
    return angular.module('pendingProposalListDirective', [])
        .directive('pendingProposals', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    loader: '='
                },
                templateUrl: '/content/templates/pendingProposalListTemplate.html',
                controller: function ($scope, $sce) {
                    $scope.serverController = '/quoteapp/api/ProposalList';
                    $scope.resources = {};
                    angular.extend($scope.resources, $scope.$root.rootShell.localization.common);
                    $scope.context =
                    {
                        validate: function (a) {
                            var deferred = $scope.$root.rootShell.$q.defer();
                            jicServices.getPoliciesWithNoSignature($scope, a.Data.Matricule, a.Data.JointMatricule, null, true).then(function () {

                                $scope.$root.rootShell.modal.confirm($scope.resources.Validation).then(function (result) {

                                    $scope.$root.rootShell.server.get('/quoteapp/api/ProposalList/Validate?entityId=' + a.Data.Title)
                                    .then(function (result) {
                                        deferred.resolve(result);
                                        jicServices.showCustomMessageAfterValidation($scope, result.WithCoverNotes, result.WithPolicy, result.ProposalNumb);

                                    });

                                }, function () { deferred.reject(); });
                            });
                            return deferred.promise;
                        },
                        view: function (a) {
                            $scope.$root.rootShell.server.get('/quoteapp/api/ProposalList/AllowAction?entityId=' + a.Data.Title + '&entityName=Quote&action=view')
                            .then(function (result) {
                                if (result.canAccess) {
                                    var url = a.Data.UrlPath;
                                    url = utils.addUrlParameter(url, 'viewMode', true);
                                    $scope.$root.rootShell.navigation.goTo('/page/app/quoteapp/policyDetails.html?id=' + a.Data.Title + '&viewType=proposal');
                                }
                                else {
                                    $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.common.AccessDenied);
                                }
                            });
                        },
                        modify: function (a) {
                            $scope.$root.rootShell.server.get('/quoteapp/api/ProposalList/AllowAction?entityId=' + a.Data.Title + '&entityName=Quote&action=modify')
                            .then(function (result) {
                                if (result.canAccess) {
                                    var url = a.Data.UrlPath;
                                    $scope.$root.rootShell.navigation.goTo(url);
                                }
                                else {
                                    $scope.$root.rootShell.modal.error($scope.$root.rootShell.localization.common.AccessDenied);
                                }
                            });
                        }
                    };
                }
            };
        });

});