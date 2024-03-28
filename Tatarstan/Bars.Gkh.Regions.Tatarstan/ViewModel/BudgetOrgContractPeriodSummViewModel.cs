namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Представление <see cref="BudgetOrgContractPeriodSumm"/>
    /// </summary>
    public class BudgetOrgContractPeriodSummViewModel : BaseViewModel<BudgetOrgContractPeriodSumm>
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<BudgetOrgContractPeriodSumm> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var contragentIds = this.UserManager.GetContragentIds();

            var periodId = baseParams.Params.GetAsId("periodId");
            var serviceIds = baseParams.Params.GetAs<long[]>("serviceIds");
            var municipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");
            var orgIds = baseParams.Params.GetAs<long[]>("orgIds");
            var pubServOrgIds = baseParams.Params.GetAs<long[]>("pubServOrgIds");

            var query = domainService.GetAll()
                .Where(x => x.ContractPeriod.Id == periodId)
                .WhereIf(contragentIds.IsNotEmpty(), x => contragentIds.Contains(x.PublicServiceOrg.Contragent.Id))
                .WhereIf(serviceIds.IsNotEmpty(), x => serviceIds.Contains(x.ContractService.Service.Id))
                .WhereIf(municipalityIds.IsNotEmpty(), x => municipalityIds.Contains(x.Municipality.Id))
                .WhereIf(orgIds.IsNotEmpty(), x => orgIds.Contains(x.BudgetOrgContract.Organization.Id))
                .WhereIf(pubServOrgIds.IsNotEmpty(), x => pubServOrgIds.Contains(x.PublicServiceOrg.Id))
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    TypeCustomer = x.BudgetOrgContract.TypeCustomer.Name,
                    Service = x.ContractService.Service.Name,
                    Organization = x.BudgetOrgContract.Organization.Name,
                    PublicServiceOrg = x.PublicServiceOrg.Contragent.Name,
                    x.Charged,
                    x.Paid,
                    x.EndDebt
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToList(), query.Count());
        }
    }
}