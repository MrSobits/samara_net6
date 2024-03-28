namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;
    using Overhaul.DomainService;

    public class OvrhlWorkController: Bars.B4.Alt.DataController<Work>
    {
        public override ActionResult Create(BaseParams baseParams) 
        {
            var service = Container.Resolve<IWorkService>();
            var result = service.SaveWithFinanceType(baseParams, DomainService);
            return new JsonNetResult(result);
        }

        public override ActionResult Update(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkService>();
            var result = service.UpdateWithFinanceType(baseParams, DomainService);
            return new JsonNetResult(result);
        }

        public override ActionResult Delete(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkService>();
            var result = service.DeleteWithFinanceType(baseParams, DomainService);
            return new JsonNetResult(result);
        }
    }
}