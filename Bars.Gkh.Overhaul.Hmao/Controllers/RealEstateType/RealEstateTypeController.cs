namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Controllers.Dict.RealEstateType;
    using Bars.Gkh.Overhaul.Hmao.DomainService;

    public class RealEstateTypeController : BaseRealEstateTypeController
    {
        /// <summary>
        /// IHmaoRealEstateTypeService
        /// </summary>
        public IHmaoRealEstateTypeService Service { get; set; }

        /// <summary>
        /// Получить список муниципальных образований
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список муниципальных образований</returns>
        public ActionResult GetMuList(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Service.GetMuList(baseParams);
            return new JsonListResult((IList)result.Data);
        }
    }
}
