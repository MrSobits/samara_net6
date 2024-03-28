namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;

    using Castle.Windsor;
    
    /// <inheritdoc />
    public class MotivatedPresentationAppealCitsService : IMotivatedPresentationAppealCitsService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult Get(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            
            var mpAppealCits = this.Container.ResolveDomain<MotivatedPresentationAppealCits>();
            var documentGjiInspectorService = this.Container.Resolve<IDocumentGjiInspectorService>();
            
            using (this.Container.Using(mpAppealCits, documentGjiInspectorService))
            {
                var obj = mpAppealCits.Get(id);
            
                if (obj.IsNull())
                    return new BaseDataResult { Success = false };
            
                var dataInspectors = documentGjiInspectorService.GetInspectorsByDocumentId(id)
                    .Select(x => new
                    {
                        InspectorId = x.Inspector.Id,
                        x.Inspector.Fio
                    })
                    .ToList();
            
                var result = new
                {
                    obj.Id,
                    obj.DocumentDate,
                    obj.DocumentNum,
                    obj.DocumentNumber,
                    obj.DocumentYear,
                    obj.State,
                    InspectionId = obj.Inspection?.Id,
                    AppealCitsFormatted = $"{obj.AppealCits.Number} ({obj.AppealCits.NumberGji}) " +
                        $"{(obj.AppealCits.DateFrom.HasValue ? obj.AppealCits.DateFrom.Value.ToShortDateString() : string.Empty)}",
                    obj.PresentationType,
                    Inspectors = string.Join(", ", dataInspectors.Select(x => x.Fio)),
                    InspectorIds = string.Join(", ", dataInspectors.Select(x => x.InspectorId)),
                    obj.Official,
                    obj.ResultType
                };
            
                return new BaseDataResult(result);
            }
        }

        /// <inheritdoc />
        public IDataResult List(BaseParams baseParams)
        {
            var appealCitsId = baseParams.Params.GetAsId("appealCitizensId");

            var mpAppealCits = this.Container.ResolveDomain<MotivatedPresentationAppealCits>();
            using (this.Container.Using(mpAppealCits))
            {
                return mpAppealCits.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsId)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.PresentationType,
                        x.ResultType,
                        InspectionId = x.Inspection.Id
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container, true);
            }
        }

        /// <inheritdoc />
        public IDataResult ListForRegistry(BaseParams baseParams)
        {
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAsId("realityObjectId");
            var isExport = baseParams.Params.GetAs<bool>("isExport");
            
            var mpAppealCitsDomain = this.Container.ResolveDomain<MotivatedPresentationAppealCits>();
            var appealCitsRObjectDomain = this.Container.ResolveDomain<AppealCitsRealityObject>();
            var docGjiInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            
            using (this.Container.Using(mpAppealCitsDomain, appealCitsRObjectDomain, docGjiInspectorDomain))
            {
                var mpAppealCitsFilterQuery = mpAppealCitsDomain.GetAll()
                    .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd);

                var realityObjectsDict = appealCitsRObjectDomain.GetAll()
                    .Where(x => mpAppealCitsFilterQuery.Any(y => y.AppealCits.Id == x.AppealCits.Id))
                    .WhereIf(realityObjectId != 0, x => x.RealityObject.Id == realityObjectId)
                    .Select(x => new
                    {
                        x.AppealCits.Id,
                        RealityObject = new
                        {
                            x.RealityObject.Id,
                            x.RealityObject.Address
                        },
                        Municipality = new
                        {
                            x.RealityObject.Municipality.Id,
                            x.RealityObject.Municipality.Name
                        }
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => new
                        {
                            Addresses = x.DistinctBy(y => y.RealityObject.Id)
                                .AggregateWithSeparator(y => y.RealityObject.Address, ", "),
                            Municipalities = x.DistinctBy(y => y.Municipality.Id)
                                .AggregateWithSeparator(y => y.Municipality.Name, ", "),
                        });

                var inspectorDict = docGjiInspectorDomain.GetAll()
                    .WhereIfElse(realityObjectId != 0,
                        x => realityObjectsDict.Keys.Contains(x.DocumentGji.Id),
                        x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.MotivatedPresentationAppealCits
                            && mpAppealCitsFilterQuery.Any(y => y.Id == x.DocumentGji.Id))
                    .Select(x => new
                    {
                        x.DocumentGji.Id,
                        x.Inspector.Fio
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Fio).AggregateWithSeparator(", "));
                
                return mpAppealCitsFilterQuery
                    .WhereIf(realityObjectId != 0, x => realityObjectsDict.Keys.Contains(x.AppealCits.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        InspectionId = x.Inspection.Id,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.PresentationType,
                        x.ResultType,
                        x.Inspection.TypeBase,
                        AppealCitsId = x.AppealCits.Id
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var realityObjects = realityObjectsDict.Get(x.AppealCitsId);
                        return new
                        {
                            x.Id,
                            x.State,
                            x.InspectionId,
                            x.DocumentNumber,
                            x.DocumentDate,
                            x.PresentationType,
                            x.ResultType,
                            x.TypeBase,
                            TypeDocumentGji = TypeDocumentGji.MotivatedPresentationAppealCits,
                            MunicipalityNames = realityObjects?.Municipalities,
                            Address = realityObjects?.Addresses,
                            InspectorNames = inspectorDict.Get(x.Id)
                        };
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container, true, !isExport);
            }
        }
    }
}