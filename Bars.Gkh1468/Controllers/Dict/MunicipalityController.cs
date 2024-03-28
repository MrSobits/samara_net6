namespace Bars.Gkh1468.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh1468.DomainService;

    public class MunicipalityController : Gkh.Controllers.MunicipalityController
    {
        public ActionResult GetRegionList(string filter)
        {
            var repository = Container.Resolve<IFiasRepository>();
            var adrs =
                repository.GetAll()
                    .Where(x => x.AOLevel == FiasLevelEnum.Region && x.ActStatus == FiasActualStatusEnum.Actual)
                    .Select(x => new { RegionGuid = x.AOGuid, PostCode = x.PostalCode, RegionName = x.OffName })
                    .WhereIf(!string.IsNullOrEmpty(filter), x => x.RegionName.ToUpper().Contains(filter.ToUpper()))
                    .ToArray();

            return new JsonListResult(adrs, adrs.Length);
        }

        public ActionResult GetRegion()
        {
            var repository = Container.Resolve<IFiasRepository>();
            var fiasRegions = repository.GetAll().Where(x => x.ParentGuid == string.Empty);
            if (fiasRegions.Count() == 1)
            {
                return new JsonNetResult(new { success = true, data = fiasRegions.First().OffName });
            }

            return new JsonNetResult(new { success = false });
        }

        public ActionResult GetMunicipalityMap(BaseParams baseParams)
        {
            var data = (ListDataResult)Container.Resolve<IMunicipalityService1468>().GetMunicipalityMap();

            return new JsonNetResult(new { success = true, data = data.Data, totalCount = data.TotalCount });
        }
    }
}