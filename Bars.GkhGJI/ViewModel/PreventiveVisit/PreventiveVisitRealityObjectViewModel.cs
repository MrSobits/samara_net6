namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    public class PreventiveVisitRealityObjectViewModel : BaseViewModel<PreventiveVisitRealityObject>
    {
        public override IDataResult List(IDomainService<PreventiveVisitRealityObject> domainService, BaseParams baseParams)
        {
            var servicePreventiveVisit = Container.Resolve<IDomainService<PreventiveVisit>>();
            var serviceRealityObject = Container.Resolve<IDomainService<PreventiveVisitRealityObject>>();


            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;
            var data = serviceRealityObject.GetAll()
                .Where(x => x.PreventiveVisit.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address,
                    Municipality = x.RealityObject.Municipality.Name
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}