namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Расширенный интерфейс домен-сериса трансферов
    /// </summary>
    public interface ITransferDomainService : IDomainService<Transfer>
    {
        /// <summary>
        /// Вернуть трансферы нужного типа
        /// </summary>
        /// <typeparam name="TTransfer">Тип трансфера</typeparam>
        /// <returns></returns>
        IQueryable<TTransfer> GetAll<TTransfer>() where TTransfer : Transfer;

        /// <summary>
        /// Вернуть трансферы нужного типа
        /// </summary>
        /// <typeparam name="TTransfer">Тип трансфера</typeparam>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        TTransfer Get<TTransfer>(object id) where TTransfer : Transfer;
    }
}