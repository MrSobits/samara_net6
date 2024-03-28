namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using B4;
    using B4.Modules.DataExport.Domain;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Выгрузка для <see cref="Presentation"/>
    /// </summary>
    public class PresentationDataExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<IViewModel<Presentation>>();
            var domainService = this.Container.ResolveDomain<Presentation>();

            using (this.Container.Using(viewModel, domainService))
            {
                baseParams.Params.Add("isExport", true);
                var result = viewModel.List(domainService, baseParams);
                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }
        }
    }
}