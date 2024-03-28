namespace Bars.GkhGji.Export
{
    using System.Collections;
    
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;

    public class ActSurveyDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IActSurveyService>();
            try
            {
                var totalCount = 0;
                return service.ListView(baseParams, true, ref totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}