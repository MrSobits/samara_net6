namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class PlanWorkServiceRepairViewModel : BaseViewModel<PlanWorkServiceRepair>
    {
        public override IDataResult List(IDomainService<PlanWorkServiceRepair> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var diRealityObjId = baseParams.Params.GetAsId("disclosureInfoRealityObjId");

            var data = domainService.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == diRealityObjId)
                .Select(x => new
                {
                    x.Id,
                    x.BaseService.TemplateService.Name,
                    ProviderName = x.BaseService.Provider.Name
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<PlanWorkServiceRepair> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params["id"].To<long>());

            return new BaseDataResult(new
            {
                obj.Id,
                obj.DisclosureInfoRealityObj,
                obj.BaseService,
                Name =
                    obj.BaseService != null && obj.BaseService.TemplateService != null
                        ? obj.BaseService.TemplateService.Name
                        : string.Empty
            });
        }
    }
}