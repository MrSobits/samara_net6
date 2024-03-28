namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    public class CreditOrgServiceConditionViewModel : BaseViewModel<CreditOrgServiceCondition>
    {
        public override IDataResult List(IDomainService<CreditOrgServiceCondition> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    CreditOrgName = x.CreditOrg.Name,
                    x.CashServiceSize,
                    x.CashServiceDateFrom,
                    x.CashServiceDateTo,
                    x.OpeningAccPay,
                    x.OpeningAccDateFrom,
                    x.OpeningAccDateTo
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.CreditOrgName)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}