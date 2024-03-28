namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActCheck
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    public class ActCheckRealityObjectController : GkhGji.Controllers.ActCheckRealityObjectController
    {
        public IBlobPropertyService<ActCheckRealityObject, ActCheckRoLongDescription> LongTextService { get; set; }

        public ActionResult GetRobjectCharacteristics(BaseParams baseParams)
        {
            var service = this.Resolve<IActCheckRoChelyabinskService>();

            try
            {
                var result = service.GetRobjectCharacteristics(baseParams);

                return result.ToJsonResult();
            }
            finally
            {
                this.Container.Release(service); 
            }
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            return this.SaveBlob(baseParams);
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            return this.GetBlob(baseParams);
        }

        public virtual ActionResult SaveNotRevealedViolations(BaseParams baseParams)
        {
            return this.SaveBlob(baseParams);
        }

        public virtual ActionResult GetNotRevealedViolations(BaseParams baseParams)
        {
            return this.GetBlob(baseParams);
        }

        public virtual ActionResult SaveAdditionalChars(BaseParams baseParams)
        {
            return this.SaveBlob(baseParams);
        }

        public virtual ActionResult GetAdditionalChars(BaseParams baseParams)
        {
            return this.GetBlob(baseParams);
        }

        private ActionResult SaveBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        private ActionResult GetBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}