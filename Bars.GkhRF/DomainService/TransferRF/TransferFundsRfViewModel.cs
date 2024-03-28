namespace Bars.GkhRf.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class TransferFundsRfViewModel : BaseViewModel<TransferFundsRf>
    {
        public override IDataResult List(IDomainService<TransferFundsRf> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var requestTransferRfId = baseParams.Params.GetAsId("requestTransferRfId");

            var data = domain.GetAll()
                .Where(x => x.RequestTransferRf.Id == requestTransferRfId)
                .Select(x => new
                {
                    x.Id,
                    RealityObjectName = x.RealityObject.Address,
                    x.WorkKind,
                    x.PayAllocate,
                    x.PersonalAccount,
                    x.Sum
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.RealityObjectName)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}