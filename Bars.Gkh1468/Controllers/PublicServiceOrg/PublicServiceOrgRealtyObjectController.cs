namespace Bars.Gkh1468.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.Gkh.Domain;

    using DomainService;

    using Entities;

    /// <summary>
    /// Контроллер РСО
    /// </summary>
    public class PublicServiceOrgRealtyObjectController : B4.Alt.DataController<PublicServiceOrgRealtyObject>
    {
        /// <summary>
        /// Сервис по работе с домами РСО
        /// </summary>
        public IPublicServiceOrgRealtyObjectService Service { get; set; }

        /// <summary>
        /// Добавить дом к РСО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult AddRealtyObjects(BaseParams baseParams)
        {
            return this.Service.AddRealtyObjects(baseParams).ToJsonResult();
        }
    }
}