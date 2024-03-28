namespace Bars.Gkh1468.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh1468.DomainService;

    /// <summary>
    /// Контроллер <see cref="PublicServiceOrgContractRealObj"/>
    /// </summary>
    public class PublicServiceOrgContractRealObjController : B4.Alt.DataController<PublicServiceOrgContractRealObj>
    {
        /// <summary>
        /// Сервис работы с домами
        /// </summary>
        public IRealityObjectService RealityObjectService { get; set; }

        /// <summary>
        /// Сервис по работе с домами РСО
        /// </summary>
        public IPublicServiceOrgRealtyObjectService PublicServiceOrgRealtyObjectService { get; set; }

        /// <summary>
        /// Список Жилых домов по договору РСО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult ListByPublicServOrgContract(BaseParams baseParams)
        {
            return this.RealityObjectService.ListByPublicServOrgContract(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Добавить дом к контракту РСО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult AddRealtyObjectsToContract(BaseParams baseParams)
        {
            return this.PublicServiceOrgRealtyObjectService.AddRealtyObjectsToContract(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить контрактов РСО для жилого дома
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult ListByRealityObject(BaseParams baseParams)
        {
            return this.PublicServiceOrgRealtyObjectService.ListByRealityObject(baseParams).ToJsonResult();
        }
    }
}