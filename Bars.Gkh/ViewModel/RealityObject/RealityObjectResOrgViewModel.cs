namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class RealityObjectResOrgViewModel : BaseViewModel<RealityObjectResOrg>
    {
        public override IDataResult List(IDomainService<RealityObjectResOrg> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");
            var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");
            var fromContract = baseParams.Params.GetAs<bool>("fromContract");

            if (objectId > 0 && !fromContract)
            {
                var data1 = domainService.GetAll()
                    .Where(x => x.RealityObject.Id == objectId)
                    .Select(x => new
                    {
                        x.Id,
                        ResourceOrg = x.ResourceOrg.Contragent.Name,
                        ResourceOrgId = x.ResourceOrg.Id,
                        x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.DateStart,
                        x.DateEnd,
                        x.ContractNumber,
                        x.ContractDate,
                        x.FileInfo,
                        x.Note
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data1.Order(loadParams).Paging(loadParams).ToList(), data1.Count());
            }

            var data = domainService.GetAll()
                .WhereIf(supplyResOrgId > 0, x => x.ResourceOrg.Id == supplyResOrgId)
                .Select(x => new
                    {
                        x.Id,
                        ResourceOrg = x.ResourceOrg.Contragent.Name,
                        x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.DateStart,
                        x.DateEnd,
                        x.ContractNumber,
                        x.ContractDate,
                        x.FileInfo,
                        x.Note
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<RealityObjectResOrg> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params["id"].ToInt();

            return new BaseDataResult(domainService.GetAll().Where(x => x.Id == id)
                .Select(x => new
                    {
                        x.Id,
                        RealityObject = new { x.RealityObject.Id, x.RealityObject.Address },
                        ResourceOrg = new { x.ResourceOrg.Id, ContragentName = x.ResourceOrg.Contragent.Name },
                        Municipality = x.RealityObject.Municipality.Name,
                        x.DateStart,
                        x.DateEnd,
                        x.ContractNumber,
                        x.ContractDate,
                        x.FileInfo,
                        x.Note
                    })
                .First());
        }
    }
}