namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class RevenueSourceGjiViewModel : BaseViewModel<RevenueSourceGji>
    {
        public override IDataResult List(IDomainService<RevenueSourceGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                }).OrderBy(x=> x.Code)
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}