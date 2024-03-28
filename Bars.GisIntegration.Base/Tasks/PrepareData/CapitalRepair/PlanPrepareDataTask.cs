namespace Bars.GisIntegration.Base.Tasks.PrepareData.CapitalRepair
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.CapitalRepair;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    public class PlanPrepareDataTask : BasePrepareDataTask<exportPlanRequest>
    {
        private List<ProgramCrProxy> programs;

        //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
        //private Dictionary<long, Municipality> municipalityById;

        protected override void ExtractData(DynamicDictionary parameters)
        {
            var selectedMunicipalityIdsParam = parameters.GetAs("SelectedMunicipalityIds", string.Empty);

            var selectedMunicipalityIds =
                selectedMunicipalityIdsParam.IsEmpty() || selectedMunicipalityIdsParam.ToUpper() == "ALL"
                ? new long[0]
                : selectedMunicipalityIdsParam.ToLongArray();

            //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
            //var periodDomain = this.Container.ResolveDomain<Period>();
            //var municipalityDomain = this.Container.ResolveDomain<Municipality>();
            var programCrSelector = this.Container.Resolve<IDataSelector<ProgramCrProxy>>("ProgramCrSelector");

            try
            {
                this.programs = programCrSelector.GetExternalEntities(parameters)
                    .WhereIf(selectedMunicipalityIds.IsNotEmpty(), x => selectedMunicipalityIds.Contains(x.MunicipalityId))
                    .ToList();

                var municipalityIds = this.programs.Select(x => x.MunicipalityId).Distinct();

                //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                //this.municipalityById = municipalityDomain.GetAll()
                //    .Where(x => municipalityIds.Contains(x.Id))
                //    .ToDictionary(x => x.Id);
            }
            finally
            {
                //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                //this.Container.Release(periodDomain);
                //this.Container.Release(municipalityDomain);
                this.Container.Release(programCrSelector);
            }
        }

        protected override Dictionary<exportPlanRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<exportPlanRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var program in this.programs)
            {
                //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                //var municipality = this.municipalityById.Get(program.MunicipalityId);
                
                var request = new exportPlanRequest
                {
                    Items = new object[]
                    {
                        new OKTMORefType
                        {
                            //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                            //code = municipality?.Oktmo?.ToString(),
                            name = null
                        },
                        program.StartMonthYear,
                        program.EndMonthYear
                    },
                    ItemsElementName = new[]
                    {
                        ItemsChoiceType5.Municipality,
                        ItemsChoiceType5.StartMonthYear,
                        ItemsChoiceType5.EndMonthYear
                    }
                };

                if (this.DataForSigning)
                {
                    request.Id = Guid.NewGuid().ToStr();
                }

                result.Add(request, new Dictionary<Type, Dictionary<string, long>>());
            }

            return result;
        }

        protected override List<ValidateObjectResult> ValidateData()
        {
            return new List<ValidateObjectResult>();
        }
    }

    /// <summary>
    /// Прокси для КПКР
    /// </summary>
    public class ProgramCrProxy
    {
        public string StartMonthYear { get; set; }

        public string EndMonthYear { get; set; }

        public long MunicipalityId { get; set; }
    }
}
