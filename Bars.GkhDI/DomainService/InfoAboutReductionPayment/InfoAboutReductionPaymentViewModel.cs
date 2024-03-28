namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;
    using Entities;

    public class InfoAboutReductionPaymentViewModel : BaseViewModel<InfoAboutReductionPayment>
    {
        public override IDataResult List(IDomainService<InfoAboutReductionPayment> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
            var data = domainService.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                .Select(x => new
                {
                    x.Id,
                    BaseServiceName = x.BaseService.TemplateService.Name,
                    x.BaseService.TemplateService.TypeGroupServiceDi,
                    x.ReasonReduction,
                    x.RecalculationSum,
                    x.OrderDate,
                    x.OrderNum,
                    x.Description
                }).Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}