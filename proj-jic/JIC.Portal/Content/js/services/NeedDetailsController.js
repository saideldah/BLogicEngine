define('NeedDetailsController', ['angular', 'utils', 'jicServices'], function (angular, utils, jicServices) {

    var
        parsedFormulas = {},
        replaceWithMathFunction = function (formula) {
            var regex = new RegExp('(Min|Max)\\(', 'g');
            formula = formula.replace(regex, function (match, p1) {
                return 'Math.' + p1.toLowerCase() + '(';
            });
            return formula;
        },
        parseFormula = function (formula) {
            if (!parsedFormulas[formula]) {
                var smiExpression = new RegExp('(Smi\\.)', 'g');
                parsedFormulas[formula] = replaceWithMathFunction(formula).replace(smiExpression, 'api.description.Model.');
            }
            return parsedFormulas[formula];
        },
        evaluateFormula = function (parameter, $scope) {
            var parsedFormula = parseFormula(parameter.Value);
            var result = $scope.$eval(parsedFormula);
            return result;
        },
        initComponent = function (self, callbacks) {
            self.$scope.modelKeys = {};
            self.$scope.packages = {
                constraints: { min: {}, max: {}, step: {}, validator: {} },
                suggestedvalues: {},
                visibility: {},
                isRequired: {},
                isVisible: {},
                isEnabled: {}
            };
            self.$scope.isNumber = function (n) {
                return !isNaN(parseFloat(n)) && isFinite(n);
            };
            self.$scope.Math = window.Math;
            self.$scope.packageParameters = self.$scope.api.description[self.wizardParameterName];
            self.$scope.constraints = { min: {}, max: {}, step: {} };
            self.$scope.model = self.$scope.api.description.Model;
            self.$scope.packageName = self.$scope.api.description.Name;
            fireCallBack(callbacks, 'initComponent');

        },
        assignParameterValue = function (self, parameter, valuePath, $scope, setDefaultValue) {
            var fn = null;
            switch (parameter.ValueType) {
                case 'external':
                    //fn = $scope[parameter.Value];
                    fn = parameter.Value;
                    break;
                case 'formula':
                    fn = evaluateFormula;
                    break;
                default:
            }
            if (fn) {
                var map = self.watchCallbacks[parameter.Name];
                if (!map) {
                    map = self.watchCallbacks[parameter.Name] = { parameter: parameter, valuePath: valuePath, fn: fn, wasAssigned: !setDefaultValue };
                }
            } else {
                var isDefaultValueParam = parameter.Type === 'DefaultValue';
                if ((isDefaultValueParam && setDefaultValue) || !isDefaultValueParam) {
                    utils.assignObjectProperty($scope, valuePath, parameter.Value);
                }
            }
        },
        applyPackageParameter = function (self, callbacks) {
            var $scope = self.$scope;
            var pp = $scope.packageParameters;
            $scope.Parameter = {};
            var filterByAllowedValues = function (allValues, allowedValues) {
                var suggestedValues = [];

                angular.forEach(allValues,
                    function (item) {
                        if (utils.arrayFirst(allowedValues, function (it) {
                            return item.Name == it || item.Name == it.id;
                        }))
                            suggestedValues.push(item);
                    });
                return suggestedValues;
            },
                getSuggestedValues = function (fieldName, allowedValues) {
                    var allValues = null;
                    //this piece of code is to change the visibility of any choice atom to a simple lable if the size the the choice item is 1 , due to business requirement
                    if (allowedValues.length == 1) {
                        $scope.packages.visibility[fieldName] = false;
                        $scope.packages.visibility[fieldName + "AsLabel"] = true;
                        $scope.model[fieldName] = allowedValues[0];
                    }
                    //allValues = $scope.api.description.OptionSets[fieldName];
                    if ('CURRENCY' == fieldName.toUpperCase()) {
                        allValues = $scope.api.description.OptionSets.Currency;
                    } else if ('PAYMENTFREQUENCY' == fieldName.toUpperCase() || 'PF' == fieldName.toUpperCase()) {
                        allValues = $scope.api.description.OptionSets.PaymentFrequency;
                    }
                    else if ("BENEFICIARY.NUMBEROFUNITS" == fieldName.toUpperCase()) {
                        allValues = [];
                        for (var i = 0; i < allowedValues.length; i++)
                            allValues.push({ Id:allowedValues[i], Name: allowedValues[i], Title: allowedValues[i] });
                    }
                    else {
                        if (allowedValues) {
                            {
                                allValues = [];
                                for (var i = 0; i < allowedValues.length; i++)
                                    allValues.push({ Id: i, Name: allowedValues[i], Title: allowedValues[i] });
                            }
                        }
                    }

                    if (!allValues || allValues.length == 0) {
                        return [];
                    }
                    return filterByAllowedValues(allValues, allowedValues);
                },
                dealWithPropertyValue = function (pv) {
                    var i = this;
                    var parameter = angular.copy(pv);
                    $scope.Parameter[parameter.Name] = parameter.Value;
                    var valuePath = null;
                    var applyValueAssignment = true;
                    if (parameter.Type === 'DefaultValue' || parameter.Type === 'Value') {
                        parameter.Value = parameter.DataType == 'Choice' ? parameter.Value[0] : parameter.Value;
                        valuePath = 'model';
                        applyValueAssignment = (!$scope.model.QuoteId);
                    } else if (parameter.Type === 'Min') {
                        valuePath = 'packages.constraints.min';
                    } else if (parameter.Type === 'Max') {
                        valuePath = 'packages.constraints.max';
                    } else if (parameter.Type === 'AllowedSelectedValues') {
                        if (i && parameter.Value) {
                            var _suggestedValues = getSuggestedValues.apply(null, [i, parameter.Value]);
                            $scope.packages.suggestedvalues[i] = _suggestedValues;
                        }
                    }
                    else if (parameter.Type === 'CustomValidator') {
                        valuePath = 'packages.constraints.validator';
                    }
                    else if (parameter.Type === 'IsRequired') {
                        $scope.packages.isRequired[i.toString()] = parameter.Value;
                    } else if (parameter.Type === 'Visibility') {
                        $scope.packages.visibility[i.toString()] = parameter.Value;
                    } else if (parameter.Type === 'IsEnabled') {
                        $scope.packages.isEnabled[i.toString()] = parameter.Value;
                    }

                    if (valuePath) {
                        valuePath = valuePath + '.' + i;
                        assignParameterValue(self, parameter, valuePath, $scope, applyValueAssignment === true);
                    }
                };
            for (var j in pp) {
                if (pp.hasOwnProperty(j)) {
                    $scope.packages.visibility[j] = true;
                    var pProperty = pp[j]; // it stand for package property
                    angular.forEach(pProperty, dealWithPropertyValue, j);
                }
            }

            if ($scope.api.description.Model.Visibility && (!$scope.api.description.Model.OldVisibility)) {
                $scope.api.description.Model.OldVisibility = $scope.api.description.Model.Visibility;
            }
            $scope.api.description.Model.Visibility = $scope.packages.visibility;
            $scope.calculateNoLongerVisibleFields();
            fireCallBack(callbacks, 'applyPackageParameter');
        },
        fireCallBack = function (callbacks, fnName) {
            if (callbacks && callbacks[fnName]) callbacks[fnName].apply();
        };
    function NeedDetailsController($scope, wizardParameterName) {
        var self = this;
        self.rootShell = $scope.$root.rootShell,

        self.$scope = $scope;
        self.wizardParameterName = wizardParameterName;
        self.validateStep = null;
        self.watchCallbacks = {};
        var checkValidity = function (modelKey) {
            var result = true;;
            if (modelKey) {
                if (modelKey.hasOwnProperty('$valid'))
                    return modelKey.$valid;

                for (var i in modelKey) {
                    if (modelKey.hasOwnProperty(i)) {
                        result = result && checkValidity(modelKey[i]);
                    }
                    if (!result)
                        break;
                }
            }
            return result;
        };
        $scope.isValidStep = function () {

            var modelKeys = $scope.modelKeys;
            var result = checkValidity(modelKeys);
            if (result === true) {
                if (typeof self.validateStep == 'function') {
                    result = self.validateStep();
                }
            }
            return result;
        };

        $scope.calculateNoLongerVisibleFields = function () {
            var getPackageParameterTitle = function ($scope, name) {
                var title = null;
                if($scope.packageParameters) {
                    var ppArray = $scope.packageParameters[name];
                    if (ppArray && ppArray.length > 0) {
                        var pp = utils.find(ppArray, 'Type', 'Visibility');
                        if (pp) {
                            title = pp.Title;
                        }
                    }
                }
                return title;
            };
            var noLongerVisibleFieldNames = [];
            var oldVisibility = $scope.api.description.Model.OldVisibility;
            var newVisibility = $scope.api.description.Model.Visibility;
            if (oldVisibility && newVisibility) {
                for (var i in oldVisibility) {
                    if (oldVisibility.hasOwnProperty(i)) {
                        if (oldVisibility[i] === true && (!newVisibility[i])) {
                            var title = getPackageParameterTitle($scope, i);
                            if (title) {
                                noLongerVisibleFieldNames.push({ Name: i, Title: title });
                            }
                        }
                    }
                }
            }
            $scope.noLongerVisibleFields = noLongerVisibleFieldNames;
        };

        $scope.parser = function (value) {
            var parts = value.toString().split(".");
            parts[0] = parts[0].replace(/\,/g, "");
            var result = parts.join(".");

            return result;
        };

        $scope.formatter = function (value) {
            return jicServices.formatter(value);

        }

        $scope.applySliderText = function (item) {
            return item + ' ' + jicServices.getCurrencyTitle($scope);
        };

        $scope.getMaturityDate = function () {
            var duration = $scope.model.Duration;
            if (window.isNaN(duration)) {
                duration = 0;
            }
            var d = new Date();
            var year = parseInt((d.getFullYear() + duration));
            var mnth = ((d.getMonth() + 1));
            var day = (d.getDate());

            return day + " / " + mnth + " / " + year;
        };

        $scope.doOnCoverClick = function (cover) {
            angular.forEach(cover.SecondaryCovers, function (item) {
                item.IsSelected = cover.IsSelected;
            });
        };

        $scope.getInsuredAgetAtMaturity = function () {
            var duration = $scope.model.Duration;
            if (window.isNaN(duration)) {
                duration = 0;
            }
            var insuredAgetAtMaturity = null;
            if ($scope.model.Client) {
                insuredAgetAtMaturity = ($scope.model.Client.Age + duration);
            }
            return insuredAgetAtMaturity;
        };

        $scope.formattedCurrency = function () {
            return jicServices.getCurrencyTitle($scope);
        }

        $scope.getPaymentEndDate = function () {
            var dt = new Date();
            if ($scope.model.PaymentDuration) {
                dt.setFullYear(dt.getFullYear() + $scope.model.PaymentDuration);
            }
            return dt.getDate() + " / " + ((dt.getMonth() + 1)) + " / " + dt.getFullYear();
        };

        $scope.getState = function () {
            var s = {};
            if ($scope.model) {
                s.Model = $scope.model;
            }
            if ($scope.api.description.Model.AvailableInsuredClients) {
                s.AvailableInsuredClients = $scope.api.description.Model.AvailableInsuredClients;
            }
            return s;
        };

        $scope.setState = function (s) {
            if (s) {
                if (s.Model) {
                    angular.extend($scope.model, s.Model);
                    $scope.NoNeedToConvertSumInsured = true;
                }
                if (s.AvailableInsuredClients) {
                    $scope.api.description.Model.AvailableInsuredClients = s.AvailableInsuredClients;
                }
            }
        };
        self.updateDurationFormula = function () {
            if (self.$scope.api.description.Model.JointInsured) {
                if (self.watchCallbacks['maxduration'] && self.watchCallbacks['maxduration'].parameter.Value.indexOf('Client') > -1) {
                    self.watchCallbacks['maxduration'].parameter.Value = self.watchCallbacks['maxduration'].parameter.Value.replace('Client', 'JointInsured');
                }
            }
            else if (!self.$scope.api.description.Model.JointInsured) {
                if (self.watchCallbacks['maxduration'] && self.watchCallbacks['maxduration'].parameter.Value.indexOf('JointInsured') > -1) {
                    self.watchCallbacks['maxduration'].parameter.Value = self.watchCallbacks['maxduration'].parameter.Value.replace('JointInsured', 'Client');
                }
            }
        };
    }

    NeedDetailsController.prototype = {

        init: function (callbacks) {
            var self = this;
            fireCallBack(callbacks, 'initResources');
            initComponent(this, callbacks);
            applyPackageParameter(this, callbacks);
            var deepWatchCallback = function () {
                for (var index in self.watchCallbacks) {
                    if (self.watchCallbacks.hasOwnProperty(index)) {
                        var map = self.watchCallbacks[index];
                        if (map) {
                            var isDefaultValueParam = map.parameter.Type === 'DefaultValue';
                            if ((!isDefaultValueParam || (isDefaultValueParam && !map.parameter.wasAssigned)) && (!isDefaultValueParam || !self.$scope.api.description.Model.QuoteId)) {
                                var fn = typeof map.fn === 'function' ? map.fn : (self.$scope[map.fn] || self.$scope.$parent[map.fn]);
                                if (fn) {
                                    var value = fn(map.parameter, self.$scope);
                                    utils.assignObjectProperty(self.$scope, map.valuePath, value);
                                    if (isDefaultValueParam) {
                                        map.parameter.wasAssigned = true;
                                    }
                                }
                            }
                        }
                    }
                }
            };
            self.$scope.$on('jointInsuredUpdated', self.updateDurationFormula);
            // Identifying scope with a unique name 
            self.$scope.name = 'needdetails';

            var scopeLoadedCallbackCalled = false;
            var scopeLoadedCallback = function () {
                if (!scopeLoadedCallbackCalled) {
                    scopeLoadedCallbackCalled = true;

                    // Broadcasting scopeLoaded to cater for maintaining states
                    self.$scope.$root.$broadcast('scopeLoaded', { scope: self.$scope, name: self.$scope.name });
                }
            };

            self.$scope.$watch('api.description.Model', function (newVal) {
                if (newVal) {
                    deepWatchCallback();
                    scopeLoadedCallback();
                }
            }, true);
            self.$scope.api.description.Model.formattedCurrency = function () {
                return jicServices.getCurrencyTitle(self.$scope);
            };

            //self.$scope.$watch('api.description.Model.Client.Age', deepWatchCallback);
            //self.$scope.$watch('api.description.Model.Duration', deepWatchCallback);
        }
    };
    return NeedDetailsController;
});