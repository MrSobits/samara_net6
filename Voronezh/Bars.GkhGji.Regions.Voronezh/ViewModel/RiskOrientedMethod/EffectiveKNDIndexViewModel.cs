namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class EffectiveKNDIndexViewModel : BaseViewModel<EffectiveKNDIndex>
    {
        public override IDataResult List(IDomainService<EffectiveKNDIndex> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.KindKND,
                        x.YearEnums,
                        x.Code,
                        x.Name,
                        x.TargetIndex,
                        x.CurrentIndex
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}