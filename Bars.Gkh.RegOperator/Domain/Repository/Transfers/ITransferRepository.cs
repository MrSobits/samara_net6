namespace Bars.Gkh.RegOperator.Domain.Repository.Transfers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Repositories;
    using Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Интерфейс работы с трансферами
    /// </summary>
    /// <typeparam name="TTransfer">Тип трансфера</typeparam>
    public interface ITransferRepository<TTransfer> : IDomainRepository<TTransfer> where TTransfer : Transfer
    {
        /// <summary>
        /// Метод возвращает входящие/исходящие трансферы по <paramref name="guid"/>
        /// </summary>
        /// <param name="guid">Гуид</param>
        /// <param name="direction">Движение</param>
        /// <returns>Позапрос</returns>
        IQueryable<TTransfer> GetByGuid(string guid, MoneyDirection direction);

        /// <summary>
        /// Метод возвращает исходящие трансферы по <paramref name="guids"/>
        /// </summary>
        /// <param name="guids">Гуидs</param>
        /// <returns>Позапрос</returns>
        IQueryable<TTransfer> GetBySourcesGuids(IEnumerable<string> guids);

        /// <summary>
        /// Метод возвращает входящие трансферы по <paramref name="guids"/>
        /// </summary>
        /// <param name="guids">Гуидs</param>
        /// <returns>Позапрос</returns>
        IQueryable<TTransfer> GetByTargetsGuids(IEnumerable<string> guids);

        /// <summary>
        /// Метод возвращает подзапрос по MoneyOperation
        /// </summary>
        /// <param name="operation">Операция</param>
        /// <returns>Позапрос</returns>
        IQueryable<TTransfer> GetByMoneyOperation(MoneyOperation operation);

        /// <summary>
        /// Метод возвращает подзапрос по Гуиду инициатора
        /// </summary>
        /// <param name="originatorGuid">Инициатор</param>
        /// <returns>Позапрос</returns>
        IQueryable<TTransfer> GetByOriginatorGuid(string originatorGuid);

        /// <summary>
        /// Сохранить трансфер
        /// </summary>
        /// <param name="transfer">Трансфер</param>
        void SaveOrUpdate(Transfer transfer);
    }

    /// <summary>
    /// Интерфейс работы с трансферами
    /// </summary>
    public interface ITransferRepository
    {
        /// <summary>
        /// Метод возвращает неотмененные операции
        /// </summary>
        /// <param name="originatorGuid">Инициатор</param>
        /// <returns>Позапрос</returns>
        IQueryable<MoneyOperation> GetNonCanceledOperations(string originatorGuid);

        /// <summary>
        /// Сохранить трансфер
        /// </summary>
        /// <param name="transfer">Трансфер</param>
        void SaveOrUpdate(Transfer transfer);

        /// <summary>
        /// Вернуть трансферы нужного типа
        /// </summary>
        /// <typeparam name="TTransfer">Тип трансфера</typeparam>
        /// <returns>Подзапрос по трансферам</returns>
        IQueryable<TTransfer> QueryOver<TTransfer>() where TTransfer : Transfer;
    }
}
