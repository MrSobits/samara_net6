namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActIsolatedRealObjViewModel : BaseViewModel<ActIsolatedRealObj>
    {
        public override IDataResult List(IDomainService<ActIsolatedRealObj> domainService, BaseParams baseParams)
        {
            var serviceViolations = this.Container.Resolve<IDomainService<ActIsolatedRealObjViolation>>();

            try
            {
                var loadParam = this.GetLoadParam(baseParams);
                var documentId = baseParams.Params.GetAs<long>("documentId");

                var dictViolations = serviceViolations.GetAll()
                    .Where(x => x.ActIsolatedRealObj.ActIsolated.Id == documentId)
                    .AsEnumerable()
                    .GroupBy(x => x.ActIsolatedRealObj.Id)
                    .ToDictionary(x => x.Key, y => y.Count());

                var data = domainService.GetAll()
                    .Where(x => x.ActIsolated.Id == documentId)
                    .Select(x => new
                    {
                        x.Id,
                        RealityObject = x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.HaveViolation,
                        x.Description,
                    })
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.RealityObject,
                        x.Municipality,
                        x.HaveViolation,
                        x.Description,
                        ViolationCount = dictViolations.ContainsKey(x.Id) ? dictViolations[x.Id] : 0,
                    })
                    .AsQueryable()
                    .Filter(loadParam, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(serviceViolations);
            }
        }
    }
}