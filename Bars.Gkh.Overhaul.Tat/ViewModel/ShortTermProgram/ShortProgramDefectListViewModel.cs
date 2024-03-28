namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;
    using B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class ShortProgramDefectListViewModel : BaseViewModel<ShortProgramDefectList>
    {
        public override IDataResult List(IDomainService<ShortProgramDefectList> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .Where(x => x.ShortObject.Id == objectId)
                .Select(x => new
                    {
                        x.Id,
                        Work = x.Work.Name,
                        x.DocumentName,
                        x.DocumentDate,
                        x.State,
                        x.File,
                        x.Sum
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}