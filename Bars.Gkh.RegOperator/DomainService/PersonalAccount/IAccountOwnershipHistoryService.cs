namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс для сервиса работы с историей принадлежности лс абоненту
    /// </summary>
    //TODO Расширить при реализации режимов
    public interface IAccountOwnershipHistoryService
    {
        /// <summary>
        /// Получить абонента лс на период
        /// </summary>
        /// <param name="accountId">Id лс</param>
        /// <param name="period">Период</param>
        PersonalAccountOwner GetOwner(long accountId, IPeriod period);

        /// <summary>
        /// Получить словарь лс+абонент для указанных лс и переданного периода
        /// </summary>
        /// <param name="accountIds">Список лс, для которых нужно получить абонентов</param>
        /// <param name="period">Период</param>
        /// <returns></returns>
        Dictionary<long, long> GetOwners(long[] accountIds, IPeriod period);

        /// <summary>
        /// Получить все лс абонента на период
        /// </summary>
        /// <param name="ownerId">Id абонента</param>
        /// <param name="period">Период</param>
        List<BasePersonalAccount> GetAccounts(long ownerId, IPeriod period);
    }
}
