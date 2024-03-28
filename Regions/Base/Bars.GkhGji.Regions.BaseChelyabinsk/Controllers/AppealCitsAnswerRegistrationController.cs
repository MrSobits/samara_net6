namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Email;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Microsoft.AspNetCore.Mvc;

    public class AppealCitsAnswerRegistrationController : FileStorageDataController<AppealCitsAnswer>
    {
        public IBlobPropertyService<AppealCitsAnswer, AppealAnswerLongText> LongTextService { get; set; }

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
            var service = this.Container.Resolve<IAppealCitsAnswerRegistrationService>();
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
            var service = this.Container.Resolve<IAppealCitsAnswerRegistrationService>();
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

        public ActionResult RegisterAnswer(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsAnswerRegistrationService>();
            try
            {
                var result = service.RegisterAnswer(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }

        public ActionResult RegisterAndSendAnswer(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsAnswerRegistrationService>();
            try
            {
                var result = service.RegisterAndSendAnswer(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }

        }
        public ActionResult GetAnswer(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IAppealCitsAnswerRegistrationService>();
            try
            {
                return resolutionService.GetAnswer(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }
        public ActionResult GetList(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IAppealCitsAnswerRegistrationService>();
            try
            {
                return resolutionService.GetList(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }
    }
}