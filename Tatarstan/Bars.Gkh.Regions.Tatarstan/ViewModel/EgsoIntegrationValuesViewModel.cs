namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.Gkh.Regions.Tatarstan.Entities.Egso;
    using Bars.Gkh.Regions.Tatarstan.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class EgsoIntegrationValuesViewModel : BaseViewModel<EgsoIntegrationValues>
    {
        public override IDataResult List(IDomainService<EgsoIntegrationValues> domainService, BaseParams baseParams)
        {
            var egsoTaskId = baseParams.Params.GetAs<long?>("egsoTaskId");
            var egsoYear = baseParams.Params.GetAs<int>("year");
            var egsoTaskType = baseParams.Params.GetAs<EgsoTaskType>("taskType");

            if (egsoTaskId.HasValue && egsoTaskId != 0)
            {

                var res = domainService.GetAll()
                    .Where(x => x.EgsoIntegration != null && x.EgsoIntegration.Id == egsoTaskId)
                    .Select(x => new
                    {
                        x.Id,
                        Territory = x.MunicipalityDict.TerritoryName,
                        x.Value
                    }).ToList();

                return new ListDataResult(res, res.Count);
            }

            var municipalityDomainService = this.Container.Resolve<IDomainService<EgsoMunicipalityDict>>();
            var realityObjectyDomainService = this.Container.Resolve<IDomainService<RealityObject>>();
            var crObjectDomainService = this.Container.Resolve<IDomainService<ObjectCr>>();

            using (this.Container.Using(municipalityDomainService, realityObjectyDomainService, crObjectDomainService))
            {
                var listCondition = new List<ConditionHouse> { ConditionHouse.Emergency, ConditionHouse.Dilapidated, ConditionHouse.Serviceable };
                var dictValues = realityObjectyDomainService.GetAll()
                    .Where(x => x.TypeHouse == TypeHouse.ManyApartments && listCondition.Contains(x.ConditionHouse))
                    .Select(x => new
                    {
                        x.Municipality.Oktmo,
                        x.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Oktmo)
                    .ToDictionary(x => x.Key, x => x.Count());
                var itogValues = dictValues.Values.Sum();

                var crObjectList = crObjectDomainService.GetAll()
                    .Where(x => x.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full &&
                        (x.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Complete || x.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active))
                    .Select(x => new
                    {
                        x.ProgramCr.Period.DateStart.Year,
                        x.RealityObject.Municipality.Oktmo,
                        x.Id
                    }).ToList();

                var dictOverhaulValues = crObjectList.AsEnumerable()
                    .GroupBy(x => x.Year)
                    .ToDictionary(x => x.Key,
                        y => y.Select(t => new { t.Oktmo, t.Id })
                            .AsEnumerable()
                            .GroupBy(x => x.Oktmo)
                            .ToDictionary(x => x.Key, x => x.Count()));

                var itogOverhaulValues = crObjectList.AsEnumerable()
                    .GroupBy(x => x.Year)
                    .ToDictionary(x => x.Key,
                        y => y.Count());

                var oktmoRT = "92000000";

                var fakes = municipalityDomainService.GetAll()
                    .Select(x => new
                    {
                        Id = (int?)null,
                        MunicipalityDict = x,
                        Territory = x.TerritoryName,
                        Key = x.EgsoKey,
                        Oktmo = x.TerritoryCode
                    })
                    .ToList()
                    .Select(x =>
                    {
                        var val = 0;
                        var value = 0;

                        if (egsoTaskType == EgsoTaskType.OverhauledManyApartmentsCount)
                        {
                            if (x.Oktmo == oktmoRT)
                            {
                                value = itogOverhaulValues.ContainsKey(egsoYear) ? itogOverhaulValues[egsoYear] : 0;
                            }
                            else
                            {
                                if (dictOverhaulValues[egsoYear].TryGetValue(x.Oktmo, out val))
                                {
                                    value = val;
                                }
                            }
                        }
                        else
                        {
                            if (x.Oktmo == oktmoRT)
                            {
                                value = itogValues;
                            }
                            else
                            {
                                value = dictValues.ContainsKey(x.Oktmo) ? dictValues[x.Oktmo] : 0;
                            }
                        }

                        return new
                        {
                            x.Id,
                            x.MunicipalityDict,
                            x.Territory,
                            x.Key,
                            Value = value
                        };
                    }).ToList();

                return new ListDataResult(fakes, fakes.Count);
            }
        }
    }
}