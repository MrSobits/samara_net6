namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    public class ActCheckRealityObjectViewModel : BaseViewModel<ActCheckRealityObject>
    {
        public override IDataResult List(IDomainService<ActCheckRealityObject> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
            var onlyHasViolations = baseParams.Params.GetAs("onlyHasViolations", false);

            var serviceViolations = Container.Resolve<IDomainService<ActCheckViolation>>();

            try
            {
                var dictViolations = serviceViolations.GetAll()
                    .Where(x => x.ActObject.ActCheck.Id == documentId)
                    .GroupBy(x => x.ActObject.Id)
                    .ToDictionary(x => x.Key, y => y.Count());

                var data = domainService.GetAll()
                    .Where(x => x.ActCheck.Id == documentId)
                    .WhereIf(onlyHasViolations, x => x.HaveViolation == YesNoNotSet.Yes)
                    .Select(x => new
                    {
                        x.Id,
                        RealityObject = x.RealityObject.Address,
                        RealityObjectId = x.RealityObject.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.HaveViolation,
                        x.Description,
                        x.NotRevealedViolations
                    })
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.RealityObject,
                        x.RealityObjectId,
                        x.Municipality,
                        x.HaveViolation,
                        x.Description,
                        ViolationCount = dictViolations.ContainsKey(x.Id) ? dictViolations[x.Id] : 0,
                        x.NotRevealedViolations
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
            }
            finally
            {
                Container.Release(serviceViolations);
            }
        }
    }
}