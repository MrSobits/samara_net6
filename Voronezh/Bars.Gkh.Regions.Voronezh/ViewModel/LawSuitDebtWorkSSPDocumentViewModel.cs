namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using System.Linq;

    using B4;
    using Bars.Gkh.Regions.Voronezh.Entities;

    public class LawSuitDebtWorkSSPDocumentViewModel : BaseViewModel<LawSuitDebtWorkSSPDocument>
    {
        public override IDataResult List(IDomainService<LawSuitDebtWorkSSPDocument> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var docId = loadParams.Filter.GetAs("docId", 0L);

            var data = domain.GetAll()
                .Where(x => x.LawSuitDebtWorkSSP.Id == docId)
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.NumberString,
                    Rosp = x.Rosp.Name,
                    x.TypeLawsuitDocument,
                    x.Date,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}