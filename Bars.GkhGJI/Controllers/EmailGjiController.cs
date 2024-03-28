namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.Gkh.Domain;
    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities.Email;

    public class EmailGjiController : B4.Alt.DataController<EmailGji>
	{
		public IBlobPropertyService<EmailGji, EmailGjiLongText> LongTextService { get; set; }

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
        public ActionResult GetNextQuestion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IEmailGjiService>();
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
        public ActionResult SkipAndGetNextQuestion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IEmailGjiService>();
            try
            {
                var result = service.SkipAndGetNextQuestion(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }

        public ActionResult RegisterEmail(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IEmailGjiService>();
            try
            {
                var result = service.RegisterEmail(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }

        public ActionResult DeclineEmail(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IEmailGjiService>();
            try
            {
                var result = service.DeclineEmail(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }
        public ActionResult GetListAttachments(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IEmailGjiService>();
            try
            {
                return resolutionService.GetListAttachments(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }
    }
}