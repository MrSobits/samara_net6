namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Domain;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Contracts.Params;
    using Bars.Gkh.Utils;

    using Entities;

    public class FinanceSourceResourceViewModel : BaseViewModel<FinanceSourceResource>
    {
        public IOverhaulViewModels OverhaulViewModels { get; set; }

        public override IDataResult List(IDomainService<FinanceSourceResource> domainService, BaseParams baseParams)
        {
            var formFinanceSource = Container.GetGkhConfig<GkhCrConfig>().General.FormFinanceSource;

            if (OverhaulViewModels != null && (formFinanceSource == FormFinanceSource.WithTypeWork))
            {
                return OverhaulViewModels.FinanceSourceResList(domainService, baseParams);
            }

            var loadParams = GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAsId("objectCrId");
            }

            var data =
                domainService.GetAll()
                             .Where(x => x.ObjectCr.Id == objectCrId)
                             .Select(
                                 x =>
                                 new FinanceSourceResourceProxy
                                     {
                                         Id = x.Id,
                                         FinanceSourceName = x.FinanceSource.Name,
                                         GroupField = x.FinanceSource.Name,
                                         BudgetMu = x.BudgetMu,
                                         BudgetSubject = x.BudgetSubject,
                                         OwnerResource = x.OwnerResource,
                                         FundResource = x.FundResource,
                                         BudgetMuIncome = x.BudgetMuIncome,
                                         BudgetSubjectIncome = x.BudgetSubjectIncome,
                                         FundResourceIncome = x.FundResourceIncome,
                                         Year = x.Year,
                                         TypeWorkCr = x.TypeWorkCr.Work.Name,
                                         TypeWorkCrId = (long?)x.TypeWorkCr.Id,
                                         BudgetMuPercent = x.BudgetMu > 0 ? x.BudgetMuIncome / x.BudgetMu * 100 : 0,
                                         BudgetSubjectPercent =
                                     x.BudgetSubject > 0 ? x.BudgetSubjectIncome / x.BudgetSubject * 100 : 0,
                                         FundResourcePercent =
                                     x.FundResource > 0 ? x.FundResourceIncome / x.FundResource * 100 : 0,
                                     })
                             .Filter(loadParams, Container)
                             .OrderBy(x => x.GroupField);

            var totalCount = data.Count();

            return new GenericListResult<FinanceSourceResourceProxy>(data.Order(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<FinanceSourceResource> domainService, BaseParams baseParams)
        {
            var formFinanceSource = Container.GetGkhConfig<GkhCrConfig>().General.FormFinanceSource;

            if (OverhaulViewModels != null && formFinanceSource == FormFinanceSource.WithTypeWork)
            {
                return OverhaulViewModels.FinanceSourceResGet(domainService, baseParams);
            }

            return base.Get(domainService, baseParams);
        }
    }

    public class FinanceSourceResourceProxy
    {
        public long Id { get; set; }

        public long? FinanceSourceId { get; set; }

        public string FinanceSourceName { get; set; }

        public string GroupField { get; set; }

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
