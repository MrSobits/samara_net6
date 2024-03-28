namespace Bars.GkhCr.Regions.Tatarstan.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.Regions.Tatarstan.Entities;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class TatarstanCrMenuController : BaseMenuController
    {
        /// <summary>
        /// Получает меню для "Объект программы благоустройства".
        /// </summary>
        /// <param name="baseParams">Базовые параметры.</param>
        public ActionResult GetObjectOutdoorMenu(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("objectId");
            return id > 0
                ? new JsonNetResult(this.GetMenuItems(nameof(ObjectOutdoorCr)))
                : new JsonNetResult(null);
        }
    }
}
