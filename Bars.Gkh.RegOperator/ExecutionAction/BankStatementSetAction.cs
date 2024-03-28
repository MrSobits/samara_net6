namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Действие Заполнение столбца Банковская выписка в Реестре оплат платежных агентов
    /// </summary>
    public class BankStatementSetAction : BaseExecutionAction
    {
        private readonly IDomainService<BankAccountStatementLink> bankStatementLinkDomainService;

        public BankStatementSetAction(IDomainService<BankAccountStatementLink> bankStatementLinkDomainService)
        {
            this.bankStatementLinkDomainService = bankStatementLinkDomainService;
        }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Заполнение столбца Банковская выписка в Реестре оплат платежных агентов";

        /// <summary>
        /// Название действия
        /// </summary>
        public override string Name => "Заполнение столбца Банковская выписка в Реестре оплат платежных агентов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var linkedDocuments = this.bankStatementLinkDomainService.GetAll().ToList();

            foreach (var linkedDocument in linkedDocuments)
            {
                var statement = linkedDocument.Statement;

                linkedDocument.Document.BankStatement =
                    $"{(statement.DocumentNum.IsNotNull() ? statement.DocumentNum : string.Empty)} от {(statement.DocumentDate != DateTime.MinValue ? statement.DocumentDate.ToShortDateString() : string.Empty)}";
            }

            var documents = linkedDocuments.Select(x => x.Document);

            TransactionHelper.InsertInManyTransactions(this.Container, documents, 10000, true, true);

            return new BaseDataResult
            {
                Success = true
            };
        }
    }
}