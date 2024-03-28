using Bars.Gkh.Entities;

namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    public class ContragentClwMunicipalityViewModel : BaseViewModel<ContragentClwMunicipality>
    {
        public override IDataResult List(IDomainService<ContragentClwMunicipality> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var contragentClwId = loadParams.Filter.GetAsId("contragentClwId");

            return domain.GetAll()
                .Where(x => x.ContragentClw.Id == contragentClwId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name
                })
                .ToListDataResult(loadParams);
        }
    }
}