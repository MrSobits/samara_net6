namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated;

    /// <summary>
    /// Класс экспорта в файл Excel данных реестра "КНМ без взаимодействия с контролируемыми лицами"
    /// </summary>
    public class TaskActionIsolatedExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var taskActionIsolatedViewModel = this.Container.Resolve<IViewModel<TaskActionIsolated>>();
            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();

            using (this.Container.Using(taskActionIsolatedViewModel, taskActionIsolatedDomain))
            {
                baseParams.Params.Add("isExport", true);
                var result = (taskActionIsolatedViewModel as TaskActionIsolatedViewModel).ListForDocumentRegistry(taskActionIsolatedDomain, baseParams);

                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }

        }
    }
}
