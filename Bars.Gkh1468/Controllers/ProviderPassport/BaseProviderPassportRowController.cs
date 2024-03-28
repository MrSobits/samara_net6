namespace Bars.Gkh1468.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using DomainService;

    public class BaseProviderPassportRowController<T> : B4.Alt.DataController<T> where T : BaseProviderPassportRow
    {
        public IBaseProviderPassportRowService<T> Service { get; set; }

        public ActionResult SaveRecord(BaseParams baseParams)
        {
            var result = Container.Resolve<IBaseProviderPassportRowService<T>>().SaveRecord(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetMultyMetaValues(BaseParams baseParams)
        {
            return new JsonNetResult(Service.GetMultyMetaValues(baseParams));
        }

        public ActionResult GetMetaValues(BaseParams baseParams)
        {
            return new JsonNetResult(Service.GetMetaValues(baseParams));
        }

        public ActionResult GetMultyMetaValue(BaseParams baseParams)
        {
            return new JsonNetResult(Service.GetMultyMetaValue(baseParams));
        }

        public ActionResult DeleteMultyMetaValues(BaseParams baseParams)
        {
            return new JsonNetResult(Service.DeleteMultyMetaValues(baseParams));
        }

    }
}