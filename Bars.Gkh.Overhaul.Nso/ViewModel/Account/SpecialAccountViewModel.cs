namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class SpecialAccountViewModel : BaseViewModel<SpecialAccount>
    {
        public override IDataResult List(IDomainService<SpecialAccount> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var roId = loadParams.Filter.GetAs<long>("roId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    CreditOrganization = x.Decision.CreditOrg.Name,
                    //AccountOwner = x.Decision.ManagingOrganization.Contragent.Name ?? x.Decision.RegOperator.Contragent.Name,
                    Number = x.Decision.AccountNumber,
                    x.Decision.OpenDate,
                    x.Decision.CloseDate,
                    x.TotalOut,
                    x.TotalIncome,
                    x.Balance,
                    x.LastOperationDate
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<SpecialAccount> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

            return new BaseDataResult(new
            {
                obj.Id,
                Decision =
                    new {obj.Decision.Id, DecisionType = obj.Decision.PropertyOwnerDecisionType.GetEnumMeta().Display},
                Number = obj.Decision.AccountNumber,
                //OwnerName = obj.Decision.RegOperator != null
                //    ? obj.Decision.RegOperator.Contragent.Name
                //    : obj.Decision.ManagingOrganization != null
                //        ? obj.Decision.ManagingOrganization.Contragent.Name
                //        : "",
                obj.Decision.OpenDate,
                obj.Decision.CloseDate,
                obj.CreditLimit,
                CreditOrgName = obj.Decision.CreditOrg != null ? obj.Decision.CreditOrg.Name : null
            });
        }
    }
}