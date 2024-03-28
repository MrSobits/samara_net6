namespace Bars.Gkh.Gis.Controllers.Register.HouseServiceRegister
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.Register.HouseServiceRegister;
    using Entities.Register.HouseServiceRegister;

    public class HouseServiceRegisterController :  B4.Alt.DataController<HouseServiceRegister>
    {
        protected IHouseServiceRegisterService Service { get; set; }

        public HouseServiceRegisterController(IHouseServiceRegisterService service)
        {
            Service = service;
        }

        /// <summary>
        /// Список поставщиков
        /// </summary>
        public ActionResult SupplierList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.SupplierList(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult SupplierListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.SupplierListWithoutPaging(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Список услуг
        /// </summary>
        public ActionResult ServiceList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ServiceList(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Список групп услуг
        /// </summary>
        public ActionResult ServiceGroupList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ServiceGroupList(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Список муниципальных районов
        /// </summary>
        public ActionResult MunicipalAreaList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.MunicipalAreaList(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Список населенных пунктов
        /// </summary>
        public ActionResult SettlementList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.SettlementList(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Список улиц/домов
        /// </summary>
        public ActionResult StreetList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.StreetList(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Список расхождений объемов
        /// </summary>
        public ActionResult DiscrepancyList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.DiscrepancyList(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Пометить данные как "Опубликовано"
        /// </summary>
        public ActionResult Publish(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.Publish(baseParams);
            return new JsonNetResult(result);
        }
    }
}