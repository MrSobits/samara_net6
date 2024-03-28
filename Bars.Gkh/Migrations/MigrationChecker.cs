namespace Bars.Gkh.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.Migrations;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Событие для проверки актуальности миграций при старте системы
    /// </summary>
    public class MigrationChecker : EventHandlerBase<AppStartEventArgs>
    {
        /// <summary>
        /// IoC
        /// </summary>
        private static IWindsorContainer Container => ApplicationContext.Current.Container;

        /// <inheritdoc />
        public override void OnEvent(AppStartEventArgs args)
        {
            if (IsNeedMigration())
            {
                // если под отладчиком, встаем здесь, чтоб не падал сервер расчетов  
                try
                { 
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Не все миграции проведены");
                    Console.ResetColor();
                    Console.ReadKey();
                }
                catch { }
                Debugger.Break();
                MigrationChecker.Container.Resolve<ILogger>().LogError("Не все миграции проведены");
                Environment.Exit(13); // ERROR_INVALID_DATA
            }
        }

        /// <summary>
        /// Возвращает результат необходимости проведения миграции
        /// </summary>
        public static bool IsNeedMigration()
        {
            var migrationManager = MigrationChecker.Container.Resolve<IMigrationManager>();
            try
            {
                return false;
                return migrationManager.ListMigration()
                    .Any(info =>
                        info.DatabaseStateInfos.Any(dbInfo =>
                            !new HashSet<string>(dbInfo.CurrentVersions)
                                .SetEquals(new HashSet<string>(dbInfo.NewVersions))));
            }
            finally
            {
                MigrationChecker.Container.Release(migrationManager);
            }
        }
    }
}