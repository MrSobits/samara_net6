namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Disposal
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class ChelyabinskDisposalController : DisposalController<ChelyabinskDisposal>
    {
        public IBlobPropertyService<ChelyabinskDisposal, DisposalLongText> LongTextService { get; set; }

        public virtual ActionResult SaveNoticeDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetNoticeDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult AddFactViolation(BaseParams baseParams)
        {
            var dispFactViolationService = this.Container.Resolve<IDisposalFactViolationService>();

            try
            {
                var result = dispFactViolationService.AddFactViolation(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(dispFactViolationService);
            }
        }

        public ActionResult AddAdminRegulations(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IDisposalAdminRegulationService>().AddAdminRegulations(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
