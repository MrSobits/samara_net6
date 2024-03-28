namespace Bars.Gkh.Gis.DomainService.PersonalAccount
{
    using System;
    using System.Collections.Generic;
    using Entities.PersonalAccount;

    /// <summary>
    /// Сервис для работы с данными о лицевом счете
    /// </summary>
    public interface IGisPersonalAccountService
    {
        /// <summary>
        /// Получение параметров лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<PersonalAccountParam> GetPersonalAccountParams(long realityObjectId, long personalAccountId, DateTime date);

        /// <summary>
        /// Получение услуг лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<PersonalAccountService> GetPersonalAccountServices(long realityObjectId, long personalAccountId, DateTime date);

        /// <summary>
        /// Получение показаний приборов учета лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<PersonalAccountCounter> GetPersonalAccountCounterValues(long realityObjectId, long personalAccountId, DateTime date);

        /// <summary>
        /// Получение начислений лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<PersonalAccountAccruals> GetPersonalAccountAccruals(long realityObjectId, long personalAccountId, DateTime date);

        /// <summary>
        /// Получение собственников лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<PersonalAccountOwner> GetPersonalAccountOwners(long realityObjectId, long personalAccountId, DateTime date);
    }
}
