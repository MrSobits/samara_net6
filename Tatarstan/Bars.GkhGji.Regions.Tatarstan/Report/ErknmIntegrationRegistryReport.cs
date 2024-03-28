namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Dto;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    /// <summary>
    /// Выгрузка реестра "Интеграция с ЕРКНМ"
    /// </summary>
    public class ErknmIntegrationRegistryReport : DataExportReport
    {
        /// <inheritdoc />
        public ErknmIntegrationRegistryReport()
            : base(new ReportTemplateBinary(Resources.ErknmIntegrationRegistryReport))
        {
        }

        /// <inheritdoc />
        public override string Name => "Реестр интеграции с ЕРКНМ";

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var records = (IList<ErknmRegistryDocumentDto>)this.Data;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("DocSection");

            foreach (var record in records)
            {
                section.ДобавитьСтроку();
                section["DocType"] = record.DocumentType.GetDisplayName();
                section["DocNum"] = record.DocumentNumber;
                section["DocDate"] = record.DocumentDate;
                section["ErknmRegDate"] = record.ErknmRegistrationDate;
                section["ErknmRegNum"] = record.ErknmRegistrationNumber;
            }
        }
    }
}