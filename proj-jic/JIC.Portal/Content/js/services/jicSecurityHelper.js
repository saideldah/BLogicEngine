define('JicSecurityHelper', ['utils', 'jicServices'], function (utils, jicServices) {
    var profilesByOrder, self = this,
        isBR = function () {
        },
        isRA = function () {
        },
        isBK = function () {
        },
        isHR = function () {
        };


    self.ManageProposal = self.ManagePolicy = self.ManageClient = self.ManageSimulation = self.CloseAccount = self.EventLog = self.ManageSetup = {};

    jicServices.defineProperty(self.ManageProposal, 'Create')

});