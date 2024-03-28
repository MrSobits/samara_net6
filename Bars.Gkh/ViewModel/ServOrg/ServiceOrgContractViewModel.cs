namespace Bars.Gkh.ViewModel
{
    using B4;
    using B4.Utils;
    using Entities;
    using System.Linq;
    
    public class ServiceOrgContractViewModel : BaseViewModel<ServiceOrgContract>
    {
        public override IDataResult List(IDomainService<ServiceOrgContract> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var servorgId = baseParams.Params.GetAs<long>("servorgId");
            var objectId = baseParams.Params.GetAs<long>("objectId");

            if (servorgId != 0 && objectId != 0) objectId = 0;

            var servorgRoContractService = Container.Resolve<IDomainService<ServiceOrgRealityObjectContract>>();

            var data = servorgRoContractService.GetAll()
                .WhereIf(servorgId != 0, x => x.ServOrgContract.ServOrg.Id == servorgId)
                .WhereIf(objectId != 0, x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.ServOrgContract.Id,
                    ServOrgId = x.ServOrgContract.ServOrg.Id,
                    x.RealityObject.Address,
                    x.ServOrgContract.FileInfo,
                    ContragentName = x.ServOrgContract.ServOrg.Contragent.Name,
                    x.ServOrgContract.DateStart,
                    x.ServOrgContract.DateEnd,
                    RealityObjectId = x.RealityObject.Id
                });

            int totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            Container.Release(servorgRoContractService);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}