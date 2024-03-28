namespace Bars.Gkh.ClaimWork.CodedReports.ReportHandler
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Modules.ClaimWork.DomainService.ReportHandler;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Обработчик проставления файла при печати "Исковое заявление" в реестре неплательщиков ПИР
    /// </summary>
    public class RegopPetitionExportHandler : BaseClwReportExportHandler<Petition, LawSuitReport>
    {
        /// <inheritdoc />
        protected override bool ValidateDocument(Petition document)
        {
            return document.ClaimWork.ClaimWorkTypeBase == ClaimWorkTypeBase.Debtor;
        }

        /// <inheritdoc />
        protected override void HandleExportInternal(Petition document, FileInfo file)
        {
            document.File = file;
            this.DocumentDomain.Update(document);
        }
    }
}