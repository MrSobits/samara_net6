namespace Bars.GkhGji.Regions.BaseChelyabinsk.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppealCitsBaseChelyabinskDataExport : BaseDataExportService, IAppealCitsDataExport
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizensBaseChelyabinsk>>();
            using (this.Container.Using(service))
            {
                int totalCount;
                return service.GetViewModelList(baseParams, out totalCount, false);
            }
        }
    }
}
