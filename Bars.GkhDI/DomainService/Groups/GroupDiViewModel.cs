namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class GroupDiViewModel : BaseViewModel<GroupDi>
    {
        public override IDataResult List(IDomainService<GroupDi> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var serviceRealityObjGroup = this.Container.Resolve<IDomainService<RealityObjGroup>>();

            var data = domainService
                .GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    CountObjects = serviceRealityObjGroup.GetAll().Count(y => y.GroupDi.Id == x.Id)
                });

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}