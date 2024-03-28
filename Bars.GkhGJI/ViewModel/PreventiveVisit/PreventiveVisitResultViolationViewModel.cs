namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    public class PreventiveVisitResultViolationViewModel : BaseViewModel<PreventiveVisitResultViolation>
    {
        public override IDataResult List(IDomainService<PreventiveVisitResultViolation> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("resultId")
                                   ? baseParams.Params["resultId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.PreventiveVisitResult.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    ViolationGjiName = x.ViolationGji.Name,
                    Pprf = x.ViolationGji.NormativeDocNames,
                    x.ViolationGji.CodePin
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}