namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    internal class InspectionBaseTypeViewModel : BaseViewModel<InspectionBaseType>
    {
        public override IDataResult List(IDomainService<InspectionBaseType> domainService, BaseParams baseParams)
        {
            var kindCheckId = baseParams.Params.GetAsId("kindCheckId");
            var openValuesForErknm = baseParams.Params.GetAs<bool>("openValuesForErknm");
            var loadParams = baseParams.GetLoadParam();
            var kindCheckDomain = this.Container.ResolveDomain<KindCheckGji>();
            var inspectionBaseTypeKindCheckDomain = this.Container.ResolveDomain<InspectionBaseTypeKindCheck>();

            using (this.Container.Using(kindCheckDomain, inspectionBaseTypeKindCheckDomain))
            {
                var kindCheckCode = kindCheckDomain.Get(kindCheckId)?.Code;
                var scheduledInspectionNames = new[] { TypeCheck.PlannedExit, TypeCheck.PlannedDocumentation };
                var unscheduledInspectionNames = new[] { TypeCheck.NotPlannedExit, TypeCheck.NotPlannedDocumentation, TypeCheck.InspectionSurvey };

                if (openValuesForErknm)
                {
                    var inspectionBaseTypeQuery = domainService.GetAll()
                        .Where(x => x.ErknmCode != null && x.ErknmCode != string.Empty);
                    var kindCheckLookUp = inspectionBaseTypeKindCheckDomain.GetAll()
                        .Where(x => inspectionBaseTypeQuery.Any(y => y.Id == x.InspectionBaseType.Id))
                        .ToLookup(x => x.InspectionBaseType.Id, x => x.KindCheck);

                    return inspectionBaseTypeQuery
                        .Select(x => new
                        {
                            x.Id,
                            x.ErknmCode,
                            x.Name,
                            x.HasTextField,
                            x.HasDate,
                            x.HasRiskIndicator
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Id,
                            x.ErknmCode,
                            x.Name,
                            x.HasTextField,
                            x.HasDate,
                            x.HasRiskIndicator,
                            KindCheck = kindCheckLookUp[x.Id]
                        })
                        .ToListDataResult(loadParams, this.Container);
                }

                return domainService.GetAll()
                    .Where(x => x.ErknmCode == null || x.ErknmCode == string.Empty)
                    .WhereIf(kindCheckCode.HasValue && scheduledInspectionNames.Contains(kindCheckCode.Value),
                        x => x.InspectionKindId == InspectionKind.ScheduledInspection)
                    .WhereIf(kindCheckCode.HasValue && unscheduledInspectionNames.Contains(kindCheckCode.Value),
                        x => x.InspectionKindId == InspectionKind.UnscheduledInspection)
                    .ToListDataResult(loadParams, this.Container);
            }
        }
    }
}