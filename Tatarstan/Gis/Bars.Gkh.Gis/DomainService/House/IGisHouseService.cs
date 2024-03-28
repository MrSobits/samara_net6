namespace Bars.Gkh.Gis.DomainService.House
{
    using System;
    using System.Collections.Generic;
    using Entities.House;
    using Entities.PersonalAccount;

    /// <summary>
    /// Сервис для работы с данными о доме
    /// </summary>
    public interface IGisHouseService
    {
        /// <summary>
        /// Получение хранилища дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <returns></returns>
        GisHouseProxy GetHouseStorage(long realityObjectId);

        /// <summary>
        /// Получение параметров дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<HouseParam> GetHouseParams(long realityObjectId, DateTime date);

        /// <summary>
        /// Получение услуг дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<HouseService> GetHouseServices(long realityObjectId, DateTime date);

        /// <summary>
        /// Получение показаний приборов учета дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<HouseCounter> GetHouseCounterValues(long realityObjectId, DateTime date);

        /// <summary>
        /// Получение начислений дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<HouseAccruals> GetHouseAccruals(long realityObjectId, DateTime date);


        /// <summary>
        /// Получение лицевых счетов дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        IEnumerable<GisPersonalAccount> GetHousePersonalAccounts(long realityObjectId, DateTime date);
    }
}