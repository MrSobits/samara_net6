namespace Bars.Gkh.ViewModel
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using System.Linq;

    using B4;

    public class LawsuitClwCourtViewModel : BaseViewModel<LawsuitClwCourt>
    {
        public override IDataResult List(IDomainService<LawsuitClwCourt> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var docId = loadParams.Filter.GetAs("docId", 0L);

            var data = domain.GetAll()
                .Where(x => x.DocumentClw.Id == docId)
                .Select(x => new
                {
                    x.Id,
                    x.DocNumber,
                    x.DocDate,
                    x.LawsuitCourtType,
                    x.File,
                    x.PretensionType,
                    x.PretensionReciever,
                    x.PretensionDate,
                    x.PretensionResult,
                    x.PretensionReviewDate,
                    x.PretensionNote
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}