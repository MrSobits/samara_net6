namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;

    public class MKDLicTypeRequestViewModel : BaseViewModel<MKDLicTypeRequest>
    {
        public override IDataResult List(IDomainService<MKDLicTypeRequest> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                  x.Id,
                  x.Code,
                  x.Name,
                  x.Description
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}