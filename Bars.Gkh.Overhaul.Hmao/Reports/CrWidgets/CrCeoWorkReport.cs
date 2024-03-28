namespace Bars.Gkh.Overhaul.Hmao.Reports.CrWidgets
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Impl;
    using Bars.Gkh.Overhaul.Hmao.Properties;

    using Castle.Windsor;
    
    /// <summary>
    /// Выгрузка данных виджета "Количество работ ДПКР в разрезе ООИ"
    /// </summary>
    public class CrCeoWorkReport : DataExportReport
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public override string Name => "Статистические данные по КР. Количество работ ДПКР в разрезе ООИ";
        
        private List<DpkrService.CrCeoWorkReportDataDto> ReportData { get; set; }
        
        /// <inheritdoc />
        public CrCeoWorkReport() : base(new ReportTemplateBinary(Resources.CrCeoWork))
        {
        }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            this.PrepareData();

            var municipalityId = this.BaseParams.Params.GetAs<long>("municipalityId");
            var isAllMunicipalities = municipalityId == default(long);
            var headerTitle = !isAllMunicipalities ? "Муниципальное образование:" : string.Empty;
            var footerTitle = isAllMunicipalities ? "Итого по всем МО" : "Итого по МО";
            
            var rowIndex = 0;
            long totalCount = 0;
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRo");
            
            foreach (var record in this.ReportData)
            {
                section.ДобавитьСтроку();
            
                section["seqNumber"] = ++rowIndex;
                section["commonEstateObjectName"] = record.CommonEstateObjectName;
                section["workCount"] = record.WorkCount;

                totalCount += record.WorkCount;
            }

            if (isAllMunicipalities)
            {
                reportParams.SimpleReportParams["municipalityName"] = string.Empty;
                reportParams.SimpleReportParams["headerTitle"] = headerTitle;
                reportParams.SimpleReportParams["footerTitle"] = footerTitle;
            }
            else
            {
                var municipalityName = string.Empty;
                this.Container.UsingForResolved<IDomainService<Municipality>>((_, service) =>
                {
                    municipalityName = service.Load(municipalityId).Name;
                });
            
                reportParams.SimpleReportParams["municipalityName"] = municipalityName;
                reportParams.SimpleReportParams["headerTitle"] = headerTitle;
                reportParams.SimpleReportParams["footerTitle"] = footerTitle;
            }

            reportParams.SimpleReportParams["countRoTotal"] = totalCount;
        }

        private void PrepareData()
        {
            var dpkrService = this.Container.Resolve<IDpkrService>();
            
            using (this.Container.Using(dpkrService))
            {
                this.ReportData = (List<DpkrService.CrCeoWorkReportDataDto>)dpkrService.GetCrCeoWorkCounts(this.BaseParams).Data;
            }
        }
    }
}