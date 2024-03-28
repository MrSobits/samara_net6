namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер договоры на услуги
    /// </summary>
    public class SpecialContractCrController : B4.Alt.DataController<SpecialContractCr>
    {
        /// <summary>
        /// Добавить виды работ
        /// </summary>
        public ActionResult AddTypeWorks(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialContractCrService>();
            using (this.Container.Using(service))
            {
                var result = service.AddTypeWorks(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
        }
    }
}