namespace Bars.Gkh.Gis.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    using Dapper;

    /// <summary>
    /// Закрытие прав ограничений на поле "Выгрузка показаний ПУ" для ролей: УО (МО), МО, ЕРЦ, РСО
    /// </summary>
    public class ExportDeviceReadingPermissionsRemoveAction : BaseExecutionAction
    {
        /// <summary>
        /// Код
        /// </summary>
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Закрытие прав ограничений на поле \"Выгрузка показаний ПУ\" для ролей: УО (МО), МО, ЕРЦ, РСО";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Закрытие прав на \"Выгрузка показаний ПУ\"";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => ExportDeviceReadingPermissionsRemoveAction.Execute;

        private static BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;

            ISessionProvider sessions = container.Resolve<ISessionProvider>();

            using (var connection = sessions.OpenStatelessSession().Connection)
            {
                using (var tr = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(
                            @"
                        delete
                            from b4_role_permission
                            where permission_id = 'Gis.ImportExportData.UnloadCounterValues'
                                and role_id in (select id from b4_role where trim(name) in ('УО (МО)', 'МО', 'ЕРЦ', 'РСО'));",
                            transaction: tr,
                            commandTimeout: 10000);
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }

                    tr.Commit();
                }
            }

            return new BaseDataResult();
        }
    }
}