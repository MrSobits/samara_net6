using Bars.B4;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using Bars.Gkh.Domain;

    public class MandatoryReqsNormativeDocViewModel : BaseViewModel<MandatoryReqsNormativeDoc>
    {
        public override IDataResult List(IDomainService<MandatoryReqsNormativeDoc> domain, BaseParams baseParams)
        {
            var mandatoryReqId = baseParams.Params.GetAsId("mandatoryReqId");

            return domain.GetAll()
                .Where(x => x.MandatoryReqs.Id == mandatoryReqId)
                .Select(x => new
                {
                    x.Id,
                    x.Npa,
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }        
    }
}
