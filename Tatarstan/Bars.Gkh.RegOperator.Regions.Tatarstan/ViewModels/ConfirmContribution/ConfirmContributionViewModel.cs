namespace Bars.Gkh.RegOperator.Regions.Tatarstan.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    /// <summary>
    /// Вьюмодель для "Учет платежных документов по начислениям и оплатам на КР"
    /// </summary>
    public class ConfirmContributionViewModel : BaseViewModel<ConfirmContribution>
    {
        public override IDataResult List(IDomainService<ConfirmContribution> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    MunicipalityName = x.ManagingOrganization.Contragent.Municipality.Name,
                    ManagingOrganizationName = x.ManagingOrganization.Contragent.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<ConfirmContribution> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var confContrib = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    MunicipalityName = x.ManagingOrganization.Contragent.Municipality.Name,
                    ManagingOrganization = x.ManagingOrganization.Contragent.Name
                });

            return new BaseDataResult(confContrib);
        }
    }
}