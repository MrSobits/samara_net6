namespace Bars.Gkh.Diagnostic.DomainServices.Impl
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Migrations;
    using Bars.B4.Utils;
    using Bars.Gkh.Diagnostic.Entities;
    using Bars.Gkh.Diagnostic.Enums;

    using Castle.Windsor;

    public class MigrationsDiagnostic : IDiagnostic
    {
        public IWindsorContainer Container { get; set; }

        public string Name
        {
            get
            {
                return "Диагностика миграций";
            }
        }

        public string Id
        {
            get
            {
                return "MigrationDiagnosticService";
            }
        }

        public void Run(DiagnosticResult diagnostic)
        {
            string exceptionString = string.Empty;

            var strBuilder = new StringBuilder();

            var loadedAssembles = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).ToArray();

            var modules = loadedAssembles.Where(x => x.GetTypes().Any(t => t.Is<IModule>())).Select(x => x.GetName());

            var migrationInfo =
                Container.Resolve<IMigrationManager>()
                    .ListMigration();
            try
            {
                foreach (var module in modules)
                {


                    var currentModule = migrationInfo.FirstOrDefault(x => x.ModuleId == module.Name);

                    if (currentModule == null)
                    {
                        continue;
                    }

                    var availableMigrations = Container.Resolve<IMigrationManager>()
                        .ModuleMigrationsList(module.Name, null);

                    var sessionProvider = Container.Resolve<ISessionProvider>();

                    try
                    {
                        var currentSession = sessionProvider.GetCurrentSession();

                        var completedMigrations =
                            currentSession.CreateSQLQuery(
                                string.Format(
                                    "select version from schemainfo where schemainfo.module_id = '{0}'",
                                    module.Name)).List<string>();

                        var unappliedMigrations = availableMigrations.Except(completedMigrations).ToArray();
                        var removedMigrations = completedMigrations.Except(availableMigrations).ToArray();

                        if (unappliedMigrations.Any())
                        {
                            strBuilder.Append(
                                string.Format("Не проведены миграции: {0}.", string.Join(", ", unappliedMigrations)));
                        }

                        if (removedMigrations.Any())
                        {
                            strBuilder.Append(
                                string.Format("Удалены миграции: {0}.", string.Join(", ", removedMigrations)));
                        }
                    }
                    finally
                    {
                        Container.Release(sessionProvider);
                        Container.Release(availableMigrations);
                    }

                    if (strBuilder.Length > 0)
                    {
                        exceptionString = string.Format(
                            "{0}</br> для модуля {1} найдены следующие ошибки {2};",
                            exceptionString,
                            module.Name,
                            strBuilder);
                    }

                    strBuilder.Clear();
                }
            }
            finally
            {
                Container.Release(migrationInfo);
            }

            if (exceptionString != string.Empty)
            {
                diagnostic.State = DiagnosticResultState.Fail;

                diagnostic.AddMessage(exceptionString);
            }

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
    }
}
