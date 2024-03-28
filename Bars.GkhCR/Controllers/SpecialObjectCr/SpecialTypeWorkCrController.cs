namespace Bars.GkhCr.Controllers
{
    using System.Collections;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialTypeWorkCrController : B4.Alt.DataController<SpecialTypeWorkCr>
    {
        /// <summary>
        /// Сервис
        /// </summary>
        public ISpecialTypeWorkCrService Service { get; set; }

        /// <summary>
        /// метод для получения списка работ по жилому дому и периоду, используется
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ListRealityObjectWorksByPeriod(BaseParams baseParams)
        {
            var result = this.Service.ListRealityObjectWorksByPeriod(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        public ActionResult CalcPercentOfCompletion(BaseParams baseParams)
        {
            var result = this.Service.CalcPercentOfCompletion(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        public ActionResult ListByProgramCr(BaseParams baseParams)
        {
            var totalCount = 0;
            var list = this.Resolve<ISpecialTypeWorkCrService>().ListByProgramCr(baseParams, true, ref totalCount);
            return new JsonListResult(list, totalCount);
        }

        public ActionResult ListWorksCr(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Service.ListWorksCr(baseParams);
            return new JsonListResult((IEnumerable) result.Data, result.TotalCount);
        }

        public ActionResult CreateTypeWork(BaseParams baseParams)
        {
            return this.Service.CreateTypeWork(baseParams).ToJsonResult();
        }

        public ActionResult ListFinanceSources(BaseParams baseParams)
        {
            return this.Service.ListFinanceSources(baseParams).ToJsonResult();
        }
    }
}