namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// Экспорт в Excel файл для <see cref="MotivatedPresentation"/>
    /// </summary>
    public class MotivatedPresentationAppealCitsExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var mpService = this.Container.Resolve<IMotivatedPresentationAppealCitsService>();
            using (this.Container.Using(mpService))
            {
                baseParams.Params.Add("isExport", true);
                var result = mpService.ListForRegistry(baseParams);
                
                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }
        }
    }
}