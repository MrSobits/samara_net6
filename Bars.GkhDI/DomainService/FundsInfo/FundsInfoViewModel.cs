namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;
    using Entities;

    public class FundsInfoViewModel : BaseViewModel<FundsInfo>
    {
        public override IDataResult List(IDomainService<FundsInfo> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var data = domainService.GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    x.DocumentDate,
                    x.Size
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}