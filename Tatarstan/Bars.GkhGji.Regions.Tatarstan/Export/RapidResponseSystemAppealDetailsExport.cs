namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    /// <summary>
    /// Экспорт реестра СОПР
    /// </summary>
    public class RapidResponseSystemAppealDetailsExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var rapidResponseSystemAppealDetailsViewModel = this.Container.Resolve<IViewModel<RapidResponseSystemAppealDetails>>();
            var rapidResponseSystemAppealDetailsDomain = this.Container.ResolveDomain<RapidResponseSystemAppealDetails>();

            using (this.Container.Using(rapidResponseSystemAppealDetailsViewModel, rapidResponseSystemAppealDetailsDomain))
            {
                var result = rapidResponseSystemAppealDetailsViewModel.List(rapidResponseSystemAppealDetailsDomain, baseParams);
                return result.Success 
                    ? (IList) result.Data 
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }
        }
    }
}