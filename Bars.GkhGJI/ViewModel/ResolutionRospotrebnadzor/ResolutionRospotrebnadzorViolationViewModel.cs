namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// View Нарушения в постановлении Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorViolationViewModel
        : ResolutionRospotrebnadzorViolationViewModel<ResolutionRospotrebnadzorViolation>
    {
    }

    /// <summary>
    /// Generic View Нарушения в постановлении Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorViolationViewModel<T> : BaseViewModel<T>
        where T: ResolutionRospotrebnadzorViolation
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .WhereIf(documentId > 0, x => x.Resolution.Id == documentId)
                .ToList()
                .Select(x => new
                {
                    Id = x.Id,
                    ViolationGjiPin = x.Violation.InspectionViolation.Violation.CodePin,
                    ViolationGjiName = x.Violation.InspectionViolation.Violation.Name,
                    DatePlanRemoval = x.Violation.DatePlanRemoval.HasValue ? x.Violation.DatePlanRemoval : x.Violation.InspectionViolation.DatePlanRemoval,
                    DateFactRemoval = x.Violation.InspectionViolation.DateFactRemoval,
                    Description = x.Description
                })
                .AsQueryable()
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var violationDomain = this.Container.ResolveDomain<ViolationGji>();
            try
            {
                var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

                if (obj.Violation.InspectionViolation != null)
                {
                    var violation = violationDomain.Load(obj.Violation.InspectionViolation.Violation.Id);

                    obj.Violation.InspectionViolation.ViolationGji = violation.Name;
                }

                return new BaseDataResult(obj);
            }
            finally
            {
                this.Container.Release(violationDomain);
            }
        }
    }
}