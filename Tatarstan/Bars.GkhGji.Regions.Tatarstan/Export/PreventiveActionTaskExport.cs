using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Modules.DataExport.Domain;
using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;
using System;
using System.Collections;

namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    /// <summary>
    /// Экспорт в Excel файл для <see cref="PreventiveActionTask"/>
    /// </summary>
    public class PreventiveActionTaskExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var actActionIsolatedService = Container.Resolve<IPreventiveActionTaskService>();

            using (Container.Using(actActionIsolatedService))
            {
                baseParams.Params.Add("isExport", true);
                var result = actActionIsolatedService.List(baseParams);

                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }

        }
    }
}