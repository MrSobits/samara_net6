namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для <see cref="VisitSheet"/>
    /// </summary>
    public class VisitSheetService : IVisitSheetService
    {
        #region DependencyInjection
        private readonly IWindsorContainer container;

        public VisitSheetService(IWindsorContainer container)
        {
            this.container = container;
        }
        #endregion

        /// <inheritdoc />
        public IDataResult GetVisitSheetMunicipality(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            var documentGjiChildrenDomain = this.container.ResolveDomain<DocumentGjiChildren>();
            var preventiveActionDomain = this.container.ResolveDomain<PreventiveAction>();

            using (this.container.Using(documentGjiChildrenDomain, preventiveActionDomain))
            {
                try
                {
                    var taskId = documentGjiChildrenDomain
                        .GetAll()
                        .First(x => x.Children.Id == documentId &&
                            x.Parent.TypeDocumentGji == TypeDocumentGji.PreventiveActionTask).Parent.Id;

                    var actionId = documentGjiChildrenDomain.GetAll()
                        .First(x => x.Children.Id == taskId &&
                            x.Parent.TypeDocumentGji == TypeDocumentGji.PreventiveAction).Parent.Id;

                    var action = preventiveActionDomain.Get(actionId);

                    return new BaseDataResult(action.Municipality.Id);
                }
                catch (Exception e)
                {
                    return new BaseDataResult(false, "Не удалось определить муниципальное образование для листа визита");
                }
            }
        }

        /// <inheritdoc />
        public IDataResult GetViolationRealityObjectsList(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            var visitSheetViolationInfoDomain = this.container.ResolveDomain<VisitSheetViolationInfo>();
            var visitSheetViolationDomain = this.container.ResolveDomain<VisitSheetViolation>();

            using (this.container.Using(visitSheetViolationInfoDomain, visitSheetViolationDomain))
            {
                return visitSheetViolationInfoDomain
                    .GetAll()
                    .Where(x => x.VisitSheet.Id == documentId)
                    .SelectMany(x => visitSheetViolationDomain.GetAll()
                        .Where(y => y.ViolationInfo.Id == x.Id)
                        .DefaultIfEmpty())
                    .Where(x => x.IsThreatToLegalProtectedValues)
                    .Select(x => new
                    {
                        RealityObjectId = x.ViolationInfo.RealityObject.Id,
                        Municipality = x.ViolationInfo.RealityObject.Municipality.Name,
                        x.ViolationInfo.RealityObject.Address
                    })
                    .AsEnumerable()
                    .DistinctBy(x => x.RealityObjectId)
                    .ToListDataResult(baseParams.GetLoadParam(), this.container);
            }
        }

        /// <inheritdoc />
        public IDataResult ListForRegistry(BaseParams baseParams)
        {
            var visitSheetDomain = this.container.ResolveDomain<VisitSheet>();
            var visitSheetViolationInfoDomain = this.container.ResolveDomain<VisitSheetViolationInfo>();
            var preventiveActionDomain = this.container.ResolveDomain<PreventiveAction>();
            
            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var isExport = baseParams.Params.GetAs<bool>("isExport");

            using (this.container.Using(visitSheetDomain, visitSheetViolationInfoDomain, preventiveActionDomain))
            {
                var visitSheetFilterQuery = visitSheetDomain.GetAll()
                    .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd);

                var realityObjectsDict = visitSheetViolationInfoDomain.GetAll()
                    .Where(x => visitSheetFilterQuery.Any(y => y.Id == x.VisitSheet.Id))
                    .Select(x => new
                    {
                        x.VisitSheet.Id,
                        RealityObject = new
                        {
                            x.RealityObject.Id,
                            x.RealityObject.FiasAddress.AddressName
                        }
                    })
                    .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, 
                        x => new
                        {
                            Addresses = x.DistinctBy(y => y.RealityObject.Id)
                                .AggregateWithSeparator(y => y.RealityObject.AddressName, ", "),
                            Count = x.Count()
                        });

                var prevActionMunicipalityDict = preventiveActionDomain.GetAll()
                    .WhereIfElse(realityObjectId != 0,
                        x => realityObjectsDict.Keys.Contains(x.Id),
                        x => visitSheetFilterQuery.Any(z => z.Inspection.Id == x.Inspection.Id))
                    .ToDictionary(x => x.Inspection.Id, x => x.Municipality.Name);

                return visitSheetDomain.GetAll()
                    .WhereIfElse(realityObjectId != 0,
                        x => realityObjectsDict.Keys.Contains(x.Id),
                        x => visitSheetFilterQuery.Any(y => y == x))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        InspectionId = x.Inspection.Id,
                        x.Inspection.TypeBase,
                        ExecutingInspector = x.ExecutingInspector.Fio,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.VisitDateStart,
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var realityObjects = realityObjectsDict.Get(x.Id);
                        return new
                        {
                            x.Id,
                            x.State,
                            MunicipalityName = prevActionMunicipalityDict.Get(x.InspectionId),
                            Address = realityObjects?.Addresses ?? string.Empty,
                            x.ExecutingInspector,
                            RealityObjectCount = realityObjects?.Count ?? 0,
                            x.DocumentNumber,
                            x.DocumentDate,
                            x.VisitDateStart,
                            HasViolation = realityObjects?.Count > 0,
                            x.TypeBase,
                            TypeDocumentGji = TypeDocumentGji.VisitSheet,
                            x.InspectionId
                        };
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), usePersistentObjectOrdering: true, usePaging: !isExport);
            }
        }
    }
}