namespace Bars.GkhRf.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Domain;
    using Entities;
    using Enums;

    public class TransferRfRecObjViewModel : BaseViewModel<TransferRfRecObj>
    {
        public override IDataResult List(IDomainService<TransferRfRecObj> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var contractRfId = baseParams.Params.GetAsId("contractRfId");
            var transferRfRecordId = baseParams.Params.GetAsId("transferRfRecordId");
            var dateTransfer = baseParams.Params.GetAs<DateTime>("dateTransfer");

            // Получаем дома из данного договора которые исключены что бы не отображать их даже если они имееют ссылку на данную сущность
            var serviceContractRfObject = Container.Resolve<IDomainService<ContractRfObject>>();
            var excludeObjects =
                serviceContractRfObject.GetAll()
                    .Where(x => x.ContractRf.Id == contractRfId)
                    .Where(x => x.TypeCondition == TypeCondition.Exclude)
                    .Where(x => x.IncludeDate > dateTransfer || x.ExcludeDate < dateTransfer);

            var data = domain.GetAll()
                .Where(y => !excludeObjects.Any(x => x.RealityObject.Id == y.RealityObject.Id))
                .Where(x => x.TransferRfRecord.Id == transferRfRecordId)
                .Select(x => new
                {
                    x.Id,
                    MunicipalityName = x.RealityObject.Municipality.Name,
                    RealityObjectName = x.RealityObject.FiasAddress.AddressName,
                    x.RealityObject.GkhCode,
                    x.Sum
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjectName)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
