namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class PlanReductionExpenseViewModel : BaseViewModel<PlanReductionExpense>
    {
        public override IDataResult List(IDomainService<PlanReductionExpense> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var data = domainService.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                .Select(x => new
                {
                    x.Id,
                    x.BaseService.TemplateService.Name 
                }).Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<PlanReductionExpense> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

            if (obj != null && obj.BaseService != null && obj.BaseService.TemplateService != null)
            {
                    return new BaseDataResult(new
                                                  {
                                                      obj.Id,
                                                      obj.ObjectEditDate,
                                                      obj.ObjectCreateDate,
                                                      obj.ObjectVersion,
                                                      obj.ExternalId,
                                                      obj.DisclosureInfoRealityObj,
                                                      obj.BaseService,
                                                      obj.BaseService.TemplateService.Name
                                                  });
            }

            return new BaseDataResult(obj);
        }
    }
}