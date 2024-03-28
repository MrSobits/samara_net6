namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <inheritdoc />
    public class WarningDocViolationsViewModel : BaseViewModel<WarningDocViolations>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<WarningDocViolations> domainService, BaseParams baseParams)
        {
            var warningDocId = baseParams.Params.GetAsId("documentId");

            return domainService.GetAll()
                .Where(x => x.WarningDoc.Id == warningDocId)
                .Select(x => new
                {
                    x.Id,
                    RealityObject = x.RealityObject.Address,
                    NormativeDoc = x.NormativeDoc.Name,
                    Violations = x.ViolationList.Select(y => y.ViolationGji.Name),
                    x.Description,
                    x.TakenMeasures,
                    x.DateSolution
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    x.NormativeDoc,
                    Violations = x.Violations.AggregateWithSeparator(", "),
                    x.Description,
                    x.TakenMeasures,
                    x.DateSolution
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<WarningDocViolations> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var result = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    RealityObject = new { RealityObjectId = x.RealityObject.Id, x.RealityObject.Address },
                    NormativeDoc = x.NormativeDoc != null ? new { x.NormativeDoc.Id, x.NormativeDoc.Name } : null,
                    Violations = x.ViolationList.Select(y => new { y.ViolationGji.Id, y.ViolationGji.Name }),
                    x.Description,
                    x.TakenMeasures,
                    x.DateSolution
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    x.NormativeDoc,
                    x.Violations,
                    x.Description,
                    x.TakenMeasures,
                    x.DateSolution
                })
                .FirstOrDefault();

            return new BaseDataResult(result);
        }
    }
}