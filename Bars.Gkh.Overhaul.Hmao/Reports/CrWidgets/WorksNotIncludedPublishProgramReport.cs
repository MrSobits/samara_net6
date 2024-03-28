namespace Bars.Gkh.Overhaul.Hmao.Reports.CrWidgets
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Отчёт для виджета "Работы из основной версии ДПКР, вне опубликованной программы"
    /// </summary>
    public class WorksNotIncludedPublishProgramReport : DataExportReport
    {
        public IWindsorContainer Container { get; set; }

        private Dictionary<long, IEnumerable<ReportDataDto>> ReportData { get; set; }

        /// <inheritdoc />
        public WorksNotIncludedPublishProgramReport() : base(new ReportTemplateBinary(Resources.WorksNotIncludedPublishProgram))
        {
        }

        /// <inheritdoc />
        public override string Name => "Статистические данные по КР. Работы из основной версии ДПКР, вне опубликованной программы";
        
        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            this.PrepareData();
            
            var rowIndex = 0;
            
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            
            foreach (var muData in this.ReportData.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();
                    
                foreach (var record in muData.Value)
                {
                    sectionRo.ДобавитьСтроку();
                    
                    sectionRo["seqNumber"] = ++rowIndex;
                    sectionRo["municipalityName"] = record.MunicipalityName;
                    sectionRo["address"] = record.Address;
                    sectionRo["commonEstateObjectName"] = record.CommonEstateObjectName;
                    sectionRo["planYear"] = record.PlanYear;
                }

                sectionMu["countRoMu"] = muData.Value.Count();
            }

            reportParams.SimpleReportParams["countRoTotal"] = rowIndex;
        }

        private void PrepareData()
        {
            var municipalityId = this.BaseParams.Params.GetAs<long>("municipalityId");
            var stage3Domain = this.Container.ResolveDomain<VersionRecord>();
            var dpkrService = this.Container.Resolve<IDpkrService>();
            
            using (this.Container.Using(stage3Domain, dpkrService))
            {
                var municipalityIds = dpkrService.GetMunicipalityIds();
                
                this.ReportData = stage3Domain.GetAll()
                    .Where(x => x.ProgramVersion.IsMain)
                    .WhereIfElse(municipalityId > 0, x => x.ProgramVersion.Municipality.Id == municipalityId,
                        x => municipalityIds.Contains(x.ProgramVersion.Municipality.Id))
                    .GroupBy(x => x.RealityObject.Municipality.Id)
                    .ToDictionary(x => x.Key, 
                        x => x.OrderBy(y => y.RealityObject.Address)
                            .Select(y => new ReportDataDto
                            {
                                MunicipalityName = y.RealityObject.Municipality.Name,
                                Address = y.RealityObject.Address,
                                CommonEstateObjectName = y.CommonEstateObjects,
                                PlanYear = y.Year.ToString()
                            }));
            }
        }
        
        /// <summary>
        /// DTO данных для выгрузки 
        /// </summary>
        private class ReportDataDto
        {
            public string MunicipalityName { get; set; }
            public string Address { get; set; }
            public string CommonEstateObjectName { get; set; }
            public string PlanYear { get; set; }
        }
    }
}