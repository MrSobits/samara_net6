namespace Bars.GkhGji.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class AppealCitsDataExport : BaseDataExportService, IAppealCitsDataExport
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizens>>();

            using (this.Container.Using(service))
            {
                int totalCount;

                return service.GetViewModelList(baseParams, out totalCount, false);
            }
        }
    }
}
