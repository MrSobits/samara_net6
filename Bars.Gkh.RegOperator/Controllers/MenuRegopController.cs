namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;

    using DomainService.Interface;

    /// <summary>
    /// Контроллер начислений
    /// </summary>
    public class MenuRegopController : MenuController
    {
        /// <summary>
        /// Начисления по Л/С
        /// </summary>
        public IPersonalAccountCharger Charger { get; set; }

        /// <summary>
        /// Меню анализа сведений от биллинга
        /// </summary>
        public ActionResult GetChesMenu(StoreLoadParams storeParams)
        {
            var defaultMenu = this.GetMenuItems("ChesImport");

            return new JsonNetResult(defaultMenu.Union(this.GetChesPaymentsMenu(storeParams)));
        }

        private IEnumerable<MenuItem> GetChesPaymentsMenu(BaseParams baseParams)
        {
            var result = Enumerable.Empty<MenuItem>();
            if (baseParams.Params.GetAs<long>("periodId") == 0)
            {
                return result;
            }
            this.Container.UsingForResolved<IChesImportService>((ioc, service) =>
            {
                var paymentDays = service.GetPaymentDays(baseParams);
                if (paymentDays.IsNotEmpty())
                {
                    var paymentsMenu = new MenuItem("Оплаты", null);
                    foreach (var paymentDay in paymentDays.OrderBy(x => x))
                    {
                        paymentsMenu.Add($"День {paymentDay}", $"chesimport_detail/{{0}}/payments/{paymentDay}");
                    }

                    result = new[] { paymentsMenu };
                }

            });

            return result;
        }
    }
}