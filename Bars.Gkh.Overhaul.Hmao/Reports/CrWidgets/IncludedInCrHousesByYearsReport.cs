namespace Bars.Gkh.Overhaul.Hmao.Reports.CrWidgets
{
    using System.Collections.Generic;
    using System.Linq;
    
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Properties;

    using Castle.Windsor;
    
    /// <summary>
    /// Выгрузка данных виджета "Дома, включенные в ДПКР в разрезе годов"
    /// </summary>
    public class IncludedInCrHousesByYearsReport : DataExportReport
    {
        public IWindsorContainer Container { get; set; }

        private Dictionary<long, IEnumerable<ReportDataDto>> ReportData { get; set; }
        
        /// <inheritdoc />
        public IncludedInCrHousesByYearsReport() : base(new ReportTemplateBinary(Resources.IncludedInCrHousesByYears))
        {
        }

        /// <inheritdoc />
        public override string Name => "Статистические данные по КР. Дома, включенные в ДПКР в разрезе годов";

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
                    sectionRo["includeDate"] = record.IncludeDate;
                }

                sectionMu["countRoMu"] = muData.Value.Count();
            }

            reportParams.SimpleReportParams["countRoTotal"] = rowIndex;
        }

        private void PrepareData()
        {
            var municipalityId = this.BaseParams.Params.GetAs<long>("municipalityId");
            var dpkrService = this.Container.Resolve<IDpkrService>();
            
            using (this.Container.Using(dpkrService))
            {
                var baseQuery = dpkrService.GetDpkrDocumentRealityObjectBaseQuery(municipalityId);
                
                this.ReportData = baseQuery
                    .GroupBy(x => x.RealityObject.Municipality.Id)
                    .ToDictionary(x => x.Key,
                        x => x.OrderBy(y => y.RealityObject.Address)
                            .Select(y => new ReportDataDto
                            {
                                MunicipalityName = y.RealityObject.Municipality.Name,
                                Address = y.RealityObject.Address,
                                IncludeDate = y.DpkrDocument.DocumentDate.ToString(),
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
            public string IncludeDate { get; set; }
        }
    }
}