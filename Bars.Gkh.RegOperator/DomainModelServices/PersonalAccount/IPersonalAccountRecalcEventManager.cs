namespace Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.RegOperator.Enums;

    using Entities;

    /// <summary>
    /// Интерфейс создания отсечек перерасчета для ЛС
    /// </summary>
    public interface IPersonalAccountRecalcEventManager
    {
        /// <summary>
        /// Инициализировать кэш по оплатам.
        /// <para>Использовать, когда производятся массовые операции по ЛС</para>
        /// </summary>
        /// <param name="accountInfo">Информация для собираемого кэша</param>
        void InitCache(IDictionary<DateTime, BasePersonalAccount[]> accountInfo);

        /// <summary>
        /// Создание отсечки для перерасчета пеней
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <param name="eventDate">Реальная дата наступления события</param>
        /// <param name="eventType">Тип события перерасчета</param>
        /// <param name="description">Описание события</param>
        void CreatePenaltyEvent(BasePersonalAccount account, DateTime eventDate, RecalcEventType eventType, string description = null);

        /// <summary>
        /// Создание отсечки для перерасчета начисления
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <param name="eventDate">Реальная дата наступления события</param>
        /// <param name="eventType">Тип события перерасчета</param>
        /// <param name="description">Описание события</param>
        void CreateChargeEvent(BasePersonalAccount account, DateTime eventDate, RecalcEventType eventType, string description = null);

        /// <summary>
        /// Сохранение созданных изменений
        /// </summary>
        void SaveEvents();

        /// <summary>
        /// Удаление информации по событиям для всех ЛС.
        /// Полезно при закрытии периода
        /// </summary>
        void ClearPersistedEvents();
    }
}