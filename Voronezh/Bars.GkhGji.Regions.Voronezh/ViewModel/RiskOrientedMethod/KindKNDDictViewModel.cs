namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class KindKNDDictViewModel : BaseViewModel<KindKNDDict>
    {
        public override IDataResult List(IDomainService<KindKNDDict> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.KindKND,
                        x.DateFrom,
                        x.DateTo
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}