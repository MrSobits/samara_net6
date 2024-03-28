using Bars.GkhCr.DomainService;

namespace Bars.GkhCr.Export
{
    using System.Collections;

    using B4;
    using B4.Modules.DataExport.Domain;

    public class CompetitionDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<ICompetitionService>();

            try
            {
                var totalCount = 0;
                return service.GetList(baseParams, true, ref totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}