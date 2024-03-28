namespace Bars.GkhGji.Regions.Smolensk.Controllers.Disposal
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhGji.Regions.Smolensk.DomainService.Disposal;
    using Bars.GkhGji.Regions.Smolensk.Entities.Disposal;

    public class DisposalSurveySubjectController : B4.Alt.DataController<DisposalSurveySubject>
    {
        public ActionResult AddDisposalSurveySubject(BaseParams baseParams)
        {
            var service = Container.Resolve<IDisposalSurveySubjectService>();

            try
            {
                var result = service.AddDisposalSurveySubject(baseParams);

                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}