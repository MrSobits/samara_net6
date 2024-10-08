﻿namespace Bars.Gkh.Gis.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    using Dapper;

    /// <summary>
    /// Открытие прав ограничений на поле "Выгрузка показаний ПУ" для ролей: УО (МО), МО, ЕРЦ, РСО
    /// </summary>
    public class ExportDeviceReadingPermissionsAddAction : BaseExecutionAction
    {
        /// <summary>
        /// Код
        /// </summary>
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Открытие прав ограничений на поле \"Выгрузка показаний ПУ\" для ролей: УО (МО), МО, ЕРЦ, РСО";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Открытие прав на \"Выгрузка показаний ПУ\"";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => ExportDeviceReadingPermissionsAddAction.Execute;

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
                                and role_id in (select id from b4_role where trim(name) in ('УО (МО)', 'МО', 'ЕРЦ', 'РСО'));

                        insert into b4_role_permission(permission_id, role_id)
                            select 'Gis.ImportExportData.UnloadCounterValues', id from b4_role where trim(name) = 'УО (МО)'
                            union
                            select 'Gis.ImportExportData.UnloadCounterValues', id from b4_role where trim(name) = 'МО'
                            union
                            select 'Gis.ImportExportData.UnloadCounterValues', id from b4_role where trim(name) = 'ЕРЦ'
                            union
                            select 'Gis.ImportExportData.UnloadCounterValues', id from b4_role where trim(name) = 'РСО';",
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