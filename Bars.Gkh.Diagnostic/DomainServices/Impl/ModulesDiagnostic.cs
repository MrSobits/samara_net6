namespace Bars.Gkh.Diagnostic.DomainServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Diagnostic.Entities;
    using Bars.Gkh.Diagnostic.Enums;
    using Bars.Gkh.Diagnostic.Helpers;

    using Castle.Windsor;

    using Newtonsoft.Json;

    public class ModulesDiagnostic : IDiagnostic
    {
        public IWindsorContainer Container { get; set; }

        public string Name
        {
            get
            {
                return "Диагностика загруженных библиотек";
            }
        }

        public string Id
        {
            get
            {
                return "ModulesDiagnostic";
            }
        }

        public void Run(DiagnosticResult diagnostic)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var modules = new List<string>();

            using (StreamReader r = new StreamReader(string.Format("{0}//regionModules.json", baseDirectory)))
            {
                string json = r.ReadToEnd();
                modules = JsonConvert.DeserializeObject<List<string>>(json);
            }

            var missingModules = new List<string>();

            foreach (var module in modules)
            {
                var exist = CheckModuleExist(module);

                if (exist)
                {
                    missingModules.Add(module);
                }
            }

            if (missingModules.Any())
            {
                var missingModulesString = string.Join(", ", missingModules);

                diagnostic.State = DiagnosticResultState.Fail;

                diagnostic.AddMessage(string.Format("В домене отсутствуют следующие модули {0}", missingModulesString));
            }

            CheckModules(Container, diagnostic);

            if (diagnostic.State != DiagnosticResultState.Fail)
            {
                diagnostic.State = DiagnosticResultState.Success;
                diagnostic.AddMessage("Успешно");
            }

            var dsDiagnosticResult = Container.Resolve<IDomainService<DiagnosticResult>>();

            try
            {
                dsDiagnosticResult.Update(diagnostic);
            }
            finally
            {
                Container.Release(dsDiagnosticResult);
            }
        }

        private void CheckModules(IWindsorContainer container, DiagnosticResult diagnostic)
        {
            var loadedAssembles = AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .Where(x => !x.IsDynamic)
                    .ToArray();

            var modules = loadedAssembles.Where(x => x.GetTypes().Any(t => t.Is<IModule>()));

            var loadedAssembliesNames = loadedAssembles.Select(x => x.GetName());

            var exceptions = new List<ModuleException>();

            foreach (var module in modules)
            {
                var references = module.GetReferencedAssemblies();

                foreach (var reference in references)
                {
                    var test = loadedAssembliesNames.Where(x => x.Name == reference.Name);

                    if (test.Count() > 1)
                    {
                        var e = 1;
                    }

                    var loadedAssembly = loadedAssembliesNames.FirstOrDefault(x => x.Name == reference.Name);

                    if (loadedAssembly == null)
                    {
                        exceptions.Add(
                            new ModuleException(
                                module,
                                string.Format(
                                    "отсутствует сборка, на которую ссылается модуль, {0}",
                                    reference.FullName)));
                    }
                    else if (loadedAssembly.Version != reference.Version)
                    {
                        exceptions.Add(
                            new ModuleException(
                                module,
                                string.Format(
                                    "сборка {0} имеет версию {1}  вместо ожидаемой {2}",
                                    reference.Name, loadedAssembly.Version, reference.Version)));
                    }
                }
            }

            if (exceptions.Any())
            {
                var GroupedExceptions = exceptions.GroupBy(x => x.Module, y => y.Message);

                var exceptionString = String.Join(
                    "</br>",
                    GroupedExceptions.Select(
                        x => string.Format("для модуля {0} произошли ошибки: {1}", x.Key.FullName, string.Join(", ", x))));

                diagnostic.State = DiagnosticResultState.Fail;

                diagnostic.AddMessage(exceptionString);
            }
        }

        private bool CheckModuleExist(string moduleName)
        {
            var loadedAssembles = AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .Where(x => !x.IsDynamic)
                    .ToArray();

            var modules = loadedAssembles.Where(x => x.GetTypes().Any(t => t.Is<IModule>()));

            var exist = modules.Any(x => x.FullName == moduleName);

            return exist;
        }

        private class ModuleException
        {
            public ModuleException(Assembly module, string message)
            {
                this.Module = module;

                this.Message = message;
            }

            public string Message { get; private set; }

            public Assembly Module { get; private set; }
        }
    }
}
