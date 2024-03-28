namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectImageViewModel : BaseViewModel<RealityObjectImage>
    {
        public override IDataResult List(IDomainService<RealityObjectImage> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.DateImage,
                    x.Name,
                    WorkCrName = x.WorkCr.Name,
                    PeriodName = x.Period.Name,
                    x.ImagesGroup,
                    x.Description,
                    x.Period,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}