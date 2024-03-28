namespace Bars.Gkh.RegOperator.Entities
{
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Ссылка на банковский документ
    /// </summary>
    public class BankAccountStatementLink : BaseImportableEntity
    {
        /// <summary>
        /// For NH
        /// </summary>
        protected BankAccountStatementLink()
        {
            
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="document"></param>
        public BankAccountStatementLink(BankAccountStatement statement, BankDocumentImport document)
        {
            ArgumentChecker.NotNull(statement, "statement");
            ArgumentChecker.NotNull(document, "document");

            Statement = statement;
            Document = document;
        }

        /// <summary>
        /// Банковская выписка
        /// </summary>
        public virtual BankAccountStatement Statement { get; set; }

        /// <summary>
        /// Импортированный банковский документ
        /// </summary>
        public virtual BankDocumentImport Document { get; set; }
    }
}
