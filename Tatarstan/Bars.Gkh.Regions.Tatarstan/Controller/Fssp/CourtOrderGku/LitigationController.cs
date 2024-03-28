namespace Bars.Gkh.Regions.Tatarstan.Controller.Fssp.CourtOrderGku
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    /// <summary>
    /// Контроллер для <see cref="Litigation"/>
    /// </summary>
    public class LitigationController : B4.Alt.DataController<Litigation>
    {
        /// <summary>
        /// Получить регистрационный номер ИП
        /// </summary>
        public ActionResult GetIndEntrRegistrationNumbers(long addressId)
        {
            var litigationService = this.Container.Resolve<ILitigationService>();

            using (this.Container.Using(litigationService))
            {
                return new JsonNetResult(new BaseDataResult(litigationService.GetIndEntrRegistrationNumbers(addressId)));
            }
        }

        /// <summary>
        /// Получить список оплат по ЖКУ
        /// </summary>
        public ActionResult PaymentList(BaseParams baseParams)
        {
            var litigationService = this.Container.Resolve<ILitigationService>();

            using (this.Container.Using(litigationService))
            {
                return new JsonListResult(litigationService.PaymentList(baseParams));
            }
        }
    }
}