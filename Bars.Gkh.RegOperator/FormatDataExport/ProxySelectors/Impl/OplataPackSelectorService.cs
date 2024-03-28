namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Селектор Пачка оплат ЖКУ
    /// </summary>
    public class OplataPackSelectorService : BaseProxySelectorService<OplataPackProxy>
    {
        /// <inheritdoc />
        protected override ICollection<OplataPackProxy> GetAdditionalCache()
        {
            var bankDocumentImportRepository = this.Container.ResolveRepository<BankDocumentImport>();
            var bankAccountStatementRepository = this.Container.ResolveRepository<BankAccountStatement>();

            using (this.Container.Using(bankDocumentImportRepository, bankAccountStatementRepository))
            {
                return OplataPackSelectorService.GetProxies(bankDocumentImportRepository.GetAll().WhereContainsBulked(x => x.ExportId, this.AdditionalIds),
                    bankAccountStatementRepository.GetAll().WhereContainsBulked(x => x.ExportId, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, OplataPackProxy> GetCache()
        {
            var bankDocumentImportQuery = this.FilterService.GetFiltredQuery<BankDocumentImport>();
            var bankAccountStatementQuery = this.FilterService.GetFiltredQuery<BankAccountStatement>();

            return OplataPackSelectorService.GetProxies(bankDocumentImportQuery, bankAccountStatementQuery)
                .ToDictionary(x => x.Id);
        }

        public static ICollection<OplataPackProxy> GetProxies(IQueryable<BankDocumentImport> bankDocumentImportQuery,
            IQueryable<BankAccountStatement> bankAccountStatementQuery)
        {
            var container = ApplicationContext.Current.Container;
            var importedPaymentRepository = container.ResolveRepository<ImportedPayment>();

            using (container.Using(importedPaymentRepository))
            {
                var bankDocumentImports = bankDocumentImportQuery
                    .Select(x => new OplataPackProxy
                    {
                        Id = x.ExportId,
                        Date = x.DocumentDate ?? x.ImportDate,
                        Number = x.DocumentNumber != null && x.DocumentNumber != string.Empty
                            ? x.DocumentNumber
                            : x.LogImport.File.Name,
                        Sum = x.ImportedSum ?? 0m,
                        Count = importedPaymentRepository.GetAll().Count(y => y.BankDocumentImport.Id == x.Id),
                        TransferGuid = x.TransferGuid,
                        PayerName = x.PaymentAgentCode ?? x.PaymentAgentName,
                        Destination = x.ImportType,
                        OperationDate = x.ImportDate
                    })
                    .AsEnumerable();

                var bankAccountStatements = bankAccountStatementQuery
                    .Select(x => new OplataPackProxy
                    {
                        Id = x.ExportId,
                        Date = x.DocumentDate,
                        Number = x.DocumentNum != null && x.DocumentNum != string.Empty ? x.DocumentNum : x.RecipientAccountNum,
                        Sum = x.Sum,
                        Count = 1,
                        TransferGuid = x.TransferGuid,
                        PayerName = x.PayerAccountNum ?? x.PayerName,
                        Destination = x.PaymentDetails,
                        OperationDate = x.OperationDate
                    })
                    .AsEnumerable();

                return bankDocumentImports
                    .Union(bankAccountStatements)
                    .ToList();
            }
        }
    }
}