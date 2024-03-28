namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    public class ActIsolatedRealObjViolationViewModel : BaseViewModel<ActIsolatedRealObjViolation>
    {
        public override IDataResult List(IDomainService<ActIsolatedRealObjViolation> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .WhereIf(objectId > 0, x => x.ActIsolatedRealObj.Id == objectId)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.EventResult,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    ViolationGjiName = x.InspectionViolation.Violation.Name,
                    DatePlanRemoval = x.DatePlanRemoval.HasValue ? x.DatePlanRemoval : x.InspectionViolation.DatePlanRemoval,
                    x.InspectionViolation.DateFactRemoval,
                    x.InspectionViolation.DateCancel,
                    ViolationGjiId = x.InspectionViolation.Violation.Id
                })
                .AsQueryable()
                .Filter(loadParam, this.Container);

            return new ListDataResult(data.ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<ActIsolatedRealObjViolation> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());
            var violationGjiDomain = this.Container.Resolve<IDomainService<ViolationGji>>();

            try
            {
                if (obj.InspectionViolation != null)
                {
                    var violation = violationGjiDomain.Load(obj.InspectionViolation.Violation.Id);
                    obj.InspectionViolation.ViolationGji = violation.Name;
                }

                return new BaseDataResult(obj);
            }
            finally
            {
                this.Container.Release(violationGjiDomain);
            }
        }
    }
}