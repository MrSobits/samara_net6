namespace Bars.Gkh.ViewModel.ManOrg
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities.ManOrg;

    public class ManagingOrgRegistryViewModel: BaseViewModel<ManagingOrgRegistry>
    {
        public override IDataResult List(IDomainService<ManagingOrgRegistry> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var manorgId = baseParams.Params.GetAs<long>("manorgId");

            var data = domain.GetAll()
                .Where(x => x.ManagingOrganization.Id == manorgId)
                .Select(x => new
                {
                    x.Id,
                    Doc = x.Doc != null ? x.Doc.Name : string.Empty,
                    EgrulDate = x.EgrulDate.GetValueOrDefault(),
                    x.InfoType,
                    x.InfoDate,
                    x.RegNumber
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            var result = data.Order(loadParams).Paging(loadParams)
                .Select(x => new
                {
                    x.Id,
                    x.Doc,
                    EgrulDate = (x.EgrulDate <= DateTime.MinValue) ? string.Empty : x.EgrulDate.ToString("dd.MM.yyyy"),
                    x.InfoType,
                    x.InfoDate,
                    x.RegNumber
                });

            return new ListDataResult(result.ToList(), totalCount);
        }
    }
}
