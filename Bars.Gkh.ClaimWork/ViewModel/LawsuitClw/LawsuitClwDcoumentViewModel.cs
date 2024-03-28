namespace Bars.Gkh.ViewModel
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using System.Linq;

    using B4;

    public class LawsuitClwDocumentViewModel : BaseViewModel<LawsuitClwDocument>
    {
        public override IDataResult List(IDomainService<LawsuitClwDocument> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var docId = loadParams.Filter.GetAs("docId", 0L);

            var data = domain.GetAll()
                .Where(x => x.DocumentClw.Id == docId)
                .Select(x => new
                {
                    x.Id,
                    x.DocName,
                    x.DocNumber,
                    x.DocDate,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}