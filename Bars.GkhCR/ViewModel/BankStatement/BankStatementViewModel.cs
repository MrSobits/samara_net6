namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Entities;

    public class BankStatementViewModel : BaseViewModel<BankStatement>
    {
        public override IDataResult List(IDomainService<BankStatement> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = Container.Resolve<IBankStatementService>().GetFilteredByOperator()
                .Select(x => new
                    {
                        x.Id,
                        ObjectCrName = x.ObjectCr.RealityObject.Address,
                        ManagingOrganizationName = x.ManagingOrganization.Contragent.Name,
                        ContragentName = x.Contragent.Name,
                        PeriodName = x.Period.Name,
                        MunicipalityName = x.ObjectCr.RealityObject.Municipality.Name,
                        x.TypeFinanceGroup,
                        x.BudgetYear,
                        x.IncomingBalance,
                        x.OutgoingBalance,
                        x.PersonalAccount,
                        x.DocumentNum,
                        x.OperLastDate,
                        x.DocumentDate
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.ObjectCrName)
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<BankStatement> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id", 0);
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(new
            {
                obj.Id,
                obj.ObjectCr,
                ManagingOrganization = obj.ManagingOrganization != null ?
                new {  
                        obj.ManagingOrganization.Id,
                        ContragentName = obj.ManagingOrganization.Contragent.Name 
                    } : null,
                obj.Contragent,
                obj.Period,
                obj.TypeFinanceGroup,
                obj.BudgetYear,
                obj.IncomingBalance,
                obj.OutgoingBalance,
                obj.PersonalAccount,
                obj.DocumentNum,
                obj.OperLastDate,
                obj.DocumentDate
            }): new BaseDataResult();
        }
    }
}