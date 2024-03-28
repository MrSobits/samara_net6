namespace Bars.GkhGji.Regions.Smolensk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckRealityObjectViewModel : BaseViewModel<ActCheckRealityObject>
    {
        public override IDataResult List(IDomainService<ActCheckRealityObject> domainService, BaseParams baseParams)
        {
            var serviceViolations = Container.Resolve<IDomainService<ActCheckViolation>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var documentId = baseParams.Params.GetAs("documentId", 0L);

                var dictViolations = serviceViolations.GetAll()
                   .Where(x => x.ActObject.ActCheck.Id == documentId)
                   .GroupBy(x => x.ActObject.Id)
                   .AsEnumerable()
                   .ToDictionary(x => x.Key, y => y.Count());

                var data = domainService.GetAll()
                .Where(x => x.ActCheck.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    IsRealityObject = x.RealityObject != null,
                    RealityObjectId = x.RealityObject != null ? x.RealityObject.Id : 0,
                    RealityObject = x.RealityObject != null ? x.RealityObject.Address : null,
                    Municipality = x.RealityObject != null ? x.RealityObject.Municipality.Name : null,
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
                    x.IsRealityObject,
                    x.RealityObject,
                    x.Municipality,
                    x.HaveViolation,
                    x.Description,
                    ViolationCount = dictViolations.ContainsKey(x.Id) ? dictViolations[x.Id] : 0,
                    x.NotRevealedViolations
                })
                .AsQueryable()
                .Filter(loadParam, Container);

                int totalCount = data.Any() ? data.Count() : 0;

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);

            }
            finally 
            {
                Container.Release(serviceViolations);
            }
        }

        public override IDataResult Get(IDomainService<ActCheckRealityObject> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                new
                {
                    obj.Id,
                    obj.ActCheck,
                    IsRealityObject = obj.RealityObject != null,
                    obj.RealityObject,
                    obj.Description
 
                }) : new BaseDataResult();
        }
    }
}