namespace Bars.Gkh.Reforma.Domain
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.Enums;

    /// <summary>
    ///     Сервис работы с заявками на раскрытие информации по УО
    /// </summary>
    public interface IManOrgService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Получает список ИНН УО, по которым не создано заявок
        /// </summary>
        /// <returns>Список ИНН</returns>
        string[] GetUnrequestedInns();

        /// <summary>
        /// Получает список ИНН УО, по которым заявки подтверждены
        /// </summary>
        /// <returns>Список ИНН</returns>
        string[] GetSynchronizableInns();

        /// <summary>
        ///     Обновляет статус заявки
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="requestDate">Дата запроса</param>
        /// <param name="processDate">Дата обработки</param>
        /// <param name="status">Статус</param>
        /// <returns>Успех. False - если УО с указанным ИНН не найдена</returns>
        bool SetRequestState(string inn, DateTime requestDate, DateTime? processDate, RequestStatus status);

        /// <summary>
        /// Получение УО по ИНН
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <returns>УО</returns>
        ManagingOrganization GetManOrgByInn(string inn);

        /// <summary>
        /// Проверяет, является ли УО синхронизируемой
        /// </summary>
        /// <param name="organization">УО</param>
        /// <returns>Синхронизируема?</returns>
        bool IsSynchronizable(ManagingOrganization organization);

        /// <summary>
        /// Получение Id синхронизируемой УО по ИНН
        /// </summary>
        /// <param name="inn">ИНН УО</param>
        /// <returns>Id синхронизируемой УО</returns>
        long GetRefManOrgIdByInn(string inn);

        /// <summary>
        /// Возвращает статус заявки по ИНН
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <returns>Статус заявки</returns>
        RequestStatus GetRequestState(string inn);

        #endregion
    }
}