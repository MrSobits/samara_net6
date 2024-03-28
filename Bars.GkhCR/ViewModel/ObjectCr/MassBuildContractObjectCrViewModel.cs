using Bars.Gkh.DataResult;

namespace Bars.GkhCr.ViewModel
{
    using System.Linq;
    using B4;
    using Bars.Gkh.Domain;
    using Entities;

    public class MassBuildContractObjectCrViewModel : BaseViewModel<MassBuildContractObjectCr>
    {
        public override IDataResult List(IDomainService<MassBuildContractObjectCr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var buildContractId = baseParams.Params.GetAsId("buildContractId");

            var data = domainService.GetAll()
                .Where(x => x.MassBuildContract.Id == buildContractId)
                .Select(x => new
                {
                    x.Id,
                    ObjectCr = x.ObjectCr.RealityObject.Address,
                    Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                    x.Sum
                })
                .Filter(loadParams, this.Container);

            var summary = data.Sum(x => x.Sum);
            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListSummaryResult(data.ToList(), totalCount, new { Sum = summary });
        }

        public override IDataResult Get(IDomainService<MassBuildContractObjectCr> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var value = domainService.GetAll().Where(x => x.Id == id)
                .Select(x => new
                {
                   x.Id,
                   x.ImportEntityId,
                   x.MassBuildContract,
                   ObjectCrName = x.ObjectCr.RealityObject.Address,
                   x.ObjectCr,
                   x.ObjectCreateDate,
                   x.ObjectEditDate,
                   x.ObjectVersion,
                   x.Sum
                }).FirstOrDefault();

            return new BaseDataResult(value);
        }
    }
}