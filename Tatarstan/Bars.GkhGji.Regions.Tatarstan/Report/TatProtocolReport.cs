namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;
    using Bars.GkhGji.Report;

    /// <summary>
    /// Отчет "Протокол"
    /// </summary>
    public class TatProtocolReport : ProtocolGjiReport<TatProtocol>
    {
        /// <inheritdoc />
        protected override void FillRegionParams(ReportParams reportParams, DocumentGji document)
        {
            var protocol = document as TatProtocol;

            reportParams.SimpleReportParams["ДатаРождения"] = protocol.BirthDate;
            reportParams.SimpleReportParams["МестоРождения"] = protocol.BirthPlace;
            reportParams.SimpleReportParams["Гражданство"] = 
                (protocol.CitizenshipType ?? CitizenshipType.RussianFederation).GetDisplayName();
            reportParams.SimpleReportParams["МестоРаботы"] = protocol.Company;
            reportParams.SimpleReportParams["ФактАдрес"] = protocol.FactAddress;
        }
    }
}