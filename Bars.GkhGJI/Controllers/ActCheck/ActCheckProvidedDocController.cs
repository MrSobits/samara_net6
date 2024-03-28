namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActCheckProvidedDocController : B4.Alt.DataController<ActCheckProvidedDoc>
    {
        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckProvidedDocService>();
            try
            {
                var result = service.AddProvidedDocs(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
            
        }

        public ActionResult AddCTListAnswers(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckProvidedDocService>();
            try
            {
                var result = service.AddCTListAnswers(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }

        public ActionResult GetNextQuestion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckProvidedDocService>();
            try
            {
                var result = service.GetNextQuestion(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }
        public ActionResult SaveAndGetNextQuestion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckProvidedDocService>();
            try
            {
                var result = service.SaveAndGetNextQuestion(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }

        public ActionResult PrintReport(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckProvidedDocService>();
            try
            {
                var result = service.PrintReport(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }
    }
}