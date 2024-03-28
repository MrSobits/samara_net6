namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    public class FundFormationContractViewModel : BaseViewModel<FundFormationContract>
    {
        public override IDataResult List(IDomainService<FundFormationContract> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.LongTermPrObject.RealityObject.Municipality.Name,
                    LongTermPrObject = x.LongTermPrObject.RealityObject.Address,
                    RegOperator = x.RegOperator.Contragent.Name,
                    x.TypeContract,
                    x.DateStart,
                    x.DateEnd
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.LongTermPrObject)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<FundFormationContract> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var value =domainService.Get(id);

            return value == null
                ? new BaseDataResult()
                : new BaseDataResult(new
                {
                    value.Id,
                    LongTermPrObject = new {value.LongTermPrObject.Id, value.LongTermPrObject.RealityObject.Address},
                    RegOperator = new {value.RegOperator.Id, Contragent = value.RegOperator.Contragent.Name},
                    value.TypeContract,
                    value.ContractNumber,
                    value.ContractDate,
                    value.DateStart,
                    value.DateEnd,
                    value.File
                });
        }
    }

}
