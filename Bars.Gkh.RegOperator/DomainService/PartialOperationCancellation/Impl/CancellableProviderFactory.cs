namespace Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.BankDocumentImport;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Castle.Windsor;

    /// <summary>
    /// Поставщик сервисов, поддерживаемых частичную отмену
    /// </summary>
    public class CancellableProviderFactory : ICancellableProviderFactory
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BankDocumentImport"/>
        /// </summary>
        public IDomainService<BankDocumentImport> BankDocumentImportDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BankAccountStatement"/>
        /// </summary>
        public IDomainService<BankAccountStatement> BankAccountStatementDomain { get; set; }

        /// <inheritdoc />
        public ICancellableSourceProvider GetProvider(MoneyOperation operation)
        {
            if (this.BankDocumentImportDomain.GetAll().Any(x => x.TransferGuid == operation.OriginatorGuid))
            {
                return this.Container.Resolve<IBankDocumentImportService>();
            }

            if (this.BankAccountStatementDomain.GetAll().Any(x => x.TransferGuid == operation.OriginatorGuid))
            {
                return this.Container.Resolve<IDistributionProvider>();
            }

            throw new NotImplementedException("Источник операции не поддерживает частичную отмену");
        }
    }
}