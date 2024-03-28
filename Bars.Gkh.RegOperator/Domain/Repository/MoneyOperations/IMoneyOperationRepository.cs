namespace Bars.Gkh.RegOperator.Domain.Repository.MoneyOperations
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Entities.ValueObjects;
    using DomainModelServices;

    public interface IMoneyOperationRepository
    {
        /// <summary>
        /// Получить активную (не отмененную) операцию для счета НВС
        /// </summary>
        /// <param name="suspenseAccount"></param>
        /// <returns></returns>
        MoneyOperation GetCurrentMoneyOperationFor(SuspenseAccount suspenseAccount);

        /// <summary>
        /// Получить активные (не отмененные) операции по IEnumerable участников трансфера
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IQueryable<MoneyOperation> GetOperationsByOriginator(IEnumerable<ITransferParty> source);

        /// <summary>
        /// Получить активные (не отмененные) операции по IQueryable участников трансфера
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IQueryable<MoneyOperation> GetOperationsByOriginator(IQueryable<ITransferParty> source);
    }
}