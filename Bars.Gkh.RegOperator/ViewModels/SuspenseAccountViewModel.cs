using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Enums;

namespace Bars.Gkh.RegOperator.ViewModels
{
    using B4.DataAccess;
    using B4.Modules.FIAS;

    class SuspenseAccountViewModel : BaseViewModel<SuspenseAccount>
    {
        public override IDataResult List(IDomainService<SuspenseAccount> domainService, BaseParams baseParams)
        {
            var q = Container.ResolveDomain<RealityObjectPaymentAccount>().GetAll()
                .Join(
                    Container.ResolveDomain<Fias>().GetAll(),
                    x => x.RealityObject.FiasAddress.StreetGuidId,
                    y => y.AOGuid,
                    (a, b) => new
                    {
                        RoId = a.RealityObject.Id,
                        AccountNumber = a.AccountNumber,
                        AddressGuid = a.RealityObject.FiasAddress.AddressGuid,
                        House = a.RealityObject.FiasAddress.House,
                        Housing = a.RealityObject.FiasAddress.Housing,
                        CodeStreet = b.CodeStreet
                    })
                .Where(x => x.AccountNumber == null || x.AccountNumber == "")
                .Where(x => x.AddressGuid != null)
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.First());

            var loadParam = GetLoadParam(baseParams);

            var showDistr = baseParams.Params.GetAs<bool>("showDistr", ignoreCase: true);

            var data = domainService.GetAll()
                .WhereIf(!showDistr, x => x.DistributeState != DistributionState.Distributed)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToArray(), data.Count());
        }
    }
}
