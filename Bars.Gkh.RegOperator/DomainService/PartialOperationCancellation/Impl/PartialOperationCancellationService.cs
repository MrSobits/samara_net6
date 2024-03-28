namespace Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис частичной отмены операции
    /// </summary>
    public class PartialOperationCancellationService : IPartialOperationCancellationService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Поставщик сервисов, поддерживаемых частичную отмену
        /// </summary>
        public ICancellableProviderFactory ProviderFactory { get; set; }

        /// <summary>
        /// Расширенный интерфейс домен-сериса трансферов
        /// </summary>
        public ITransferDomainService TransferDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MoneyOperation"/>
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BasePersonalAccount"/>
        /// </summary>
        public IDomainService<BasePersonalAccount> AccountDomain { get; set; }

        /// <inheritdoc />
        public IDataResult UndoAndRepayment(IQueryable<Transfer> transferQuery, ITransferOwner repaymentTarget)
        {
            var transferDict = transferQuery
                .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null) // операция не отменена и не является отменой
                .AsEnumerable()
                .GroupBy(x => x.Operation).ToDictionary(x => x.Key);

            this.Container.InTransaction(() =>
            {
                foreach (var kvp in transferDict)
                {
                    var result = this.UndoAndRepaymentInternal(kvp.Key, kvp.Value, repaymentTarget);
                    if (!result.Success)
                    {
                        throw new ValidationException(result.Message);
                    }
                }
            });

            return new BaseDataResult();
        }

        private IDataResult UndoAndRepaymentInternal(MoneyOperation operation, IEnumerable<Transfer> transfers, ITransferOwner repaymentTarget)
        {
            var newOperation = operation.Clone().As<MoneyOperation>();

            var transferIdsToFilter = transfers.Select(x => x.Id).ToArray();
            var transfersToMove = transfers.ToList();

            this.TransferDomainService.GetAll<RealityObjectTransfer>()
                .Where(x => transferIdsToFilter.Contains(x.CopyTransfer.Id) || transferIdsToFilter.Contains(x.Originator.Id))
                .AddTo(transfersToMove);

            this.TransferDomainService.GetAll<PersonalAccountPaymentTransfer>()
                .Where(x => transferIdsToFilter.Contains(x.Originator.Id))
                .AddTo(transfersToMove);

            foreach (var transfer in transfersToMove)
            {
                transfer.Operation = newOperation;
            }

            this.MoneyOperationDomain.Save(newOperation);
            transfersToMove.ForEach(this.TransferDomainService.Update);

            var repaymentParameters = new RepaymentParameters
            {
                OwnerToRepayment = repaymentTarget,
                OldMoneyOperation = operation,
                OldOwners = transfers.Select(x => x.Owner).DistinctBy(x => x.Id).ToList(),
                UndoTransfers = transfers
            };

            return this.ProviderFactory.GetProvider(newOperation).CancelOperation(newOperation, repaymentParameters);
        }
    }
}