namespace Bars.Gkh1468.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh.Entities;

    public class RealityObject1468Controller : B4.Alt.DataController<RealityObject>
    {
        public ActionResult ListRealityObjectInfo(BaseParams baseParams)
        {
            var data = (ListDataResult)this.Resolve<IRealityObjectService>().ListRealityObjectInfo();

            return new JsonNetResult(new {success = true, data = data.Data, totalCount = data.TotalCount});
        }

        public ActionResult ListView(BaseParams baseParams)
        {
#warning Тут был замут стем что в 1468 добавился новый сервис для работы с реестром домов. Однако в РЕгоператорах есть замуты стем что там свои сервисмы для работы с домами. Так поставил для камчатки
            var result = (ListDataResult)Resolve<Bars.Gkh.DomainService.IRealityObjectService>().ListView(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListForPassport(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListForPassport(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListByPublicServOrg(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListByPublicServOrg(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

		public ActionResult ListRoByPublicServOrg(BaseParams baseParams)
		{
			var result = (ListDataResult)Resolve<IRealityObjectService>().ListRoByPublicServOrg(baseParams);
			return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
		}
    }
}