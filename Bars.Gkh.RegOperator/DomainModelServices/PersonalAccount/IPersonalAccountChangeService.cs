namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;

    using B4;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;

    using Entities;

    /// <summary>
    /// Сервис изменений по ЛС
    /// </summary>
    public interface IPersonalAccountChangeService
    {
        /// <summary>
        /// Изменение значения доли собственности
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        IDataResult ChangeAreaShare(BaseParams baseParams);

        /// <summary>
        /// Изменение собственника лицевого счета
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        IDataResult ChangeOwner(BaseParams baseParams);

        /// <summary>
        /// Изменение значения даты открытия лицевого счета
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        IDataResult ChangeDateOpen(BaseParams baseParams);

        /// <summary>
        /// Изменение баланса за период
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        IDataResult ChangePeriodBalance(BaseParams baseParams);

        /// <summary>
        /// Ручное изменение пеней
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        IDataResult ChangePenalty(BaseParams baseParams);

        /// <summary>
        /// Изменение значения доли собственности
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="newValue">Новое значение доли собственности</param>
        /// <param name="dateActual">Дата, с которой начинает действовать значение</param>
        /// <param name="fileInfo">Документ</param>
        void ChangeAreaShare(BasePersonalAccount account, decimal newValue, DateTime dateActual, FileInfo fileInfo);

        /// <summary>
        /// Изменение собственника лицевого счета
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="newOwner">Новый собственник</param>
        /// <param name="changeInfo">Информация для истории изменений.</param>
        void ChangeOwner(BasePersonalAccount account, PersonalAccountOwner newOwner, PersonalAccountChangeInfo changeInfo);

        /// <summary>
        /// Изменение значения даты открытия лицевого счета
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="newDateOpen">Новая дата открытия</param>
        /// <param name="dateActual">Дата, с которой начинает действовать значение</param>
        void ChangeDateOpen(BasePersonalAccount account, DateTime newDateOpen, DateTime dateActual);
    }
}