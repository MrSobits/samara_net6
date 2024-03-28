namespace Bars.Gkh.RegOperator.Extenstions
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Методы расширения для работы с трансферами
    /// </summary>
    public static class TransferRepoExtensions
    {
        /// <summary>
        /// Метод возвращает <see cref="IQueryable{TTransfer}"/>
        /// </summary>
        /// <typeparam name="TTransfer">Тип трансфера</typeparam>
        /// <param name="domain">Базовый домен-сервис</param>
        /// <returns>Подзапрос</returns>
        public static IQueryable<TTransfer> GetAll<TTransfer>(this IDomainService<Transfer> domain) where TTransfer : Transfer
        {
            var transferDomain = domain as ITransferDomainService;
            if (transferDomain.IsNotNull())
            {
                return transferDomain.GetAll<TTransfer>();
            }

            return ApplicationContext.Current.Container.Resolve<ITransferDomainService>().GetAll<TTransfer>();
        }

        /// <summary>
        /// Метод возвращает <typeparamref name="TTransfer"/>
        /// </summary>
        /// <typeparam name="TTransfer">Тип трансфера</typeparam>
        /// <param name="domain">Базовый домен-сервис</param>
        /// <param name="id">Идентификатор</param>
        /// <returns>Подзапрос</returns>
        public static TTransfer Get<TTransfer>(this IDomainService<Transfer> domain, object id) where TTransfer : Transfer
        {
            var transferDomain = domain as ITransferDomainService;
            if (transferDomain.IsNotNull())
            {
                return transferDomain.Get<TTransfer>(id);
            }

            return ApplicationContext.Current.Container.Resolve<ITransferDomainService>().Get<TTransfer>(id);
        }
    }
}