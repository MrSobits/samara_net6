using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Config;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;

namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Overhaul.Entities;

    public class RealEstateTypeMunicipalityViewModel : BaseViewModel<RealEstateTypeMunicipality>
    {
        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IGkhParams GkhParams { get; set; }

        public override IDataResult List(IDomainService<RealEstateTypeMunicipality> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var retId = baseParams.Params.GetAs<long>("RealEstateTypeId");

            var appParams = GkhParams.GetParams();

            var moLevel = appParams.ContainsKey("RealEstTypeMoLevel")
                ? appParams["RealEstTypeMoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var muIds = MunicipalityDomain.GetAll()
             .Select(x => new { x.Id, x.Level })
             .AsEnumerable()
             .Where(x => x.Level.ToMoLevel(Container) == moLevel)
             .Select(x => x.Id)
             .ToList();

            var data = domainService.GetAll()
                .Where(x => x.RealEstateType.Id == retId && muIds.Contains(x.Municipality.Id))
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}