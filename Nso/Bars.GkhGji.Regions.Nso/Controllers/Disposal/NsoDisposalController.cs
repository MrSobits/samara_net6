namespace Bars.GkhGji.Regions.Nso.Controllers.Disposal
{
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Regions.Nso.DomainService;
    using Bars.GkhGji.Regions.Nso.Entities;
    using GkhGji.DomainService;
	using Microsoft.AspNetCore.Mvc;
	using Bars.B4;
	using Bars.Gkh.DomainService;
	using Bars.GkhGji.Regions.Nso.Entities.Disposal;

    // На основе существующег оконтроллера делаем совй для того чтобы все запросы шли на новый Url
	public class NsoDisposalController : DisposalController<NsoDisposal>
    {
        public IBlobPropertyService<NsoDisposal, DisposalLongText> LongTextService { get; set; }

        public virtual ActionResult SaveNoticeDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetNoticeDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult AddFactViolation(BaseParams baseParams)
        {
            var dispFactViolationService = Container.Resolve<IDisposalFactViolationService>();

            try
            {
                var result = dispFactViolationService.AddFactViolation(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(dispFactViolationService);
            }
        }

		public ActionResult AddSurveySubjects(BaseParams baseParams)
		{
			var result = Container.Resolve<IDisposalVerificationSubjectService>().AddSurveySubjects(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddSurveyPurposes(BaseParams baseParams)
		{
			var result = Container.Resolve<IDisposalSurveyPurposeService>().AddSurveyPurposes(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddSurveyObjectives(BaseParams baseParams)
		{
			var result = Container.Resolve<IDisposalSurveyObjectiveService>().AddSurveyObjectives(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddInspFoundations(BaseParams baseParams)
		{
			var result = Container.Resolve<IDisposalInsFoundationService>().AddInspFoundations(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddInspFoundationChecks(BaseParams baseParams)
		{
			var result = Container.Resolve<IDisposalInsFoundationCheckService>().AddInspFoundationChecks(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddAdminRegulations(BaseParams baseParams)
		{
			var result = Container.Resolve<IDisposalAdminRegulationService>().AddAdminRegulations(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}
	}
}
