namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Контроллер договоры на услуги
    /// </summary>
    public class ContractCrController : B4.Alt.DataController<ContractCr>
    {
        /// <summary>
        /// Добавить виды работ
        /// </summary>
        public ActionResult AddTypeWorks(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IContractCrService>();
            try
            {
                var result = service.AddTypeWorks(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}