namespace Bars.Gkh.ClaimWork.CodedReports.ReportHandler
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.ClaimWork.CodedReports;
    using Bars.Gkh.Modules.ClaimWork.DomainService.ReportHandler;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Обработчик проставления файла при печати "Заявление о вынесении судебного приказа" в реестре неплательщиков ПИР
    /// </summary>
    public class RegopCourtOrderClaimExportHandler : BaseClwReportExportHandler<CourtOrderClaim, LawSuitDeclarationReport>
    {
        /// <inheritdoc />
        protected override bool ValidateDocument(CourtOrderClaim document)
        {
            return document.ClaimWork.ClaimWorkTypeBase == ClaimWorkTypeBase.Debtor;
        }

        /// <inheritdoc />
        protected override void HandleExportInternal(CourtOrderClaim document, FileInfo file)
        {
            document.File = file;
            this.DocumentDomain.Update(document);
        }
    }
}