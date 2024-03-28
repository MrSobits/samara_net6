namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction;

    /// <summary>
    /// Экспорт в Excel файл для <see cref="PreventiveActionExport"/>
    /// </summary>
    public class PreventiveActionExport : BaseDataExportService
    {
        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<IViewModel<PreventiveAction>>();
            var domainService = this.Container.ResolveDomain<PreventiveAction>();

            using (this.Container.Using(viewModel, domainService))
            {
                baseParams.Params.Add("isExport", true);
                var result = (viewModel as PreventiveActionViewModel).ListForDocumentRegistry(domainService, baseParams);

                return result.Success
                    ? (IList)result.Data
                    : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
            }

        }
    }
}