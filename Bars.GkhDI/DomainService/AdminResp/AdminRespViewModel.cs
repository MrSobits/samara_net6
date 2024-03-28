namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class AdminRespViewModel : BaseViewModel<AdminResp>
    {
        public override IDataResult List(IDomainService<AdminResp> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var data = domainService.GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .Select(x => new
                    {
                        x.Id,
                        SupervisoryOrgName = x.SupervisoryOrg.Name,
                        x.AmountViolation,
                        x.SumPenalty,
                        x.DatePaymentPenalty,
                        x.DateImpositionPenalty,
                        x.TypeViolation,
                        x.File
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<AdminResp> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                new
                {
                    obj.Id,
                    DisclosureInfo = obj.DisclosureInfo.Id,
                    obj.SupervisoryOrg,
                    obj.AmountViolation,
                    obj.SumPenalty,
                    obj.DatePaymentPenalty,
                    obj.DateImpositionPenalty,
                    obj.File,
                    obj.TypeViolation,
                    obj.DocumentName,
                    obj.DocumentNum,
                    obj.DateFrom,
                    obj.TypePerson,
                    obj.Fio,
                    obj.Position
                }) : new BaseDataResult();
        }
    }
}