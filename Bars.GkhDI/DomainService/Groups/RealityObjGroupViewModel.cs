namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class RealityObjGroupViewModel : BaseViewModel<RealityObjGroup>
    {
        public override IDataResult List(IDomainService<RealityObjGroup> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var groupDiId = baseParams.Params.GetAs<long>("groupDiId");

            var data = domainService
                .GetAll()
                .Where(x => x.GroupDi.Id == groupDiId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.FiasAddress.AddressName
                });     

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}