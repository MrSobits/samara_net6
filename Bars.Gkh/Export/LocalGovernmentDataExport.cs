namespace Bars.Gkh.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class LocalGovernmentDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<ILocalGovernmentService>();

            try
            {
                int totalCount;
                return service.GetViewModelList(baseParams, out totalCount, false);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}