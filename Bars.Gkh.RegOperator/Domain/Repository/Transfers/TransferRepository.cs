namespace Bars.Gkh.RegOperator.Domain.Repository.Transfers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories;

    /// <summary>
    /// Репозиторий для работы с трансферами
    /// </summary>
    /// <typeparam name="TTransfer">Тип трансфера</typeparam>
    public class TransferRepository<TTransfer> : BaseDomainRepository<TTransfer>, ITransferRepository<TTransfer>, ITransferRepository
        where TTransfer : Transfer
    {
        /// <summary>
        /// Домен-сервис <see cref="Transfer"/>
        /// </summary>
        public ITransferDomainService TransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MoneyOperation"/>
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <inheritdoc />
        public IQueryable<TTransfer> GetByGuid(string guid, MoneyDirection direction)
        {
            if (direction == MoneyDirection.Income)
            {
                return this.GetBySourcesGuids(new[] { guid });
            }

            return this.GetByTargetsGuids(new[] { guid });
        }

        /// <inheritdoc />
        public IQueryable<TTransfer> GetBySourcesGuids(IEnumerable<string> guids)
        {
            return this.DomainService.GetAll().Where(t => guids.Contains(t.SourceGuid));
        }

        /// <inheritdoc />
        public IQueryable<TTransfer> GetByTargetsGuids(IEnumerable<string> guids)
        {
            return this.DomainService.GetAll().Where(t => guids.Contains(t.TargetGuid));
        }

        /// <inheritdoc />
        public IQueryable<TTransfer> GetByMoneyOperation(MoneyOperation operation)
        {
            return this.DomainService.GetAll().Where(x => x.Operation.Id == operation.Id && x.ChargePeriod.Id == operation.Period.Id);
        }

        /// <inheritdoc />
        public IQueryable<TTransfer> GetByOriginatorGuid(string originatorGuid)
        {
            return this.DomainService.GetAll().Where(t => originatorGuid == t.Operation.OriginatorGuid);
        }

        /// <inheritdoc />
        public void SaveOrUpdate(Transfer transfer)
        {
            this.TransferDomain.SaveOrUpdate(transfer);
        }

        /// <inheritdoc />
        public IQueryable<MoneyOperation> GetNonCanceledOperations(string originatorGuid)
        {
            return this.MoneyOperationDomain.GetAll()
                .Where(x => x.OriginatorGuid == originatorGuid)
                .Where(x => !this.MoneyOperationDomain.GetAll().Any(y => y.CanceledOperation == x))
                .Where(x => x.CanceledOperation == null);
        }

        IQueryable<T> ITransferRepository.QueryOver<T>()
        {
            return this.TransferDomain.GetAll<T>();
        }
    }
}
