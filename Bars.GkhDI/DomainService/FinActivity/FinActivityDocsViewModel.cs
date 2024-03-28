namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class FinActivityDocsViewModel : BaseViewModel<FinActivityDocs>
    {
        public override IDataResult List(IDomainService<FinActivityDocs> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var data = domainService
                .GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .Select(x => new
                                 {
                                     x.Id,
                                     x.BookkepingBalance,
                                     x.BookkepingBalanceAnnex
                                 })
                .Filter(loadParams, this.Container);


            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<FinActivityDocs> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params["id"].To<long>());

            return new BaseDataResult(
                new
                {
                    obj.Id,
                    DisclosureInfo = obj.DisclosureInfo.Id,
                    obj.BookkepingBalance,
                    obj.BookkepingBalanceAnnex
                });
        }
    }
}
