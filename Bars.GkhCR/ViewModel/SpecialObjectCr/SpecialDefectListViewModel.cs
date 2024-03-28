namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Entities;

    using Gkh.Domain;
    
    public class SpecialDefectListViewModel : BaseViewModel<SpecialDefectList>
    {
        public override IDataResult List(IDomainService<SpecialDefectList> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var objectCrId = baseParams.Params.ContainsKey("objectCrId")
                ? baseParams.Params.GetAsId("objectCrId")
                : loadParams.Filter.GetAsId("objectCrId");
            var twId = baseParams.Params.GetAsId("twId");

            var data =
                domainService.GetAll()
                    .Where(x => x.ObjectCr.Id == objectCrId)
                    .WhereIf(twId > 0, x => x.TypeWork.Id == twId)
                    .Select(
                        x => new
                        {
                            x.Id,
                            WorkName = x.Work.Name,
                            x.DocumentName,
                            x.DocumentDate,
                            x.State,
                            x.File,
                            x.Volume,
                            x.Sum,
                            x.TypeDefectList
                        })
                    .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}