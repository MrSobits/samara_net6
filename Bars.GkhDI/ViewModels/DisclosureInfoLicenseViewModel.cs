namespace Bars.GkhDi.ViewModels
{
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    class DisclosureInfoLicenseViewModel : BaseViewModel<DisclosureInfoLicense>
    {
        public override IDataResult List(IDomainService<DisclosureInfoLicense> domainService, BaseParams baseParams)
        {
            var dInfoId = baseParams.Params.GetAsId("disclosureInfoId");
            var loadParams = GetLoadParam(baseParams);
            var data = domainService.GetAll().Where(x => x.DisclosureInfo.Id == dInfoId).Filter(loadParams, Container);
            int totalCount = data.Count();
            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
