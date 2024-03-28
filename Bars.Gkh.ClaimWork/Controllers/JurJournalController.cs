namespace Bars.Gkh.ClaimWork.Controllers
{
   using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.IoC;
    using DomainService;

    /// <summary>
    /// Контроллер журнала судебной практики
    /// </summary>
    public class JurJournalController : BaseController
    {
        /// <summary>
        /// Список типов основания
        /// </summary>
        /// <returns></returns>
        public ActionResult ListTypeBase()
        {
            var service = Resolve<IJurJournalService>();

            using (Container.Using(service))
            {
                var result = service.ListTypeBase();

                return new JsonListResult(result, result.Count);
            }
        }
    }
}