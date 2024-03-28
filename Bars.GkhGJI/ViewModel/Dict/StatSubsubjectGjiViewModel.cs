namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class StatSubsubjectGjiViewModel : BaseViewModel<StatSubsubjectGji>
    {
        public override IDataResult List(IDomainService<StatSubsubjectGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.SSTUNameSub,
                    x.ISSOPR,
                    x.SSTUCodeSub,
                    x.TrackAppealCits,
                    x.NeedInSopr
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}