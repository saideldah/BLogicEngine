define('jicFilterControllerBase', ['angular', 'jicServices'], function (angular, jicServices) {
    function jicFilterControllerBase($scope, name) {
        var rootShell = $scope.$root.rootShell;
        $scope.resources = {};
        angular.extend($scope.resources, rootShell.localization.Filters, rootShell.localization.clientIdentification, rootShell.localization.common);

        $scope.name = name;

        $scope.getState = function () {
            return $scope.model;
        };

        $scope.setState = function (s) {
            if (!$scope.model) {
                $scope.model = {};
            }
            if (s) {
                angular.extend($scope.model, s);
                if (typeof $scope.filter == 'function') {
                    $scope.filter();
                }
            }
        };

    }


    jicFilterControllerBase.commonLinkFunction = function (scope, iElement, attrs, listController, filterName) {
        if (!filterName || typeof filterName != 'string') {
            filterName = 'filter';
        }
        var rootShell = scope.$root.rootShell;
        var listManager = listController.getListManager();

        scope.model = {};

        scope.formValidation = function (notValidated, isEmpty) {
            return notValidated || isEmpty;
        };

        scope.backToIndex = function () {
            rootShell.navigation.goTo('/page/index.html');
        };

        scope.filter = function (callback) {
            var filter = {
                filter: {
                    ActionName: filterName
                },
                data: scope.model
            };
            return jicServices.filterUsingListManager(listManager, filter, callback, rootShell);
        };
    };

    return jicFilterControllerBase;
});