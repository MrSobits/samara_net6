namespace Bars.Gkh.Gis.Controllers.RealityObject
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Gkh.Entities;

    public class RealityObjectExtraController : BaseController
    {
        protected IRepository<RealityObject> RealityObjectRepository;

        public RealityObjectExtraController(
            IRepository<RealityObject> realityObjectRepository
            )
        {
            RealityObjectRepository = realityObjectRepository;
        }

        public ActionResult GetCodes(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var codes = RealityObjectRepository.Get(id).Return(x => x.CodeErc).Split(",");

            return new JsonNetResult(new { success = true, codes });
        }
    }
}
