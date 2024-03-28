namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class KindKNDDictArtLawViewModel : BaseViewModel<KindKNDDictArtLaw>
    {

        public override IDataResult List(IDomainService<KindKNDDictArtLaw> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var parentId = loadParams.Filter.GetAs("parentId", 0L);
            object value;
        
            var data = domainService.GetAll()
                .Where(x => x.KindKNDDict.Id == parentId)
                .Select(x => new
                {
                    x.Id,
                    x.ArticleLawGji.Name,
                    x.Koefficients
                }).Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}