namespace Bars.Gkh.Overhaul.Hmao.Reports.CrWidgets
{
    using System.Collections.Generic;
    using System.Linq;
    
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Properties;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    
    /// <summary>
    /// Выгрузка данных виджета "Дома, с отсутствующими параметрами для расчета ДПКР"
    /// </summary>
    public class HousesWithMissingParamsReport : DataExportReport
    {
        public IWindsorContainer Container { get; set; }
        
        public IGkhConfigProvider ConfigProvider { get; set; }
        
        /// <summary>
        /// Минимальное количество квартир
        /// </summary>
        private int MinCountApartments => this.ConfigProvider.Get<OverhaulHmaoConfig>().HouseAddInProgramConfig.MinimumCountApartments;

        private Dictionary<long, IEnumerable<ReportDataDto>> ReportData { get; set; }
        
        /// <inheritdoc />
        public HousesWithMissingParamsReport() : base(new ReportTemplateBinary(Resources.HousesWithMissingParams))
        {
        }

        /// <inheritdoc />
        public override string Name => "Статистические данные по КР. Дома, с отсутствующими параметрами для расчета ДПКР";
        
        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            this.PrepareData();
            
            var rowIndex = 0;
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRo");
            
            foreach (var muData in this.ReportData.OrderBy(x => x.Key))
            {
                foreach (var record in muData.Value)
                {
                    section.ДобавитьСтроку();
                
                    section["seqNumber"] = ++rowIndex;
                    section["municipalityName"] = record.MunicipalityName;
                    section["address"] = record.Address;
                    section["missingParameter"] = record.MissingParameter;
                    section["parameterName"] = record.ParameterName;
                }
            }
        }

        private void PrepareData()
        {
            var municipalityId = this.BaseParams.Params.GetAs<long>("municipalityId");
            
            var realityObjectStructuralElementDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            var missingCeoDomain = this.Container.ResolveDomain<RealityObjectMissingCeo>();
            var dpkrService = this.Container.Resolve<IDpkrService>();

            using (this.Container.Using(realityObjectDomain, realityObjectStructuralElementDomain, missingCeoDomain, dpkrService))
            {
                var municipalityIds = dpkrService.GetMunicipalityIds();
                
                var baseQuery = realityObjectDomain.GetAll()
                    .WhereIfElse(municipalityId > 0, x => x.Municipality.Id == municipalityId, 
                        x => municipalityIds.Contains(x.Municipality.Id))
                    .Where(x => new[] { ConditionHouse.Serviceable, ConditionHouse.Dilapidated }.Contains(x.ConditionHouse))
                    .Where(x => x.RealEstateType.Name.ToLower() != "не задано")
                    .Where(x => !x.IsNotInvolvedCr)
                    .Where(x => x.NumberApartments >= this.MinCountApartments)
                    .Where(x => realityObjectStructuralElementDomain
                        .GetAll()
                        .Where(y => x.Id == y.RealityObject.Id)
                        .Any(y => y.State.StartState));

                var emptyYearParam = realityObjectStructuralElementDomain.GetAll()
                    .Where(x => baseQuery.Any(bq => bq.Id == x.RealityObject.Id))
                    .Where(x => x.StructuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm)
                    .Where(x => x.LastOverhaulYear == 0)
                    .Select(x => new ReportDataDto
                    {
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        MissingParameter = "Год установки или последнего кап. ремонта",
                        ParameterName = x.StructuralElement.Group.Name,
                    })
                    .ToList();

                var emptyVolumeParam = realityObjectStructuralElementDomain.GetAll()
                    .Where(x => baseQuery.Any(bq => bq.Id == x.RealityObject.Id))
                    .Where(x => x.StructuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm)
                    .Where(x => x.Volume == 0)
                    .Select(x => new ReportDataDto
                    {
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        MissingParameter = "Объём",
                        ParameterName = x.StructuralElement.Group.Name,
                    })
                    .ToList();

                var missingCeoParams = missingCeoDomain
                    .GetAll()
                    .WhereIf(municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId)
                    .Where(x => baseQuery.Any(bq => bq.Id == x.RealityObject.Id))
                    .Where(x => x.MissingCommonEstateObject.IncludedInSubjectProgramm)
                    .Select(x => new ReportDataDto
                    {
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        MissingParameter = "КЭ",
                        ParameterName = x.MissingCommonEstateObject.Name,
                    })
                    .ToList();
                
                emptyYearParam.AddRange(missingCeoParams);
                emptyYearParam.AddRange(emptyVolumeParam);

                this.ReportData = emptyYearParam
                    .GroupBy(x => x.MunicipalityId)
                    .ToDictionary(x => x.Key,
                        x => x.OrderBy(y => y.Address)
                            .Select(y => new ReportDataDto
                            {
                                MunicipalityName = y.MunicipalityName,
                                Address = y.Address,
                                MissingParameter = y.MissingParameter,
                                ParameterName = y.ParameterName
                            }));
            }
        }
        
        /// <summary>
        /// DTO данных для выгрузки 
        /// </summary>
        private class ReportDataDto
        {
            public long MunicipalityId { get; set; }
            public string MunicipalityName { get; set; }
            public string Address { get; set; }
            public string MissingParameter { get; set; }
            public string ParameterName { get; set; }
        }
    }
}