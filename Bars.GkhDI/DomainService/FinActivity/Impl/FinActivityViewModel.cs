namespace Bars.GkhDi.DomainService
{
    using B4;

    using Entities;

    public class FinActivityViewModel : BaseViewModel<FinActivity>
    {
        public override IDataResult Get(IDomainService<FinActivity> domainService, BaseParams baseParams)
        {
            var value = domainService.Get(baseParams.Params.GetAs<long>("id"));

            return new BaseDataResult(new
                                          {
                                              value.Id,
                                              DisclosureInfo = value.DisclosureInfo != null && value.DisclosureInfo.ManagingOrganization != null ? 
                                                                    new { value.DisclosureInfo.Id, value.DisclosureInfo.ManagingOrganization.TypeManagement } : null,
                                              value.TaxSystem,
                                              value.ValueBlankActive,
                                              value.Description,
                                              value.ClaimDamage,
                                              value.FailureService,
                                              value.NonDeliveryService
                                          });
        }
    }
}
