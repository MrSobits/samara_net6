namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class RegOpCalcAccountViewModel : BaseViewModel<RegOpCalcAccount>
    {
        public override IDataResult List(IDomainService<RegOpCalcAccount> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var regOperatorId = loadParams.Filter.GetAs<long>("regOpId");
            var isSpecial = loadParams.Filter.GetAs<bool>("isSpecial");

            var data = domainService.GetAll()
                .Where(x => x.RegOperator.Id == regOperatorId)
                .Where(x => x.IsSpecial == isSpecial)
                .Select(x => new
                    {
                        x.Id,
                        x.ContragentBankCrOrg,
                        x.ContragentBankCrOrg.CreditOrg, //TODO проставить ссылку через расчетный счет
                        x.OpenDate,
                        x.CloseDate,
                        x.TotalIncome,
                        x.TotalOut,
                        x.BalanceIncome
                    })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<RegOpCalcAccount> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var rec = domainService.GetAll().Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.AccountType,
                    x.Balance,
                    x.BalanceIncome,
                    x.BalanceOut,
                    x.CloseDate,
                    x.CreditLimit,
                    x.IsSpecial,
                    x.LastOperationDate,
                    x.Number,
                    x.OpenDate,
                    x.RealityObject,
                    x.RegOperator,
                    x.TotalIncome,
                    x.TotalOut,
                    ContragentBankCrOrg = x.ContragentBankCrOrg != null ? new
                    {
                        x.ContragentBankCrOrg.Id,
                        Name = x.ContragentBankCrOrg.CreditOrg.Name ?? x.ContragentBankCrOrg.Name,
                        Bik = x.ContragentBankCrOrg.CreditOrg.Bik ?? x.ContragentBankCrOrg.Bik,
                        CorrAccount = x.ContragentBankCrOrg.CreditOrg.CorrAccount ?? x.ContragentBankCrOrg.CorrAccount,
                        Okpo = x.ContragentBankCrOrg.CreditOrg.Okpo ?? x.ContragentBankCrOrg.Okpo,
                        x.ContragentBankCrOrg.SettlementAccount
                    } : null
                }).FirstOrDefault();

            return new BaseDataResult(rec);
        }
    }
}