namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;


    public class ActCheckRealityObjectController : ActCheckRealityObjectController<ActCheckRealityObject>
    {
    }

    // Класс переделан на для того чтобы в регионах можно было расширят ьсущность через subclass 
    // и при этом не писать дублирующий серверный код
    public class ActCheckRealityObjectController<T> : B4.Alt.DataController<T>
        where T : ActCheckRealityObject
    {
        public ActionResult SaveParams(BaseParams baseParams)
        {
            var actCheckRealityObjectService = Container.Resolve<IActCheckRealityObjectService>();

            using (this.Container.Using(actCheckRealityObjectService))
            {
                var result = actCheckRealityObjectService.SaveParams(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}