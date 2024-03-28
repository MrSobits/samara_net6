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
    /// Выгрузка данных виджета "Стоимость работ в разрезе КЭ"
    /// </summary>
    public class CostOfWorksInStructuralElementContextReport : DataExportReport
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public override string Name => "Статистические данные по КР. Стоимость работ в разрезе КЭ";
        
        private List<DpkrService.CostOfWorksInStructuralElementContextReportDataDto> ReportData { get; set; }
        
        /// <inheritdoc />
        public CostOfWorksInStructuralElementContextReport() : base(new ReportTemplateBinary(Resources.CostOfWorksInStructuralElementContext))
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
            decimal total = 0;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRo");

            foreach (var record in this.ReportData)
            {
                section.ДобавитьСтроку();
            
                section["seqNumber"] = ++rowIndex;
                section["structuralElementName"] = record.StructuralElementName;
                section["workCost"] = record.WorkCost;

                total += record.WorkCost;
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

            reportParams.SimpleReportParams["countRoTotal"] = total;
        }

        private void PrepareData()
        {
            var dpkrService = this.Container.Resolve<IDpkrService>();

            using (this.Container.Using(dpkrService))
            {
                this.ReportData = (List<DpkrService.CostOfWorksInStructuralElementContextReportDataDto>)dpkrService
                    .GetCostOfWorksInStructuralElementContext(this.BaseParams).Data;
            }
        }
    }
}