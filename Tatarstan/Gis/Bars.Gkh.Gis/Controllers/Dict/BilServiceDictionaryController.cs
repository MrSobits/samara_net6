namespace Bars.Gkh.Gis.Controllers.Dict
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.Dict;
    using Entities.Kp50;

    /// <summary>
    /// Контроллер BilServiceDictionary
    /// </summary>
    public class BilServiceDictionaryController : B4.Alt.DataController<BilServiceDictionary>
    {
        /// <summary>
        /// Сервис BilServiceDictionary
        /// </summary>
        public IBilServiceDictionaryService BilServiceDictionaryService { get; set; }

        /// <summary>
        /// Получить список дополнительных услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список дополнительных услуг</returns>
        public ActionResult ListAdditionalService(BaseParams baseParams)
        {
            var result = (ListDataResult)this.BilServiceDictionaryService.ListAdditionalService(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Получить список коммунальных услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список коммунальных услуг</returns>
        public ActionResult ListCommunalService(BaseParams baseParams)
        {
            var result = (ListDataResult)this.BilServiceDictionaryService.ListCommunalService(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Получить список работ и услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список работ и услуг</returns>
        public ActionResult ListServiceWork(BaseParams baseParams)
        {
            var result = (ListDataResult)this.BilServiceDictionaryService.ListServiceWork(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}
