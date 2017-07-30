define('JicWizardHelper', function () {
    var
        _evaluateRequiredItems = function (url, request, $scope) {
            if (request.Model.SumInsured == null) {
                request.Model.SumInsured = 0;
            }
            return $scope.$root.rootShell.server.post(url, request)
            .then(function (response) {
                var i = 0;
                if ($scope.wizard.description.Model.ClientRequiredItems && response.ClientVisibilityResults) {
                    var clientItems = $scope.wizard.description.Model.ClientRequiredItems;
                    for (i = 0; i < clientItems.length; i++) {
                        clientItems[i].VisibleIf = response.ClientVisibilityResults[clientItems[i].Name];
                    }
                }
                if ($scope.wizard.description.Model.JointInsured &&
                    $scope.wizard.description.Model.JointInsuredRequiredItems && response.JointInsuredVisibilityResults) {
                    var jointInsuredItems = $scope.wizard.description.Model.JointInsuredRequiredItems;
                    for (i = 0; i < jointInsuredItems.length; i++) {
                        jointInsuredItems[i].VisibleIf = response.JointInsuredVisibilityResults[jointInsuredItems[i].Name];
                    }
                }
            }, function () {
                $scope.$root.rootShell.notification.show('Error while evaluating the required items conditions', 'error');
            });
        };

    return {
        evaluateRequiredItems: _evaluateRequiredItems
    };
});