define('accountController', ['jQuery', 'angular'], function ($, angular) {
    var AccountController = function ($scope, $location, identity, $timeout) {
        $scope.identity = identity;
        $scope.showView = 'login';
        $scope.loginModel = $scope.loginModel || {};
        $scope.errorMessage = null;
        $scope.errorMessageDescription = null;
        $scope.showErrorMessage = false;
        $scope.loginModel.username = null;
        
        $scope.initialize = function() {
            $scope.hasFAProvider = false;
            $scope.hasADProvider = false;
            $scope.otherProviders = [];
            $scope.siteData = {};
            $scope.siteId = null;
        };

        $scope.setOtherProviders = function() {
            angular.forEach($scope.siteData.Providers, function(type) {
                if (type != 'FA' && type != 'Windows') {
                    $scope.otherProviders.push(type);
                } else if (type == 'FA') {
                    $scope.hasFAProvider = true;
                } else {
                    $scope.hasADProvider = true;
                }
            });
        };

        $scope.showError = function(errorMessage, description) {
            $scope.$root.safeApply(function() {
                $scope.showErrorMessage = true;
                $scope.errorMessage = errorMessage;
                $scope.errorMessageDescription = description;
            });

            $timeout(function() {
                $scope.showErrorMessage = false;
            }, 5000);
        };
        
        $scope.cancel = function (callback) {
            $scope.showView = 'login';
            callback();
        };

        $scope.forgotPassword = function () {
            $scope.showView = 'forgotPassword';
        };

        $scope.sendForgotPasswordRequest = function (callback) {
            $scope.$root.rootShell.server.post('/api/UserAccount/ForgotPassword', {
                Email: $scope.loginModel.username
            }, {
                success: function (data, status) {
                    $scope.showView = 'login';
                    callback();
                },
                error: function (data, status) {
                    $scope.showError('Failed to submit !');
                    callback();
                }
            });
        };

        $scope.initialize();
    };

    AccountController.$inject = ['$scope', '$location', 'identity', '$timeout'];

    return AccountController;
});