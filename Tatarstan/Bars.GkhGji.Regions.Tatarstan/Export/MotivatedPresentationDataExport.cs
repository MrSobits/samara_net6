namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;

    /// <summary>
    /// Класс экспорта в файл Excel данных реестра "Мотивированное представление"
    /// </summary>
    public class MotivatedPresentationDataExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var motivatedPresentationService = this.Container.Resolve<IMotivatedPresentationService>();

            using (this.Container.Using(motivatedPresentationService))
            {
                baseParams.Params.Add("isExport", true);
                var result = motivatedPresentationService.ListForRegistry(baseParams);
                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }
        }
    }
}