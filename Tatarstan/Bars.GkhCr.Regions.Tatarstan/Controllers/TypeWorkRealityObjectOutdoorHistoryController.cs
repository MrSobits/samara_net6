namespace Bars.GkhCr.Regions.Tatarstan.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhCr.Regions.Tatarstan.DomainService;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class TypeWorkRealityObjectOutdoorHistoryController : B4.Alt.DataController<TypeWorkRealityObjectOutdoorHistory>
    {
        /// <summary>
        /// Восстанавливает записи из истории.
        /// </summary>
        public ActionResult Recover(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITypeWorkRealityObjectOutdoorHistoryService>();
            using (this.Container.Using(service))
            {
                var result = service.Recover(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
        }
    }
}
