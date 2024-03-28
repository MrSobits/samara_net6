namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Entities;

    public class ResolutionPayFineViewModel : ResolutionPayFineViewModel<ResolutionPayFine>
    {
    }

    public class ResolutionPayFineViewModel<T> : BaseViewModel<T>
        where T : ResolutionPayFine
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            /*
             * параметр documentId означает что нужно отдать оплаты, которые ссылаются на этот документ ГЖИ
             */

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.TypeDocumentPaid,
                    x.Amount,
                    x.GisUip
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}