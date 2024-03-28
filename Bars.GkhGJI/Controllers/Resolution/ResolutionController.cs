namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ResolutionController : ResolutionController<Entities.Resolution>
    {
    }

    public class ResolutionController<T> : B4.Alt.DataController<T>
        where T : Entities.Resolution
    {

        public Gkh.DomainService.IBlobPropertyService<Entities.Resolution, Entities.ResolutionLongText> LongTextService { get; set; }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
        public ActionResult GetInfo(long? documentId)
        {
            var service = this.Container.Resolve<DomainService.IResolutionService>();
            try
            {
                var result = service.GetInfo(documentId);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        public ActionResult GetResolutionInfo(BaseParams baseParams)
        {
            var service = this.Container.Resolve<DomainService.IResolutionService>();
            try
            {
                var result = service.GetResolutionInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var service = this.Container.Resolve<DomainService.IResolutionService>();
            try
            {
                return service.ListView(baseParams).ToJsonResult();
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ResolutionDataExport");

            try
            {
                return export?.ExportData(baseParams);
            }
            finally 
            {
                this.Container.Release(export);
            }
        }
    }
}