namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionPreventiveAction;

    /// <summary>
    /// Экспортер данных реестра "Проверки по профилактическим мероприятиям" в Excel файл
    /// </summary>
    public class InspectionPreventiveActionExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var inspectionPreventiveActionViewModel = this.Container.Resolve<IViewModel<InspectionPreventiveAction>>();
            var inspectionPreventiveActionDomain = this.Container.ResolveDomain<InspectionPreventiveAction>();

            using(this.Container.Using(inspectionPreventiveActionViewModel, inspectionPreventiveActionDomain))
            {
                baseParams.Params.Add("isExport", true);
                var result = inspectionPreventiveActionViewModel.List(inspectionPreventiveActionDomain, baseParams);

                return result.Success 
                    ? (IList) result.Data 
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }

        }
    }
}