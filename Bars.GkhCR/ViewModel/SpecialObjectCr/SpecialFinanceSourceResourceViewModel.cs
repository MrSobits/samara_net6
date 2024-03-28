namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Contracts.Params;

    using Entities;

    public class SpecialFinanceSourceResourceViewModel : BaseViewModel<SpecialFinanceSourceResource>
    {
        public override IDataResult List(IDomainService<SpecialFinanceSourceResource> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAsId("objectCrId");
            }

            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .Select(x => new SpecialFinanceSourceResourceProxy
                {
                    Id = x.Id,
                    FinanceSourceName = x.FinanceSource.Name,
                    GroupField = x.Year,
                    BudgetMu = x.BudgetMu,
                    BudgetSubject = x.BudgetSubject,
                    OwnerResource = x.OwnerResource,
                    FundResource = x.FundResource,
                    BudgetMuIncome = x.BudgetMuIncome,
                    BudgetSubjectIncome = x.BudgetSubjectIncome,
                    FundResourceIncome = x.FundResourceIncome,
                    Year = x.Year,
                    TypeWorkCr = x.TypeWorkCr.Work.Name,
                    TypeWorkCrId = x.TypeWorkCr.Id,
                    BudgetMuPercent = x.BudgetMu > 0 ? x.BudgetMuIncome / x.BudgetMu * 100 : 0,
                    BudgetSubjectPercent =
                x.BudgetSubject > 0 ? x.BudgetSubjectIncome / x.BudgetSubject * 100 : 0,
                    FundResourcePercent =
                x.FundResource > 0 ? x.FundResourceIncome / x.FundResource * 100 : 0,
                })
                .Filter(loadParams, this.Container)
                .OrderBy(x => x.GroupField);

            var totalCount = data.Count();

            return new GenericListResult<SpecialFinanceSourceResourceProxy>(data.Order(loadParams).ToList(), totalCount);
        }
    }

    public class SpecialFinanceSourceResourceProxy
    {
        public long Id { get; set; }

        public long? FinanceSourceId { get; set; }

        public string FinanceSourceName { get; set; }

        public int? GroupField { get; set; }

        public long? TypeWorkCrId { get; set; }

        public decimal? BudgetMu { get; set; }

        public decimal? BudgetMuPercent { get; set; }

        public decimal? BudgetSubjectPercent { get; set; }

        public decimal? FundResourcePercent { get; set; }

        public decimal? BudgetSubject { get; set; }

        public decimal? OwnerResource { get; set; }

        public decimal? FundResource { get; set; }

        public decimal? BudgetMuIncome { get; set; }

        public decimal? BudgetSubjectIncome { get; set; }

        public decimal? FundResourceIncome { get; set; }

        public decimal? OtherResource { get; set; }

        public string TypeWorkCr { get; set; }

        public int? Year { get; set; }

        public bool Bool { get; set; }
    }
}
