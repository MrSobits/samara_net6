using Bars.B4.Modules.DataExport.Domain;
using Bars.GkhCr.DomainService;

namespace Bars.GkhCr.Controllers.BankStatement
{
    using Bars.B4;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class BuilderViolatorController : B4.Alt.DataController<BuilderViolator>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BuilderViolatorDataExport");

            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                Container.Release(export);
            }
        }

        public ActionResult AddViolations(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuilderViolatorService>();

            try
            {
                var result = service.AddViolations(baseParams);

                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }


        public ActionResult Clear(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuilderViolatorService>();

            try
            {
                var result = service.Clear(baseParams);

                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult MakeNew(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuilderViolatorService>();

            try
            {
                var result = service.MakeNew(baseParams);

                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ValidateToCreateClaimWorks(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuilderViolatorService>();

            try
            {
                var result = service.ValidateToCreateClaimWorks(baseParams);

                return result.Success ? new JsonNetResult( new { message = result.Message }) : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult CreateClaimWorks(BaseParams baseParams)
        {
            var service = Container.Resolve<IBuilderViolatorService>();

            try
            {
                var result = service.CreateClaimWorks(baseParams);

                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}