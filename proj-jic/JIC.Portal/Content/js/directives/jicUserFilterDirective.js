define('jicUserFilterDirective', ['angular', 'jicServices', 'jicFilterControllerBase'], function (angular, jicServices, jicFilterControllerBase) {
    var m = angular.module('jic.jicUserFilterDirective', []);
    m.directive('jicUserFilter', function () {
        return {
            restrict: 'E',
            replace: true,
            require: '^amsList',
            templateUrl: '/Content/templates/jicUserFilter.html',
            controller: function ($scope) {
                jicFilterControllerBase($scope, 'jicuserfilter');
            },
            link: function (scope, iElement, attrs, listController) {

                jicFilterControllerBase.commonLinkFunction(scope, iElement, attrs, listController, 'jicFilter');

                scope.$root.$broadcast('scopeLoaded', { scope: scope, name: scope.name });
            }
        };
    });
    return m;
});