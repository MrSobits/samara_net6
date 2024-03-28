namespace Bars.GkhGji.Controllers
{

    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;

    using Bars.Gkh.Domain;

    using DomainService;
    using Entities;

    public class BaseStatementController : BaseStatementController<BaseStatement>
    {
    }
    
    public class BaseStatementController<T> : B4.Alt.DataController<T>
        where T : BaseStatement
    {
        public IBaseStatementService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("BaseStatementDataExport");

            try
            {
                return export?.ExportData(baseParams);
            }
            finally 
            {
                this.Container.Release(export);
            }
            
        }

        public ActionResult AddAppealCitizens(BaseParams baseParams)
        {
            var result = this.Service.AddAppealCitizens(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CreateWithAppealCits(BaseParams baseParams)
        {
            var result = this.Service.CreateWithAppealCits(baseParams);

            return result.Success
                ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data })
                {
                    ContentType = "text/html; charset=utf-8"
                }
                : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CreateWithBasis(BaseParams baseParams)
        {
            return this.Service.CreateWithBasis(baseParams).ToJsonResult();
        }

        public ActionResult AddBasisDocs(BaseParams baseParams)
        {
            return this.Service.AddBasisDocs(baseParams).ToJsonResult();
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = this.Service.GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult ListByAppealCits(BaseParams baseParams)
        {
            return this.Service.ListByAppealCits(baseParams).ToJsonResult();
        }

        public ActionResult CheckAppealCits(BaseParams baseParams)
        {
            return this.Service.CheckAppealCits(baseParams).ToJsonResult();
        }

        public ActionResult AnyThematics(BaseParams baseParams)
        {
            return this.Service.AnyThematics(baseParams).ToJsonResult();
        }

    }
}