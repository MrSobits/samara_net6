using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Entities;

namespace Bars.Gkh.RegOperator.Controllers
{
    using Bars.B4.Alt;
    using Bars.B4.Modules.FIAS;

    using Microsoft.AspNetCore.Mvc;

    public class LocationCodeFiasController: BaseDataController<Fias>
    {
        public override ActionResult List(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var muId = baseParams.Params.GetAs<long>("municipalityId");

            var mo = Container.Resolve<IRepository<Municipality>>().GetAll()
                .Where(x => x.Id == muId)
                .Select(x => new { x.Okato, x.Oktmo, x.FiasId, ParentFiasId = x.ParentMo.FiasId })
                .FirstOrDefault();

            var moOktmo = string.Empty;
            var fiasId = string.Empty;

            if (mo != null)
            {
                moOktmo = mo.Oktmo != null ? mo.Oktmo.ToString() : string.Empty;

                fiasId = (string.IsNullOrWhiteSpace(mo.FiasId) ? mo.ParentFiasId : mo.FiasId).ToStr();
            }

            var data = DomainService.GetAll()
                .WhereIf(mo != null, x => x.OKATO == mo.Okato || x.OKTMO == moOktmo || (x.OKTMO.Length == 0 && x.ParentGuid == fiasId))
                .Where(x => x.AOLevel == FiasLevelEnum.Place || x.AOLevel == FiasLevelEnum.City)
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Filter(loadParams, Container);

            int totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new JsonListResult(data.ToList(), totalCount);
        }
    }
}
