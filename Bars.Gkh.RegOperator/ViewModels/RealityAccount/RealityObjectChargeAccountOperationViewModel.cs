namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;

    using Entities;
    using Gkh.Domain;

    public class RealityObjectChargeAccountOperationViewModel : BaseViewModel<RealityObjectChargeAccountOperation>
    {
        public override IDataResult List(IDomainService<RealityObjectChargeAccountOperation> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var accountId = baseParams.Params.GetAsId("accId");

            if (accountId == 0)
            {
                accountId = loadParam.Filter.GetAsId("accId");
            }

            ReplacePeriodOrder(loadParam.Order);

            var data =
                domainService.GetAll()
                    .Where(x => x.Account.Id == accountId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Account,
                        PeriodStart = x.Period.StartDate,
                        Period = x.Period.Name,
                        x.Date,
                        x.SaldoIn,
                        x.ChargedTotal,
                        x.ChargedPenalty,
                        x.PaidTotal,
                        x.PaidPenalty,
                        x.SaldoOut
                    })
                    .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }

        private void ReplacePeriodOrder(OrderField[] orderFields)
        {
            foreach (var orderField in orderFields)
            {
                if (orderField.Name == "Period")
                {
                    orderField.Name = "PeriodStart";
                }
            }
        }
    }
}