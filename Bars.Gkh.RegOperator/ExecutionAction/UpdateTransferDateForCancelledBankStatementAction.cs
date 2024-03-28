namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    public class UpdateTransferDateForCancelledBankStatementAction : BaseExecutionAction
    {
        public IDomainService<BankAccountStatement> BankStatementDomainService { get; set; }

        public IDomainService<MoneyOperation> MoneyOperationDomainService { get; set; }

        public IDomainService<Transfer> TransferDomainService { get; set; }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Обновление даты платежа трансфера у существующих отмененнных банковских выписок";

        /// <summary>
        /// Название действия
        /// </summary>
        public override string Name => "Обновление даты платежа трансфера у существующих отмененнных банковских выписок";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var receiptDatesDict = this.BankStatementDomainService.GetAll()
                .Where(x => x.DistributeState == DistributionState.NotDistributed)
                .Where(x => x.DistributionCode == "")
                .Where(x => x.RemainSum == 0)
                .Select(
                    x => new
                    {
                        x.TransferGuid,
                        x.DateReceipt
                    })
                .AsEnumerable()
                .GroupBy(x => x.TransferGuid)
                .ToDictionary(x => x.Key, x => (DateTime?) x.First().DateReceipt);

            var transferGuids = receiptDatesDict.Select(x => x.Key).ToArray();

            var cancelledTransfers = this.TransferDomainService.GetAll()
                .Where(x => transferGuids.Contains(x.Operation.OriginatorGuid))
                .Where(x => x.Operation.CanceledOperation != null)
                .ToArray();

            foreach (var transfer in cancelledTransfers)
            {
                var dateReceipt = receiptDatesDict.Get(transfer.Operation.OriginatorGuid);
                if (dateReceipt != null)
                {
                    transfer.PaymentDate = dateReceipt.Value;
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, cancelledTransfers, 10000, true, true);

            return new BaseDataResult
            {
                Success = true
            };
        }
    }
}