namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;

    /// <summary>
    /// Класс экспорта в файл Excel данных реестра "Проверки по КНМ без взаимодействия"
    /// </summary>
    public class InspectionActionIsolatedExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var inspectionActionIsolatedViewModel = this.Container.Resolve<IViewModel<InspectionActionIsolated>>();
            var inspectionActionIsolatedDomain = this.Container.ResolveDomain<InspectionActionIsolated>();

            using(this.Container.Using(inspectionActionIsolatedViewModel))
            {
                baseParams.Params.Add("isExport", true);
                var result = inspectionActionIsolatedViewModel.List(inspectionActionIsolatedDomain, baseParams);

                return result.Success 
                    ? (IList) result.Data 
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }
        }
    }
}