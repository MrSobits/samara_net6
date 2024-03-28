namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class CompetitionLotTypeWorkController : FileStorageDataController<CompetitionLotTypeWork>
    {
        public ActionResult AddWorks(BaseParams baseParams)
        {
            var service = Container.Resolve<ICompetitionLotTypeWorkService>();
            try
            {
                var result = service.AddWorks(baseParams);
                return result.Success ? JsonNetResult.Success : JsFailure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}
