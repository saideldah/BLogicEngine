using BSynchro.Web.Optimization.ClientModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIC.Portal.Modules
{
    public class ModuleLoader : BSynchro.Web.Optimization.ClientModules.IModuleLoaderCatalog
    {
        public System.Collections.Generic.IEnumerable<BSynchro.Web.Optimization.ClientModules.ModuleLoadConfig> GetLoaderConfiguration()
        {
            List<BSynchro.Web.Optimization.ClientModules.ModuleLoadConfig> modules = new List<BSynchro.Web.Optimization.ClientModules.ModuleLoadConfig>();

            modules.Add(new BSynchro.Web.Optimization.ClientModules.ModuleLoadConfig("Angular"));
            modules.Add(new BSynchro.Web.Optimization.ClientModules.ModuleLoadConfig("SiteModule"));
            modules.Add(new BSynchro.Web.Optimization.ClientModules.ModuleLoadConfig("JicQuoteModule"));
            return modules;
        }
    }
    internal class JicQuoteModule : ClientModuleBase
    {

        protected override string[] GetModuleDependencies()
        {
            return new[]
            {
                "RequireJS",
                "Jquery",
                "Angular",
                "AmsModule"
            };
        }

        protected override ModuleFile[] GetModuleFiles()
        {
            List<string> fileNames = new List<string>()
            {
                "jicQuoteModule",
                "WizardControllerBase",
                "NeedDetailsController",
                "jicServices",
                "jicWizardHelper"
            };
            List<ModuleFile> moduleFiles = new List<ModuleFile>();
            foreach (var f in fileNames)
            {
                moduleFiles.Add(new ModuleFile(f, string.Format("~/Content/js/services/{0}.js", f)));
            }
            return moduleFiles.ToArray();
        }

        protected override string GetModuleName()
        {
            return "JicQuoteModule";
        }

        protected override ModuleShim[] GetModuleShims()
        {
            return new ModuleShim[0];
        }

        protected override ModuleType GetModuleType()
        {
            return ModuleType.AmdModule;
        }
    }
}