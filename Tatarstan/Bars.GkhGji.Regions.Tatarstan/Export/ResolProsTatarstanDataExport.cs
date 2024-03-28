namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;

    public class ResolProsTatarstanDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var resolProsAndResolution = this.Container.Resolve<IResolProsAndResolutionService>();
            var resolProsesService = this.Container.Resolve<IDomainService<ResolPros>>();

            try
            {
                var list = resolProsAndResolution.GetList(baseParams, resolProsesService, true, false);

                return (IList) list;
            }
            finally
            {
                this.Container.Release(resolProsAndResolution);
                this.Container.Release(resolProsesService);
            }
        }
    }
}