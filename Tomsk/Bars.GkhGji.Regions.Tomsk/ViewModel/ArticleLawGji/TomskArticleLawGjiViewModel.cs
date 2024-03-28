namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class TomskArticleLawGjiViewModel : BaseViewModel<TomskArticleLawGji>
    {
        public override IDataResult List(IDomainService<TomskArticleLawGji> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            List<long> ids = null;

            if (baseParams.Params.ContainsKey("Id"))
            {
                ids = baseParams.Params.GetAs("Id", string.Empty).Split(',').Select(x => x.ToLong()).ToList();
            }

            var data = domainService.GetAll()
                .WhereIf(ids != null, x => ids.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.Description,
                    x.Part,
                    x.Article,
                    x.PhysPersonPenalty,
                    x.JurPersonPenalty
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}