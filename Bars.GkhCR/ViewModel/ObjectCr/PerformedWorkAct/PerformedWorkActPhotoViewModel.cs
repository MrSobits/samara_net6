namespace Bars.GkhCr.DomainService
{
    using B4;
    using Entities;
    using Gkh.Domain;
    using System.Linq;

    public class PerformedWorkActPhotoViewModel : BaseViewModel<PerformedWorkActPhoto>
    {
        public override IDataResult List(IDomainService<PerformedWorkActPhoto> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var perfWorkActId = baseParams.Params.GetAsId("performedWorkActId");

            if (perfWorkActId == 0)
            {
                perfWorkActId = loadParams.Filter.GetAsId("performedWorkActId");
            }

            var data = domainService.GetAll()
                .Where(x => x.PerformedWorkAct.Id == perfWorkActId)
                .Select(x => new
                {
                    x.Id,
                    x.Photo,
                    x.PhotoType,
                    x.Name,
                    x.Discription
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}