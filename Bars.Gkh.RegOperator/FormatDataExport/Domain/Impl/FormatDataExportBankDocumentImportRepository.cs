namespace Bars.Gkh.RegOperator.FormatDataExport.Domain.Impl
{
    using System.Linq;

    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    public class FormatDataExportBankDocumentImportRepository : BaseFormatDataExportRepository<BankDocumentImport>
    {
        /// <inheritdoc />
        public override IQueryable<BankDocumentImport> GetQuery(IFormatDataExportFilterService filterService)
        {
            var transferDomainService = this.Container.Resolve<ITransferDomainService>();
            using (this.Container.Using(transferDomainService))
            {
                var transferQuery = filterService.GetFiltredQuery<PersonalAccountPaymentTransfer>();

                return this.Repository.GetAll()
                    .Where(x => x.PaymentConfirmationState != PaymentConfirmationState.Deleted)
                    .Where(x => transferQuery.Any(y => y.Operation.OriginatorGuid == x.TransferGuid));
            }
        }
    }
}