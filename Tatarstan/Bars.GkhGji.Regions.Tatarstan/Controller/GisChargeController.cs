namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.Gkh.Domain;

    using Entities;
    using Integration;

    /// <summary>
    /// Контроллер работы с интеграцией с ГИС ГМП
    /// </summary>
    public class GisChargeController : B4.Alt.DataController<GisChargeToSend>
    {
        /// <summary>
        /// Отправить данные
        /// </summary>
        public ActionResult Send(BaseParams baseParams)
        {
            var result = this.Resolve<IGisGmpIntegration>().UploadCharges();

            return result.ToJsonResult();
        }

        /// <summary>
        /// Получить оплаты
        /// </summary>
        public ActionResult Upload(BaseParams baseParams)
        {
            var result = this.Resolve<IGisGmpIntegration>().LoadPayments();

            return result.ToJsonResult();
        }
    }
}