define('beneficiaryClausesDirective', ['jQuery', 'angular', 'utils'], function ($, angular, utils) {
    var m = angular.module('jic.beneficiaryClausesDirective', []);
    m.directive("clausesManager", function ($compile, $filter) {
        return {
            restrict: 'E',
            scope: {
                clausesdatamodel: '=',
                clausetemplate: '=',
                iseditmode: '=',
                beneficiaryproperty: '@'
            },
            template: function (elem, attr) {
                var d = new Date();
                var n = d.getTime();
                return '<form name="undefined" id="undefined" data-atom-form ams-load-content="htmlcontent"></form>';
            },
            link: function (scope, element, attrs) {
                scope.parameterArr = [];
                var
                    getTemplateInfo = function (item, variable) {
                        var templateInfo = {};
                        templateInfo.model = "relationclause.datamodel." + variable.Name;
                        templateInfo.clauseTemplate = item.Template;
                        templateInfo.IsEditable = item.IsEditable;
                        templateInfo.variableName = variable.Name;
                        templateInfo.parameter = '@' + variable.Name;
                        templateInfo.DefaultValue = variable.DefaultValue;
                        templateInfo.TextLabel = variable.TextLabel;
                        templateInfo.Type = variable.Type;
                        if (scope.iseditmode && !templateInfo.IsEditable) {
                            scope.clauseTemplateWithBindedParameter = '<span style="font-weight:bold">' + scope.clausesdatamodel[scope.beneficiaryproperty] + '</span>';
                        }
                        else {
                            scope.clauseTemplateWithBindedParameter = scope.clauseTemplateWithBindedParameter.replace(templateInfo.parameter, '<span style="font-weight:bold" ams-bind-html="' + templateInfo.model + '"></span>');
                        }
                        return templateInfo;
                    },
                    getFinalTemplate = function () {
                        if (!scope.clausetemplate) return;
                        var html = "",
                            fullTemplate = scope.clausetemplate.Template;
                        
                        if (!scope.relationclause.constraints.visibility.edit && !scope.iseditmode) {
                            angular.forEach(scope.parameterArr, function (param) {
                                fullTemplate = fullTemplate.replace(param.parameter, scope.relationclause.datamodel[param.variableName]);
                            });
                        }
                        else if (scope.iseditmode) {
                            fullTemplate = scope.clausesdatamodel[scope.beneficiaryproperty];
                        } else {
                            if (fullTemplate == null)
                            fullTemplate = scope.relationclause.model.edit;
                        }
                        if (fullTemplate) {
                                fullTemplate = fullTemplate.replace(/<br\s*\/?>/mg, " \n ");
                        }
                        return fullTemplate;
                    },
                    doOnEditClick = function () {
                        scope.relationclause.model.edit = getFinalTemplate();
                        scope.relationclause.constraints.visibility.template = false;
                        if (scope.clausetemplate.IsEditable) {
                            scope.relationclause.constraints.visibility.edit = true;
                        }
                        else if (scope.iseditmode) {
                            scope.relationclause.constraints.visibility.template = true;
                        }
                        scope.$parent.clauseSet.isValid = true;
                    },
                    addRelation = function () {
                        scope.relationclause.relations.push({});
                        scope.relationclause.constraints.visibility.relation.push({ specifyrelation: false, fatherName: false, dob: false });
                        scope.relationclause.model.relation.push({ datavalue: scope.relationclause.relation.suggestedvalues[0].Id });
                        doonrelationchange(scope.relationclause.model.relation[scope.relationclause.relations.length - 1].datavalue, scope.relationclause.relations.length - 1);
                    },
                    deleteRelation = function (index) {
                        scope.relationclause.relations.splice(index, 1);
                        scope.relationclause.constraints.visibility.relation.splice(index, 1);
                        scope.relationclause.model.relation.splice(index, 1);
                    },
                    getRelationTemplate = function (relation) {
                        scope.isValidPercent = true;
                        var arr = [];
                        if (relation.name) arr.push(relation.name);
                        if (relation.fatherName) arr.push(relation.fatherName);
                        if (relation.dob) arr.push($filter('date')(relation.dob, "dd/MM/yyyy"));
                        if (relation.percentage) {
                            if (relation.percentage < scope.minimumAllowedPercentagePerPerson) {
                                scope.isValidPercent = false;
                            }
                            else {
                                arr.push(relation.percentage + ' %');
                            }
                        }
                        return [arr.join(','), relation.percentage ? relation.percentage : 0];
                    },
                    validateRelations = function (variableName) {
                        var percentSumm = 0,
                            allcloses = [],
                            relationTemplate = null;
                        angular.forEach(scope.relationclause.model.relation, function (rel) {
                            relationTemplate = getRelationTemplate(rel);
                            if (rel.specifyrelation) {
                                allcloses.push(rel.specifyrelation + ' : ' + relationTemplate[0]);
                            }
                            else {
                                allcloses.push(rel.datavalue + ' : ' + relationTemplate[0]);
                            }
                            percentSumm += relationTemplate[1];
                        });
                        if (percentSumm == 100) {
                        scope.relationclause.datamodel[variableName] = allcloses.join(' <br> ');
                            scope.isValidSum = true;
                            scope.$parent.clauseSet.isValid = true;
                        }
                        else
                            scope.isValidSum = false;
                    },
                    doonrelationchange = function (relationName, index) {
                        var relationType = null;
                        angular.forEach(scope.relationclause.relation.suggestedvalues, function (value) {
                            if (value.Id == relationName) relationType = value.Name;
                        });
                        if (!relationType) return;
                        switch (relationType) {
                            case 'Relation_Details':
                                scope.relationclause.constraints.visibility.relation[index].specifyrelation = false;
                                scope.relationclause.model.relation[index].specifyrelation = null;
                                scope.relationclause.constraints.visibility.relation[index].fatherName = true;
                                scope.relationclause.constraints.visibility.relation[index].dob = true;
                                break;
                            case 'Relation_NewRelationWithDetails':
                                scope.relationclause.constraints.visibility.relation[index].specifyrelation = true;
                                scope.relationclause.constraints.visibility.relation[index].fatherName = true;
                                scope.relationclause.constraints.visibility.relation[index].dob = true;
                                break;
                            case 'Relation_NoDetails':
                                scope.relationclause.constraints.visibility.relation[index].specifyrelation = false;
                                scope.relationclause.model.relation[index].specifyrelation = null;
                                scope.relationclause.constraints.visibility.relation[index].fatherName = false;
                                scope.relationclause.model.relation[index].fatherName = null;
                                scope.relationclause.constraints.visibility.relation[index].dob = false;
                                scope.relationclause.model.relation[index].dob = null;
                                break;
                        }
                    },
                     getEditHtml = function () {
                         var html = '<div class="col-md-3"><div ng-if="clausetemplate.IsEditable"><button type="button" ng-click="doOnEditClick()" class="btn btn-primary btn-block">Edit</button></div></div>';
                         return html;
                     },
                    getTextHtml = function (param) {
                        if (!param.TextLabel) {
                            var html = '<div class="col-md-12" style="margin-bottom: 25px;"><atom type="\'multilinetext\'" name="\'' + param.variableName + '\'" datavalue="' + param.model + '"' +
                                                                    'withlabel="false" required="true"></atom>' +
                                                                    '</div>';
                        } else {
                            var html = '<div class="col-md-12" style="margin-top: 25px;"><atom type="\'multilinetext\'" name="\'' + param.variableName + '\'" watermark=" \'' + param.TextLabel + '\'" datavalue="' + param.model + '"' +
                                        'withlabel="false" required="true"></atom>' +
                                        '</div>';
                        }
                        return html;
                    },
                    getRelationHtml = function (param) {
                        return '<div class="col-md-12">' +
                                    '<div class="row" ng-show="!isValidPercent">' +
                                        '<div class="col-xs-12 text-danger bg-tooltip sg-error-message">' +
                                            '<i class="fa fa-exclamation-triangle"></i> The Percentage must be grater than ' + scope.minimumAllowedPercentagePerPerson + '.' +
                                        '</div>' +
                                    '</div>' +
                                    '<div class="row" ng-show="!isValidSum">' +
                                        '<div class="col-xs-12 text-danger bg-tooltip sg-error-message">' +
                                            '<i class="fa fa-exclamation-triangle"></i> The sum of the Percentage values must be 100%.' +
                                        '</div>' +
                                    '</div>' +
                                    '<div class="row well well-sm relation-well" ng-if="' + !scope.iseditmode + '" ng-repeat="rel in relationclause.relations">' +
                                            '<button ng-click="deleteRelation($index)" type="button" ng-disabled="relationclause.relations.length == 1" class="btn btn-link relation-remove">' +
                                                '<span class="fa fa-times" aria-hidden="true"></span>' +
                                            '</button>' +
                                           '<div class="col-md-10">' +
                                                 '<div class="col-md-4"><atom type="\'choice\'" datavalue="relationclause.model.relation[$index].datavalue" suggestedvalues="relationclause.relation.suggestedvalues" ' +
                                                'name="\'Relation\'" valuepath="\'Id\'" displaypath="\'Title\'" changehandler="doonrelationchange" required="true" changeHandlerParam="[$index]"></atom></div> ' +
                                                '<div class="col-md-4"><atom type="\'number\'" restricttonumbers="true" max="100" min="minimumAllowedPercentagePerPerson" datavalue="relationclause.model.relation[$index].percentage" ' +
                                                    'name="\'Percent\'" withlabel="true" required="true"></atom></div>' +
                                                '<div class="col-md-4"><atom type="\'text\'" datavalue="relationclause.model.relation[$index].name" ' +
                                                    'name="\'Name\'" withlabel="true" required="true" ></atom></div>' +
                                            '</div>' +
                                            '<div class="col-md-10">' +
                                                '<div class="col-md-4" ng-if="relationclause.constraints.visibility.relation[$index].specifyrelation"><atom type="\'text\'" datavalue="relationclause.model.relation[$index].specifyrelation" ' +
                                                    'name="\'Specify Relation\'" withlabel="true" required="true"></atom></div>' +
                                                '<div class="col-md-4" ng-if="relationclause.constraints.visibility.relation[$index].fatherName"><atom type="\'text\'" datavalue="relationclause.model.relation[$index].fatherName" ' +
                                                'name="\'Father Name\'" withlabel="true" required="true"></atom></div>' +
                                                '<div class="col-md-4" ng-if="relationclause.constraints.visibility.relation[$index].dob"><atom version="2" max="dobmax" type="\'date\'" datavalue="relationclause.model.relation[$index].dob" ' +
                                                    'name="\'Date of birth\'" withlabel="true" required="true"></atom></div>' +
                                            '</div>' +
                                           '</div>' +
                                        '<div class="row" ng-if="' + !scope.iseditmode + '">' +
                                                    '<button ng-click="addRelation()" ng-disabled="relationclause.relations.length == maxNumberOfRelations" type="button" class="btn btn-secondary pull-left" ng-show="relationclause.relations.length < maxNumberOfRelations">' +
                                                        '<span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Add Beneficiary' +
                                                    '</button>' +
                                            '<button ng-disabled="undefined.$invalid" ng-click="validateRelations(\'' + param.variableName + '\')" type="button" class="btn btn-primary pull-right"  aria-label="Left Align">' +
                                                '<span class="glyphicon glyphicon-ok" aria-hidden="true"></span> Validate Beneficiary' +
                                            '</button>' +
                                        '</div>' +
                                '</div>';
                    },
                    clausetemplateWatcher = function () {
                        var
                            html, htmlLabel, htmlInput = '',
                            templateInfo = null;
                        paramCount = 0;
                        if (scope.clausetemplate) {
                            scope.parameterArr = [];
                            scope.relationclause.relation.suggestedvalues = [];
                            html = '<div ng-if="relationclause.constraints.visibility.edit" class="row edittextwraper">' +
                                   '<div class="col-md-10"><atom type="\'multilinetext\'" withlabel="false" name="clausetemplate.Title" datavalue="relationclause.model.edit" required="true"></atom>' +
                                   '</div>' +
                                    '</div>' +
                                 '<div ng-if="relationclause.constraints.visibility.template" class="row">';
                            scope.clauseTemplateWithBindedParameter = scope.clausetemplate.Template;
                            if (scope.clausetemplate.IsEditable) {
                                html += getEditHtml();
                            }
                            if (scope.clausetemplate.Variables && scope.clausetemplate.Variables.length) {
                               
                                angular.forEach(scope.clausetemplate.Variables, function (variable) {
                                    var paramDoublication = false;
                                    templateInfo = getTemplateInfo(scope.clausetemplate, variable);
                                    scope.relationclause.datamodel[variable.Name] = variable.DefaultValue;
                                    scope.parameterArr.push(templateInfo);
                                    if (templateInfo.Type.toUpperCase() == 'text'.toUpperCase()) {
                                        for (var i = 0; i < paramCount; i++) {
                                            if (templateInfo.parameter == '@' + scope.clausetemplate.Variables[i].Name) {
                                                paramDoublication = true;
                                            }
                                        }
                                        if (!paramDoublication && !scope.iseditmode) {
                                            htmlInput += getTextHtml(templateInfo);
                                        }
                                        scope.$parent.clauseSet.isValid = true;
                                        paramCount++;
                                    }
                                    else if (templateInfo.Type.toUpperCase() == 'relation'.toUpperCase()) {
                                        scope.$parent.clauseSet.isValid = false;
                                        scope.relationclause.relations = [{}];
                                        scope.relationclause.model.relation.splice(0, scope.relationclause.model.relation.length);
                                        scope.relationclause.model.relation.push({ datavalue: null });
                                        scope.maxNumberOfRelations = variable.MaxNumberOfRelations;
                                        scope.minimumAllowedPercentagePerPerson = variable.MinimumAllowedPercentagePerPerson;
                                        scope.dobmax = new Date();
                                        for (var i = 0; i < paramCount; i++) {
                                            if (templateInfo.parameter == '@' + scope.clausetemplate.Variables[i].Name) {
                                                paramDoublication = true;
                                            }
                                        }
                                        if (!paramDoublication && !scope.iseditmode) {
                                            htmlInput += getRelationHtml(templateInfo, scope);
                                        }
                                        paramCount++;
                                        angular.forEach(variable.Options, function (option) {
                                            scope.relationclause.relation.suggestedvalues.push({ Id: option.Title, Title: option.Title, Name: option.RelationType });
                                        });
                                        if (scope.relationclause.relation.suggestedvalues.length > 1) {
                                            scope.relationclause.model.relation[0].datavalue = scope.relationclause.relation.suggestedvalues[1].Id;
                                            doonrelationchange(scope.relationclause.model.relation[0].datavalue, 0);
                                        }
                                    }
                                });
                            } else {
                                scope.clausesdatamodel[scope.beneficiaryproperty] = getFinalTemplate();
                            }
                            if (!scope.iseditmode) {
                                if (scope.clauseTemplateWithBindedParameter == null) {
                                    scope.iseditmode = true;
                                } else {
                                    scope.clauseTemplateWithBindedParameter = scope.clauseTemplateWithBindedParameter.replace(/\r\n|\r|\n/g, "<br>");
                                }
                            }
                            htmlLabel=     '<div class="col-md-12"><label>' + scope.clauseTemplateWithBindedParameter + '</label></div>';
                            html += htmlLabel + htmlInput + '</div>';
                            scope.htmlcontent = html;
                            scope.relationclause.constraints.visibility.template = true;
                            scope.relationclause.constraints.visibility.edit = false;
                        } else {
                            scope.htmlcontent = null;
                            scope.relationclause.constraints.visibility.template = false;
                            scope.relationclause.constraints.visibility.edit = false;
                            scope.relationclause.model.edit = null;
                            scope.clausesdatamodel[scope.beneficiaryproperty] = null;
                        }
                        if (scope.iseditmode) {
                            doOnEditClick();
                            scope.iseditmode = false;
                        }
                        scope.iseditmode = false;
                    },
                    clauseTemplateWithBindedParameter = function () {
                        if (scope.relationclause.constraints && scope.relationclause.constraints.visibility && scope.relationclause.constraints.visibility.template &&
                            scope.clausetemplate && scope.clausetemplate.Variables.length == 0) {
                            scope.$parent.clauseSet.isValid = true;
                        }
                        angular.forEach(scope.clausetemplate.Variables, function (param) {
                            if (scope.relationclause.datamodel && !scope.relationclause.datamodel[param.Name]) {
                                scope.$parent.clauseSet.isValid = false;
                            }
                        })
                    },
                    relationclauseModelWatcher = function () {
                        angular.forEach(scope.clausetemplate.Variables, function (param) {
                            if (scope.relationclause.datamodel[param.Name]) {
                                scope.relationclause.datamodel[param.Name] = scope.relationclause.datamodel[param.Name].toUpperCase();
                            }
                        })
                        if (scope.relationclause.model.edit) {
                            scope.relationclause.model.edit = scope.relationclause.model.edit.toUpperCase();
                        }
                        if (scope.clausetemplate && scope.clausetemplate.Variables && scope.clausetemplate.Variables.length && scope.clausetemplate.Variables[0].Type.toUpperCase() == 'Text'.toUpperCase()) {
                            if (scope.relationclause.model.edit && scope.relationclause.model.edit != null) {
                                scope.$parent.clauseSet.isValid = true;
                            }
                            else {
                                scope.$parent.clauseSet.isValid = false;
                            }

                            if (!scope.relationclause.constraints.visibility.edit) {
                                if (scope.relationclause.datamodel) {
                                    var paramCountValidation = 0;
                                    var totalParamCount = 0;
                                    angular.forEach( scope.clausetemplate.Variables, function (param) {
                                        if (scope.relationclause.datamodel[param.Name] && scope.relationclause.datamodel[param.Name] != null) {
                                            paramCountValidation++;
                                        }
                                        totalParamCount++;
                                    })
                                    if (paramCountValidation == totalParamCount) {
                                    scope.$parent.clauseSet.isValid = true;
                                }
                                else {
                                    scope.$parent.clauseSet.isValid = false;
                                }
                            }
                            else {
                                scope.$parent.clauseSet.isValid = false;
                            }
                                
                            }
                        }
                        else if (scope.clausetemplate && scope.clausetemplate.Variables && scope.clausetemplate.Variables.length && scope.clausetemplate.Variables[0].Type.toUpperCase() == 'relation'.toUpperCase()) {
                            if (scope.relationclause.model.edit && scope.relationclause.model.edit != null) {
                                scope.$parent.clauseSet.isValid = true;
                            }
                            else if (scope.relationclause.constraints.visibility.edit) {
                                scope.$parent.clauseSet.isValid = false;
                            }
                        }
                        if (scope.iseditmode) {
                            scope.$parent.clauseSet.isValid = true;
                        }
                          if (scope.clausesdatamodel) {
                            scope.clausesdatamodel[scope.beneficiaryproperty] = getFinalTemplate();
                        }
                    },
                    init = function () {
                        scope.isValidSum = true;
                        scope.isValidPercent = true;
                        scope.doonrelationchange = doonrelationchange;
                        scope.doOnEditClick = doOnEditClick;
                        scope.addRelation = addRelation;
                        scope.deleteRelation = deleteRelation;
                        scope.validateRelations = validateRelations;
                        scope.relationclause = {
                            constraints: {
                                visibility: {
                                    template: true,
                                    edit: false,
                                    relation: [{ specifyrelation: true, fatherName: false, dob: false }]
                                }
                            },
                            model: {
                                edit: null,
                                relation: [{ datavalue: null }]
                            },
                            relation: {
                                suggestedvalues: []
                            },
                            datamodel: {}
                        };
                        scope.$watch('clausetemplate', clausetemplateWatcher, true);
                        scope.$watch('relationclause.datamodel', relationclauseModelWatcher, true);
                        scope.$watch('relationclause.model.edit', relationclauseModelWatcher, true);
                        scope.$watch('relationclause.constraints.visibility.template', clauseTemplateWithBindedParameter, true);
                        scope.$watch('clauseTemplateWithBindedParameter', clauseTemplateWithBindedParameter, true);
                    };
                init();

            }
        };
    });
    m.directive("beneficiaryClauses", function ($compile) {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    options: '=',
                    validator: '=',
                    iseditmode: '=',
                    modelProperty: '=',
                    datamodel: '='
                },
                template: function (elem, attr) {
                    return '<div class="row">' +
                                '<div ng-repeat="clauseSet in options" class="clearfix">' +
                                    '<hr ng-if="$index > 0" />' +
                                    '<h4 ng-bind="clauseSet.Title" class="text-primary"></h4>' +
                                       '<fieldset ng-disabled="clauseSet.disabled" style="border:0px solid #DBDCDE"  ng-class="(clauseSet.selectedItem.IsEditable &&  clauseSet.SelectedItemCode)? \'col-md-9\' : \'col-md-12\'">' +
                                    '<atom type="\'choice\'" datavalue="clauseSet.SelectedItemCode" suggestedvalues="clauseSet.Items" withlabel="false"' +
                                            'name="clauseSet.Title" valuepath="\'Code\'" displaypath="\'Title\'" changehandler="onClauseChanged" changehandlerparam="clauseSet"></atom></fieldset>' +
                                    '<div ng-if="clauseSet.SelectedItemCode"><clauses-manager iseditmode="iseditmode" clausetemplate="clauseSet.selectedItem" clausesdatamodel="clauseSet" beneficiaryproperty="Value"></clauses-manager></div>' +
                                '</div>' +
                            '</div>';
                },
                controller: function ($scope) {
                    for (var i = 0; i < $scope.options.length; i++) {
                        if ($scope.options[i].Items.length == 1 && $scope.options.length == 1) {
                            $scope.options[i].SelectedItemCode = $scope.options[i].Items[0].Code;
                            $scope.options[i].selectedItem = $scope.options[i].Items[0];
                            $scope.options[i].disabled = true;
                            $scope.options[i].isValid = true;
                        }
                    }
                },
                link: function (scope, element, attrs) {
                    var rootShell = scope.$root.rootShell;
                    var resources = {};
                    angular.extend(resources, rootShell.localization.common, rootShell.localization.errorMessages);
                    for (var i = 0; i < scope.options.length; i++) {
                        scope.options[i].isValid = false;
                        scope.options[i].disabled = false;
                        //if (scope.options[i].Title.indexOf("Optional") > -1) {
                            scope.options[i].Title = scope.options[i].Title = resources.Beneficiary;
                        //}
                        if (scope.iseditmode) {
                            scope.options[i].selectedItem = utils.arrayFirst(scope.options[i].Items, function (item) {
                                return item.Code === scope.options[i].SelectedItemCode;
                            });
}
                        }   
                    var validation = {
                        Life:{},
                        Death: {},
                    }

                    if (scope.modelProperty == "LifeBeneficiaryClause") {
                        validation.Life = scope.options;
                    }
                    else if (scope.modelProperty == "DeathBeneficiaryClause") {
                        validation.Death = scope.options;
                    }
                 
                    Object.defineProperty(scope.validator, 'errorMessage', {
                        get: function () {
                            var result = resources.BeneficiaryMessageEmptyClause;
                            var selectedClause = utils.arrayFirst(scope.options, function (clauseSet) {
                                return clauseSet.SelectedItemCode != null;
                            });
                            var selectedCount = 0;
                            for (var i = 0; i < scope.options.length; i++) {
                                if (scope.options[i].SelectedItemCode) {
                                    selectedCount++;
                                }
                            }
                            if (selectedCount == 0) {
                                for (var i = 0; i < scope.options.length; i++) {
                                    scope.options[i].disabled = false;
                                }
                            }
                            else {
                                for (var i = 0; i < scope.options.length; i++) {
                                    if (scope.options[i].SelectedItemCode) {
                                        scope.options[i].disabled = false;
                                    }
                                    else {
                                        scope.options[i].disabled = true;
                                    }
                                }
                            }
                            for (var i = 0; i < scope.options.length; i++) {
                                if (scope.options[i].Items.length == 1 && scope.options.length == 1) {
                                scope.options[i].disabled = true;
                                }
                            }
                            if (validation.Death.length > 0) {
                                    for (var i = 0; i < validation.Death.length; i++) {
                                        if ((!validation.Death[i].SelectedItemCode || validation.Death[i].SelectedItemCode == null) && validation.Death[i].isValid == false) {
                                            validation.Death[i].isValid = true;
                                    }
                                    if (scope.iseditmode) {
                                        validation.Death[i].isValid = true;
                                    }
                            }
                                var validationDeathCount = 0
                                var isOneDeathSelected = false;
                                    for (var i = 0; i < validation.Death.length; i++) {
                                        if (validation.Death[i].isValid == true) {
                                            validationDeathCount++;
                                    }
                                    if (validation.Death[i].SelectedItemCode && validation.Death[i].SelectedItemCode != null) {
                                        isOneDeathSelected = true;
                                    }
                            }
                            }
                            if (validation.Life.length > 0) {
                                for (var i = 0; i < validation.Life.length; i++) {
                                    if ((!validation.Life[i].SelectedItemCode || validation.Life[i].SelectedItemCode == null) && validation.Life[i].isValid == false) {
                                        validation.Life[i].isValid = true;
                            }
                                    if (scope.iseditmode) {
                                        validation.Life[i].isValid = true;
                            }
                            }
                                var validationLifeCount = 0
                                var isOneLifeSelected = false;
                                for (var i = 0; i < validation.Life.length; i++) {
                                    if (validation.Life[i].isValid == true) {
                                        validationLifeCount++;
                                }
                                    if (validation.Life[i].SelectedItemCode && validation.Life[i].SelectedItemCode != null) {
                                        isOneLifeSelected = true;
                                }
                            }
                            }
                           
                            if (validation.Life.length > 0 && validationLifeCount == validation.Life.length && isOneLifeSelected) {
                                    result = null;
                            }
                            if (validation.Death.length > 0 && validationDeathCount == validation.Death.length && isOneDeathSelected) {
                                        result = null;
                            }

                                for (var i = 0; i < validation.Life.length; i++) {
                                    if (validation.Life[i].SelectedItemCode != null) {
                                        if (validation.Life[i].Value && validation.Life[i].Value.length > 280) {
                                            result = resources.BeneficiaryMessageSizeClause;
                                        }
                                        else {
                                            result = null;
                                        }
                                    }
                                    else if (!validation.Life[i].Value) {
                                        if (validation.Life[i].selectedItem && validation.Life[i].selectedItem.Template && validation.Life[i].isValid) {
                                            result = null;
                                        } else {
                                            result = resources.BeneficiaryMessageEmptyClause;
                                        }
                                       
                                    }

                                }
                                for (var i = 0; i < validation.Death.length; i++) {
                                    if (validation.Death[i].SelectedItemCode != null) {
                                        if (validation.Death[i].Value && validation.Death[i].Value.length > 280) {
                                            if (validation.Death[i].SelectedItemCode) {
                                                result = resources.BeneficiaryMessageSizeClause;
                                            }
                                            else {
                                                result = null;
                                            }
                                        }
                                        else if (!validation.Death[i].Value) {
                                            if (validation.Death[i].selectedItem && validation.Death[i].selectedItem.Template && validation.Death[i].isValid) {
                                                result = null;
                                            } else {
                                                result = resources.BeneficiaryMessageEmptyClause;
                                            }
                                        }
                                    }
                            }
                            return result;
                        }
                    });
                    scope.onClauseChanged = function (code, clauseSet) {
                        for (var i = 0; i < scope.options.length; i++) {
                            if (!scope.options[i].SelectedItemCode || scope.options[i].SelectedItemCode == null) {
                                scope.options[i].isValid = false;
                            }
                        }
                        scope.iseditmode = false;
                        clauseSet.selectedItem = utils.arrayFirst(clauseSet.Items, function (item) {
                            return item.Code === code;
                        });
                    };
                }
            };
    });
    return m;
});