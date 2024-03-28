namespace Bars.GkhGji.Regions.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using DomainService;
    using Gkh.Domain;
    using Gkh.DomainService;
    using GkhGji.Entities;
    using Entities;

    public class ActCheckRealityObjectController : GkhGji.Controllers.ActCheckRealityObjectController
    {
        public IBlobPropertyService<ActCheckRealityObject, ActCheckRoLongDescription> LongTextService { get; set; }

        public ActionResult GetRobjectCharacteristics(BaseParams baseParams)
        {
            var service = Resolve<IActCheckRoNsoService>();

            try
            {
                var result = service.GetRobjectCharacteristics(baseParams);

                return result.ToJsonResult();
            }
            finally
            {
                Container.Release(service); 
            }
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            return SaveBlob(baseParams);
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            return GetBlob(baseParams);
        }

        public virtual ActionResult SaveNotRevealedViolations(BaseParams baseParams)
        {
            return SaveBlob(baseParams);
        }

        public virtual ActionResult GetNotRevealedViolations(BaseParams baseParams)
        {
            return GetBlob(baseParams);
        }

        public virtual ActionResult SaveAdditionalChars(BaseParams baseParams)
        {
            return SaveBlob(baseParams);
        }

        public virtual ActionResult GetAdditionalChars(BaseParams baseParams)
        {
            return GetBlob(baseParams);
        }

        private ActionResult SaveBlob(BaseParams baseParams)
        {
            var result = LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        private ActionResult GetBlob(BaseParams baseParams)
        {
            var result = LongTextService.Get(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}