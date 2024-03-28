namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;
    using System.Collections.Generic;

    public class PreventiveVisitResultViewModel : BaseViewModel<PreventiveVisitResult>
    {
        public override IDataResult List(IDomainService<PreventiveVisitResult> domainService, BaseParams baseParams)
        {
            var violService = this.Container.Resolve<IDomainService<PreventiveVisitResultViolation>>();

            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            Dictionary<long, int> violDict = new Dictionary<long, int>();
            if (documentId > 0)
            {
                violService.GetAll()
                    .Where(x => x.PreventiveVisitResult.PreventiveVisit.Id == documentId)
                    .Select(x => x.PreventiveVisitResult.Id).ToList().ForEach(x =>
                    {
                        if (!violDict.ContainsKey(x))
                        {
                            violDict.Add(x, 1);
                        }
                        else
                        {
                            violDict[x] += 1;
                        }
                    });
            }

            var data = domainService.GetAll()
                .Where(x => x.PreventiveVisit.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.ProfVisitResult,
                    Address = x.RealityObject != null? x.RealityObject.Address:"",
                    Municipality = x.RealityObject != null ? x.RealityObject.Municipality.Name : "",
                    ViolCount = violDict.ContainsKey(x.Id)? violDict[x.Id]:0
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}