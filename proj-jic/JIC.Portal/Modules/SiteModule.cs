using System;
using BSynchro.Web.Optimization.ClientModules;
using BSynchro.Web.Optimization.Configuration.Modules;

namespace JIC.Portal.Modules
{
    internal class SiteModule : ClientModuleBase
    {
        protected override string GetModuleName()
        {
            return "SiteModule";
        }

        protected override ModuleType GetModuleType()
        {
            return ModuleType.AmdModule;
        }

        protected override ModuleShim[] GetModuleShims()
        {
            return new ModuleShim[0];
        }

        protected override ModuleFile[] GetModuleFiles()
        {
            return new[]
            {
               new ModuleFile("accountDirectives", "~/content/account/accountDirectives.js"),
                new ModuleFile("accountController", "~/content/account/accountController.js"),
                new ModuleFile("companySpecificConfiguration", "~/content/js/companySpecificConfiguration.js"),
                new ModuleFile("siteModule", "~/content/js/siteModule.js"),
                new ModuleFile("amsConfiguration", "~/content/js/amsConfiguration.js"),
                new ModuleFile("underscore", "~/content/js/lib/underscore-min.js"),
                new ModuleFile("pendingProposalListDirective", "~/content/js/directives/pendingProposalListDirective.js"),
                new ModuleFile("beneficiaryClausesDirective", "~/content/js/directives/beneficiaryClausesDirective.js"),
                new ModuleFile("jicUserFilterDirective", "~/content/js/directives/jicUserFilterDirective.js"),
                new ModuleFile("jicFilterControllerBase", "~/content/js/directives/jicFilterControllerBase.js"),
                new ModuleFile("jicBusinessFilterDirective", "~/content/js/directives/jicBusinessFilterDirective.js")
            };
        }

        protected override string[] GetModuleDependencies()
        {
            return new[]
            {
                BuiltInModuleNames.RequireJS.ToString(),
                BuiltInModuleNames.Jquery.ToString(),
                BuiltInModuleNames.Angular.ToString(),
                BuiltInModuleNames.AmsModule.ToString(),
                BuiltInModuleNames.DataChart.ToString()
            };
        }
    }
}
