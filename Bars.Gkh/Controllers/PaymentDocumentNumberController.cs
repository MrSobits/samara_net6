namespace Bars.Gkh.Controllers
{
    using B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain.PaymentDocumentNumber;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для настроек формировантяномера документа
    /// </summary>
    public class PaymentDocumentNumberController : BaseController
    {
        /// <summary>
        /// Список наименований правил
        /// </summary>
        /// <param name="baseParams">Входные параметры запроса</param>
        /// <returns>Список правил</returns>
        public ActionResult ListRulesName(BaseParams baseParams)
        {
            var rules = this.Container.ResolveAll<PaymentDocumentNumberRuleBase>().ToList();

            using (this.Container.Using(rules))
            {
                return new JsonListResult(
                    rules.Select(x => new
                    {
                        x.Name,
                        x.Description,
                        x.SymbolsCountConfig,
                        x.SymbolsLocationConfig,
                        x.DefaultConfig,
                        x.IsRequired
                    })
                    .ToList(), 
                    rules.Count);
            }
        }

        /// <summary>
        /// Получить пример номера
        /// </summary>
        /// <returns>Пример номера</returns>
        public ActionResult GetExample()
        {
            using (var paymentDocumentNumberBuilder = new PaymentDocumentNumberBuilder(this.Container))
            {
                return new JsonNetResult(
                    new
                    {
                        success = true,
                        data = paymentDocumentNumberBuilder.GetDocumentNumberExample()
                    });
            }
        }
    }
}
