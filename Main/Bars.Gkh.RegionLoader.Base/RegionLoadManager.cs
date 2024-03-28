namespace Bars.Gkh.RegionLoader.Base
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4.Utils.SolutionParser;
    using Bars.Gkh.RegionLoader.Base.Properties;

    using Bars.B4.Utils;
    using Microsoft.Build.Construction;
    
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Module = Bars.Gkh.RegionLoader.Base.JsonProxyClasses.Module;

    public class RegionLoadManager
    {
        private const string Xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";

        private const string AppProjectName = "Bars.Gkh.App";
        
        protected string MainSolutionPath;

        protected string RootPath;
        
        public string Region { get; private set; }
        
        private List<string> baseModules;

        private Dictionary<string, List<string>> regionModules;

        private JObject buildInfo;

        private string buildInfoPath;

        private SolutionProject appSolutionProject;

        public IList<string> Regions
        {
            get
            {
                return this.regionModules.Select(x => x.Key).ToList();
            }
        }

        public Solution Solution { get; private set; }

        public void InitializeLoader()
        {
            CheckSolutionPath();

            LoadConfigModules();

            LoadSolution();

            ProcessApp();
        }

        public void SetRegion(string region)
        {
            var dictRegionModulesCaseInsens = new Dictionary<string, List<string>>(this.regionModules, StringComparer.OrdinalIgnoreCase);
            if (!dictRegionModulesCaseInsens.ContainsKey(region))
            {
                return;
            }

            // создаем при необходимости targets файлы
            var targetsPath = Path.Combine(this.RootPath, "Main\\RegionModules.targets");
            if (!File.Exists(targetsPath))
            {
                if (!File.Exists(targetsPath + ".default"))
                {
                    File.WriteAllBytes(targetsPath + ".default", Resources.RegionModules_targets);
                }

                File.Copy(targetsPath + ".default", targetsPath);
            }

            var import = ProjectRootElement.Open(targetsPath);
                
            // Удаляем группы с ранее добавленными региональными модулями
            import.ItemGroups.ForEach(x => import.RemoveChild(x));

            // Получаем региональные модули и добавляем их в группу
            var currentRegionModules = dictRegionModulesCaseInsens[region];
                
            if(currentRegionModules.Any())
            {
                var itemGroup = import.AddItemGroup();
                
                foreach (var module in currentRegionModules)
                {
                    var moduleProject = this.Solution.Projects.First(x =>
                        x.Name.Equals(module, StringComparison.InvariantCultureIgnoreCase));

                    itemGroup.AddItem(
                        "ProjectReference",
                        GetRelativePath(this.appSolutionProject.Directory, moduleProject.AbsolutePath));
                }
            }

            import.Save();
            ClearBin(this.appSolutionProject.Directory);
            
            WriteConfigModules(currentRegionModules);
            this.buildInfo["region"] = region;
            File.WriteAllText(this.buildInfoPath, this.buildInfo.ToString(), Encoding.UTF8);

            Region = region;

            LoadSolution();
        }

        private void CheckConfigModules()
        {
            var notIncludedProjects = new List<string>();
            notIncludedProjects.AddRange(this.baseModules.Except(this.Solution.Projects.Select(x => x.Name), StringComparer.OrdinalIgnoreCase));
            notIncludedProjects.AddRange(this.regionModules.SelectMany(x => x.Value).Distinct().Except(this.Solution.Projects.Select(x => x.Name), StringComparer.OrdinalIgnoreCase));
            if (notIncludedProjects.Count <= 0)
            {
                return;
            }

            throw new Exception(string.Format("Следующие проекты не добавлены в решение. Для корректной работы конфигуратора необходимо добавить их: {0}", string.Join(", ", notIncludedProjects)));
        }

        private void CheckSolutionPath()
        {
            RootPath = GetSolutionFolder(Assembly.GetEntryAssembly().Location);
            MainSolutionPath = Path.Combine(RootPath, "Bars.Gkh.Main.sln");
            if (File.Exists(MainSolutionPath))
            {
                return;
            }

            throw new Exception("Файл решения " + MainSolutionPath + " не найден.");
        }

        private void ClearBin(string appFolder)
        {
            DeleteFolder(appFolder, "bin");
            DeleteFolder(appFolder, "obj");
        }

        private void DeleteFolder(string appFolder, string subFolder)
        {
            var directory = Path.Combine(appFolder, subFolder);

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        private Module GetConfigModules()
        {
            using (var streamReader = new StreamReader(GetConfigModulesPath(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<Module>(streamReader.ReadToEnd());
            }
        }

        private string GetConfigModulesPath()
        {
            var solutionDir = Path.GetDirectoryName(MainSolutionPath);
            if (string.IsNullOrEmpty(solutionDir))
            {
                throw new Exception("Директория решения не найдена!");
            }

            var configModules = Path.Combine(solutionDir, "Main\\modules.json");
            if (!File.Exists(configModules))
            {
                throw new Exception("Файл modules.json не найден!");
            }

            return configModules;
        }

        private string GetRelativePath(string @base, string target)
        {
            var targetUri = new Uri(target);
            if (!@base.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                @base += Path.DirectorySeparatorChar;
            }

            var baseUri = new Uri(@base);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(targetUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        private string GetSolutionFolder(string path)
        {
            return path.Substring(0, path.IndexOf("\\Main\\", StringComparison.InvariantCultureIgnoreCase));
        }

        private void LoadConfigModules()
        {
            var configModules = GetConfigModules();
            this.baseModules = configModules.BaseModules.ToList();
            this.regionModules = configModules.RegionModules.GroupBy(x => x.RegionName).ToDictionary(x => x.Key, y => y.SelectMany(x => x.Modules).ToList());
        }

        private void LoadSolution()
        {
            Solution = Solution.Load(MainSolutionPath);
        }

        private void ProcessApp()
        {
            CheckConfigModules();

            this.appSolutionProject = this.Solution.Projects.First(x => x.Name.Equals(RegionLoadManager.AppProjectName, StringComparison.OrdinalIgnoreCase));

            this.buildInfoPath = Path.Combine(this.appSolutionProject.Directory, "build-info.user.json");
            if (!File.Exists(this.buildInfoPath))
            {
                this.buildInfoPath = Path.Combine(this.appSolutionProject.Directory, "build-info.json");
                if (!File.Exists(this.buildInfoPath))
                {
                    this.buildInfo = new JObject();
                    return;
                }
            }

            using (var streamReader = new StreamReader(this.buildInfoPath, Encoding.UTF8))
            {
                this.buildInfo = JObject.Parse(streamReader.ReadToEnd());
                Region = this.buildInfo["region"].ToString();
            }
        }

        private void WriteConfigModules(IEnumerable<string> modules)
        {
            var modulePath = Path.Combine(this.appSolutionProject.Directory, "regionModules.json");

            using (var streamWriter = new StreamWriter(modulePath, false, Encoding.UTF8))
            {
                streamWriter.Write(JsonConvert.SerializeObject(modules, Formatting.Indented));
            }
        }
    }
}