namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;

    /// <summary>
    /// Класс экспорта в файл Excel данных реестра "Лист визита"
    /// </summary>
    public class VisitSheetDataExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var visitSheetService = this.Container.Resolve<IVisitSheetService>();

            using (this.Container.Using(visitSheetService))
            {
                baseParams.Params.Add("isExport", true);
                var result = visitSheetService.ListForRegistry(baseParams);
                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }
        }
    }
}