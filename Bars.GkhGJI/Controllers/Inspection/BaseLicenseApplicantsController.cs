namespace Bars.GkhGji.Controllers
{

    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService;
    using Entities;

    public class BaseLicenseApplicantsController : B4.Alt.DataController<BaseLicenseApplicants>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BaseLicenseApplicantsDataExport");

            try
            {
                if (export != null)
                {
                    return export.ExportData(baseParams);
                }

                return null;
            }
            finally
            {
                Container.Release(export);
            }

        }

        public virtual ActionResult ListByLicenseReq(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseLicenseApplicantsService>();
            try
            {
                var result = (ListDataResult)service.ListByLicenseReq(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }    
    }
}