using System.Linq.Expressions;
using Bars.B4.Application;
using Bars.B4.Logging;

namespace Bars.Gkh.Qa.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Hosting;
    using System.Xml;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Config.Modules;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Ecm7;
    using Bars.B4.Modules.Ecm7.Configuration;
    using Bars.B4.Modules.Ecm7.Providers;
    using Bars.B4.TestToolkit;
    using Bars.B4.TestToolkit.Context;
    using Bars.B4.TestToolkit.Integrational;
    using Bars.B4.Utils;

    using Castle.MicroKernel.Registration;

    using Component = Castle.MicroKernel.Registration.Component;

    public class GkhContext : BaseTestContext
    {
        /// <summary>
        /// Создает новый экземпляр
        /// </summary>
        public GkhContext()
            : base()
        {
        }

        public override bool IsDebug
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Имя БД
        /// </summary>
        public string DatabaseName { get; protected set; }

        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        public string ConnectionString { get; protected set; }

        /// <summary>
        /// Тестируемый регион
        /// </summary>
        public string Region { get; private set; }

        /// <summary>
        /// Начало работы контекста приложения.
        /// При переопределении в потомках необходимо в обязательном порядке вызывать base.Start()
        /// </summary>
        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Метод старта приложения.
        /// Выполняется после того как все обязательные действия выполнены
        /// </summary>
        protected override void StartContext()
        {
            // вызываем базовый метод
            base.StartContext();

            // на текущий момент модули уже загружены в контекст
            // так что мы можем создать структуру БД
            this.RunMigrations();
        }

        /// <summary>
        /// Регистрация дополнительных компонентов непосредственно перед стартом контекста
        /// </summary>
        protected override void OnBeforeAppContextStart()
        {
            // так как мы находимя вне web-окружения, необходимо проинициализировать HostingEnvironment
            if (!HostingEnvironment.IsHosted)
            {
                AppDomain.CurrentDomain.SetData(".appDomain", "B4TestContext");
                AppDomain.CurrentDomain.SetData(".appId", "B4TestContext");
                AppDomain.CurrentDomain.SetData(
                    ".appPath", AppDomain.CurrentDomain.BaseDirectory);
                AppDomain.CurrentDomain.SetData(".appVPath", "/");
                AppDomain.CurrentDomain.SetData(".domainId", "B4TestContext");

                // чтобы вытащить из web.config bindingRedirect
                var assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
                var configDirectory = Path.GetDirectoryName(assemblyPath);
                var configPath = string.Format(@"{0}\Web.config", configDirectory);
                var assemblysToRedirect = this.GetBindingRedirects(configPath);

                // убрал System потому, что он из GACа и не копируется в папку bin
                if (assemblysToRedirect.ContainsKey("System.Core"))
                {
                    assemblysToRedirect.Remove("System.Core");
                }

                if (assemblysToRedirect.ContainsKey("System"))
                {
                    assemblysToRedirect.Remove("System");
                }

                if (assemblysToRedirect.ContainsKey("System.Xml"))
                {
                    assemblysToRedirect.Remove("System.Xml");
                }

                if (assemblysToRedirect.ContainsKey("System.Data"))
                {
                    assemblysToRedirect.Remove("System.Data");
                }
                
                if (assemblysToRedirect.ContainsKey("System.Xml.Linq"))
                {
                    assemblysToRedirect.Remove("System.Xml.Linq");
                }

                if (assemblysToRedirect.ContainsKey("System.Web.Extensions"))
                {
                    assemblysToRedirect.Remove("System.Web.Extensions");
                }

                foreach (var assemblyInfo in assemblysToRedirect)
                {
                    var assembly = Assembly.LoadFrom(string.Format("{0}.dll", assemblyInfo.Key));
                    RedirectAssembly(assembly, new Version(assemblyInfo.Value));
                }

                var environment =
                    AppDomain.CurrentDomain.CreateInstanceAndUnwrap(
                        typeof(HostingEnvironment).Assembly.FullName,
                        typeof(HostingEnvironment).FullName) as HostingEnvironment;

                Container.Register(Component.For<IWindsorInstaller>().ImplementedBy<CoreInstaller>());
                Container.Register(Component.For<IWindsorInstaller>().ImplementedBy<WebInstaller>());
            }
        }

        /// <summary>
        /// Получение конфигурации приложения
        /// </summary>
        /// <returns></returns>
        protected override AppConfig GetTestAppConfiguration()
        {
            var config = base.GetTestAppConfiguration();

            var pathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //var fileDirectory = Path.GetDirectoryName(pathDirectory);
            //var configDirectory = Path.GetDirectoryName(pathDirectory);

            this.SetRegionFromConfig(pathDirectory);

            var b4ConfSection = this.GetB4Config(pathDirectory);

            var appSettings = b4ConfSection.Parameters.Cast<KeyValueSectionParameter>();

            foreach (var setting in appSettings)
            {
                config.AppSettings.Add(setting.Key, setting.Value);
            }

            config.ConnString = b4ConfSection.DbConfig.ConnString; 
            config.DbDialect = DbDialect.PostgreSql;

            var b4ModulesConfig = this.GetB4ModulesConfig(pathDirectory);

            foreach (var module in b4ModulesConfig.Modules.Cast<ModuleConfig>())
            {
                var moduleDictionary = new DynamicDictionary();

                foreach (var parametr in module.Parameters.Cast<KeyValueSectionParameter>())
                {
                    moduleDictionary.SetValue(parametr.Key, parametr.Value);
                }

                config.ModulesConfig.Add(module.Id, moduleDictionary);
            }

            if (config.ModulesConfig.ContainsKey("Bars.B4.Modules.FileSystemStorage"))
            {
                config.ModulesConfig.Remove("Bars.B4.Modules.FileSystemStorage");
            }

            var fileSystemStorageModulesDictionary = new DynamicDictionary();
            fileSystemStorageModulesDictionary.SetValue("FileDirectory", string.Format(@"{0}/.fileStorage", pathDirectory));
            
            config.ModulesConfig.Add("Bars.B4.Modules.FileSystemStorage", fileSystemStorageModulesDictionary);

            //TODO: раскоментить когда будет работать NHibernateChangeLog
            //if (config.ModulesConfig.ContainsKey("Bars.B4.Modules.NHibernateChangeLog"))
            //{
            //    config.ModulesConfig.Remove("Bars.B4.Modules.NHibernateChangeLog");
            //}

            //var nHibernateChangeLogModulesDictionary = new DynamicDictionary();

            //nHibernateChangeLogModulesDictionary.SetValue("Enabled", "true");

            //config.ModulesConfig.Add("Bars.B4.Modules.NHibernateChangeLog", nHibernateChangeLogModulesDictionary);
            loadModules();
            return config;
        }

        private void loadModules()
        {
            //CommonLogger.Instance.Info("Инициализация модулей");
            Console.WriteLine("---Инициализация модулей---");

            var moduleProvider = new DomainModuleProvider();

            // загружаем сборки
            var assemblies = moduleProvider.GetAssemblies().Distinct(x => x.FullName)
                .ToArray();

            //CommonLogger.Instance.Info($"Найдено {assemblies.Length} сборок");
            Console.WriteLine($"Найдено {assemblies.Length} сборок");

            //CommonLogger.Instance.Info("Обработка сборок");
            Console.WriteLine("Обработка сборок");

            foreach (var module in assemblies)
            {
                InitializeModuleAssembly(module);
            }

            // загрузка модулей
            //CommonLogger.Instance.Info("Загрузка модулей");
            Console.WriteLine("Загрузка модулей");

            var list = Container.ResolveAll<IModule>().ToList();
            foreach (var modulesType in list)
            {
                var a = modulesType.DependsOn;
                foreach (var assemblyName in a)
                {
                    var ab = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != assemblyName);
                    ab.ForEach(x => InitializeModuleAssembly(x));
                }
            }
            list.ForEach(x => x.Install());
        }

        /// <summary>
        /// Запуск мигратора для формирования структуры БД
        /// </summary>
        protected virtual void RunMigrations()
        {
            var sortedModules = new List<Assembly>();
            var modules = Container.ResolveAll<IModule>().ToList();
            if (modules.Count == 0)
            {
                return;
            }
            var named = modules.ToDictionary(x => x.Id);
            var sortedIds = modules
                .Select(x => x.Id)
                .TopoSort(x => named.ContainsKey(x) ? (named[x].DependsOn ?? new string[0]) : new string[0]);

            modules = sortedIds
                .Where(named.ContainsKey)
                .Select(x => named[x])
                .ToList();

            var loaded = new Dictionary<string, IModule>();
            foreach (var module in modules)
            {
                var dependenciesLoaded = module.DependsOn == null
                                         || module.DependsOn.All(loaded.ContainsKey);
                if (!dependenciesLoaded) continue;
                loaded[module.Id] = module;
                sortedModules.Add(module.GetType().Assembly);
            }

            var manualMigrations = Container.ResolveAll<IManualMigrationsProvider>().SelectMany(x => x.GetAssemblies())
                .Distinct(x => x.FullName).Where(x => !sortedModules.Contains(x));

            EnumerableExtension.ForEach(manualMigrations, sortedModules.Add);

            var logFileName = "migration_{0:dd.MM.yyyy_HH.mm.ss}.log".FormatUsing(DateTime.Now);
            var fileDirectory = this.Configuration.GetModuleConfig("Bars.B4.Modules.FileSystemStorage").GetValue("FileDirectory").ToString();
            var logFilePath = Path.Combine(fileDirectory, logFileName);

            using (var logStream = new FileStream(logFilePath, FileMode.Create))
            {
                var logWriter = new StreamWriter(logStream);
                Console.SetOut(logWriter);

                try
                {
                    var config = new MigratorConfiguration
                                     {
                                         ProviderType =
                                             ProviderFactory.GetProviderType(
                                                 "Bars.B4.Modules.Ecm7.Providers.PostgreSQL.PostgreSQLTransformationProvider, Bars.B4.Modules.Ecm7.Providers.PostgreSQL"),
                                         ConnectionString =
                                             this.Configuration.ConnString + "MaxPoolSize=1024;",
                                         Assemblies = sortedModules.ToArray()
                                     };

                    var migrator = new Migrator(config);
                    migrator.MigrateToLatest();
                }
                catch (Exception exc)
                {
                    Console.WriteLine(
                        string.Format(
                            "Error: {0}\r\nStack trace: {1}\r\n Inner: {2}",
                            exc.Message,
                            exc.StackTrace,
                            exc.InnerException != null ? exc.InnerException.Message : string.Empty));

                    throw;
                }
                finally
                {
                    logWriter.Flush();
                    logWriter.Close();
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
                }
            }
        }

        protected override void RegisterModuleProvider()
        {
            base.RegisterModuleProvider();
            Component
                .For<IModuleProvider>()
                .ImplementedBy<DomainModuleProvider>()
                .RegisterIn(this.Container);
        }

        private SectionHandler GetB4Config(string path)
        {
            var b4ConfigMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = string.Format(@"{0}\b4.config", path)
            };

            Configuration b4Config = ConfigurationManager.OpenMappedExeConfiguration(b4ConfigMap, ConfigurationUserLevel.None);
            var b4ConfSection = (SectionHandler)b4Config.Sections["b4config"];
           
            return b4ConfSection;
        }

        private ModulesSectionHandler GetB4ModulesConfig(string path)
        {
            var configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = string.Format(@"{0}\b4.config", path)
            };

            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            return (ModulesSectionHandler)config.Sections["modules"];
        }

        private void SetRegionFromConfig(string configDirectory)
        {
            var buildInfoLines = System.IO.File.ReadLines(string.Format(@"{0}\build-info.json", configDirectory));

            foreach (var line in buildInfoLines)
            {
                if (line.Contains("\"region\":"))
                {
                    this.Region = line.Split(':').Last().Trim();
                }
            }
        }

        ///Создаёт событие bindingRedirect
        public static void RedirectAssembly(Assembly assembly, Version targetVersion = null)
        {
            var assemblyShortName = assembly.GetName().Name;
            var publickKey = GetPublicKeyTokenFromAssembly(assembly);

            RedirectAssembly(assemblyShortName, publickKey, targetVersion);
        }

        ///Создаёт событие bindingRedirect
        public static void RedirectAssembly(string shortName, string publicKeyToken,  Version targetVersion = null)
        {
            ResolveEventHandler handler = null;

            handler = (sender, args) =>
            {
                // Use latest strong name & version when trying to load SDK assemblies
                var requestedAssembly = new AssemblyName(args.Name);
                if (requestedAssembly.Name != shortName)
                {
                    return null;
                }

                Debug.WriteLine("Redirecting assembly load of " + args.Name
                              + ",\tloaded by " + (args.RequestingAssembly == null ? "(unknown)" : args.RequestingAssembly.FullName));

                requestedAssembly.Version = targetVersion;
                requestedAssembly.SetPublicKeyToken(new AssemblyName("x, PublicKeyToken=" + publicKeyToken).GetPublicKeyToken());
                requestedAssembly.CultureInfo = CultureInfo.InvariantCulture;

                AppDomain.CurrentDomain.AssemblyResolve -= handler;

                return Assembly.Load(requestedAssembly);
            };
            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }

        private static string GetPublicKeyTokenFromAssembly(Assembly assembly)
        {
            var bytes = assembly.GetName().GetPublicKeyToken();
            if (bytes == null || bytes.Length == 0)
            {
                return "None";
            }

            var publicKeyToken = string.Empty;
            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                publicKeyToken += string.Format("{0:x2}", bytes[i]);
            }

            return publicKeyToken;
        }

        private IDictionary<string, string> GetBindingRedirects(string path)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(path);
            }
            catch (FileNotFoundException)
            {
                return new Dictionary<string, string>();
            }

            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);

            manager.AddNamespace("bindings", "urn:schemas-microsoft-com:asm.v1");

            XmlNode root = doc.DocumentElement;

            var nodes = root.SelectNodes("//bindings:dependentAssembly", manager);

            if (nodes == null)
            {
                throw new Exception("Invalid Configuration File");
            }

            var dict = new Dictionary<string, string>();
            foreach (var node in nodes)
            {
                var xmlNode = node as XmlNode;

                if (xmlNode != null)
                {
                    var element = xmlNode["assemblyIdentity"];
                    if (element != null)
                    {
                        var assemblyName = element.GetAttribute("name");

                        var xmlElement = xmlNode["bindingRedirect"];
                        if (xmlElement != null)
                        {
                            var version = xmlElement.GetAttribute("newVersion");

                            dict.Add(assemblyName, version);
                        }
                    }
                }
            }

            return dict;
        }
    }
}
