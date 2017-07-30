define('jicBusinessFilterDirective', ['angular', 'jicFilterControllerBase'], function (angular, jicFilterControllerBase) {
    var m = angular.module('jic.jicBusinessFilterDirective', []);
    m.directive('policyFilter', function () {
        return {
            restrict: 'E',
            replace: true,
            require: '^amsList',
            scope: {
            },
            templateUrl: '/quoteapp/content/views/policyFilters.html',
            controller: function ($scope) {
                $scope.bankingProvider = window.bankingProvider;
                jicFilterControllerBase($scope, 'policyfilter');
            },
            link: function (scope, iElement, attrs, listController) {
                var rootShell = scope.$root.rootShell;

                rootShell.server.get('/quoteapp/api/policyList/GetPolicyDropDown', {
                    success: function (response) {
                        scope.PolicyStatus = response.PolicyStatus;
                        scope.PolicyProduct = response.PolicyProduct;
                        scope.$root.$broadcast('scopeLoaded', { scope: scope, name: scope.name });
                    }
                });

                jicFilterControllerBase.commonLinkFunction(scope, iElement, attrs, listController);
            }
        };
    })
    .directive('clientFilter', function () {
        return {
            restrict: 'E',
            replace: true,
            require: '^amsList',
            templateUrl: '/quoteapp/content/views/clientFilters.html',
            controller: function ($scope) {
                $scope.bankingProvider = window.bankingProvider;
                jicFilterControllerBase($scope, 'clientfilter');
            },
            link: function (scope, iElement, attrs, listController) {

                jicFilterControllerBase.commonLinkFunction(scope, iElement, attrs, listController);

                scope.$root.$broadcast('scopeLoaded', { scope: scope, name: scope.name });
            }
        };
    })
    .directive('endorsementFilter', function () {
        return {
            restrict: 'E',
            replace: true,
            require: '^amsList',
            templateUrl: '/quoteapp/content/views/endorsementFilters.html',
            controller: function ($scope) {

                var rootShell = $scope.$root.rootShell;

                jicFilterControllerBase($scope, 'endorsementfilter');

                var endorsementsChoiceApi = null;
                var endorsementTypesChoiceApi = null;
                $scope.endorsementOptions = {
                    SuggestedValues: [],
                    ValuePath: 'Id',
                    DisplayPath: 'Title',
                    Multiple: true,
                    LimitToSuggestions: true
                };
                $scope.endorsementTypeOptions = {
                    SuggestedValues: [],
                    ValuePath: 'Id',
                    DisplayPath: 'Title',
                    Multiple: true,
                    LimitToSuggestions: true
                };
                $scope.endorsementsChoiceInit = function (api) {
                    endorsementsChoiceApi = api;
                };
                $scope.endorsementTypesChoiceInit = function (api) {
                    endorsementTypesChoiceApi = api;
                };

                $scope.init = function () {
                    rootShell.server.get('/quoteapp/api/endorsementList/Get', {
                        success: function (response) {
                            endorsementsChoiceApi.addItems(response.EndorsementRequestStatuses);
                            endorsementTypesChoiceApi.addItems(response.EndorsementRequestTypes);
                            $scope.$root.$broadcast('scopeLoaded', { scope: $scope, name: $scope.name });
                        }
                    });
                };
            },
            link: function (scope, iElement, attrs, listController) {

                jicFilterControllerBase.commonLinkFunction(scope, iElement, attrs, listController);

                scope.init();
            }
        };
    });
    return m;
});