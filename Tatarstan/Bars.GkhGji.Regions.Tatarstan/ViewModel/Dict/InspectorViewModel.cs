namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class InspectorViewModel : BaseViewModel<Inspector>
    {
        public override IDataResult List(IDomainService<Inspector> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var ids = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();
            var zonalInspectionIds = baseParams.Params.GetAs("zonalInspectionIds", string.Empty).ToLongArray();
            var headOnly = baseParams.Params.GetAs("headOnly", false);
            var notHeadOnly = baseParams.Params.GetAs("notHeadOnly", false);
            var issuerOnly = baseParams.Params.GetAs("issuerOnly", false);
            var memberOnly = baseParams.Params.GetAs("memberOnly", false);
            var excludeInpectorId = baseParams.Params.GetAsId("excludeInpectorId");
            var controlTypeId = baseParams.Params.GetAsId("controlTypeId");

            // Порядок ID используется для сортировки
            var order = ids.Select((x, i) => new { Id = x, Order = i })
                .ToDictionary(x => x.Id, y => y.Order);

            var zonalInspectionInspectorDomain = this.Container.ResolveDomain<ZonalInspectionInspector>();
            var controlTypeInspectorPositionsDomain = this.Container.ResolveDomain<ControlTypeInspectorPositions>();

            using (this.Container.Using(zonalInspectionInspectorDomain, controlTypeInspectorPositionsDomain))
            {
                var zonalInspectionInspectorIds = zonalInspectionIds.Length == 0
                    ? new List<long>()
                    : zonalInspectionInspectorDomain.GetAll()
                        .Where(x => zonalInspectionIds.Contains(x.ZonalInspection.Id))
                        .Select(x => x.Inspector.Id)
                        .ToList();

                var zonalInspectionNamesDict = zonalInspectionInspectorDomain.GetAll()
                    .WhereIf(ids.Length > 0, x => ids.Contains(x.Inspector.Id))
                    .WhereIf(zonalInspectionInspectorIds.Count > 0, x => zonalInspectionInspectorIds.Contains(x.Inspector.Id))
                    .WhereIf(headOnly, x => x.Inspector.IsHead)
                    .WhereIf(notHeadOnly, x => !x.Inspector.IsHead)
                    .WhereIf(excludeInpectorId > 0, x => x.Inspector.Id != excludeInpectorId)
                    .Select(x => new { x.Inspector.Id, x.ZonalInspection.Name })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                            x.Select(y => y.Name)
                                .AsEnumerable()
                                .Aggregate(
                                    "",
                                    (i, j) => i + (!string.IsNullOrEmpty(i) ? ", " + j : j)));

                return (controlTypeId > 0
                        ? controlTypeInspectorPositionsDomain.GetAll()
                            .Where(x => x.ControlType.Id == controlTypeId)
                            .WhereIf(issuerOnly, x => x.IsIssuer)
                            .WhereIf(memberOnly, x => x.IsMember)
                            .Join(domainService.GetAll(),
                                x => x.InspectorPosition.Id,
                                y => y.InspectorPosition.Id,
                                (x, y) => y)
                        : domainService.GetAll())
                    .WhereIf(ids.Length > 0, x => ids.Contains(x.Id))
                    .WhereIf(zonalInspectionInspectorIds.Count > 0, x => zonalInspectionInspectorIds.Contains(x.Id))
                    .WhereIf(headOnly, x => x.IsHead)
                    .WhereIf(notHeadOnly, x => !x.IsHead)
                    .WhereIf(excludeInpectorId > 0, x => x.Id != excludeInpectorId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Fio,
                        x.ShortFio,
                        x.Phone,
                        x.IsHead,
                        x.Description,
                        x.Code,
                        x.InspectorPosition
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Fio,
                        Position = x.InspectorPosition?.Name ?? string.Empty,
                        x.ShortFio,
                        x.Phone,
                        x.IsHead,
                        x.Description,
                        x.Code,
                        ZonalInspection = zonalInspectionNamesDict.Get(x.Id),
                        Order = order.Get(x.Id)
                    })
                    .ToListDataResult(loadParams, this.Container);
            }
        }
    }
}