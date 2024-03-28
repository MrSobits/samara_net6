namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;
    using B4;
    using Entities;
    using Gkh.Entities;

    /// <summary>
    /// Интерфейс сервиса создания лицевых счетов
    /// </summary>
    public interface IPersonalAccountCreateService
    {
        /// <summary>
        /// Создать лицевой счет
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        IDataResult CreateNewAccount(BaseParams baseParams);

        /// <summary>
        /// Создать лицевой счет
        /// </summary>
        /// <param name="owner">Собственник</param>
        /// <param name="room">Помещение</param>
        /// <param name="dateOpen">Дата открытия</param>
        /// <param name="areaShare">Доля собственности</param>
        /// <returns>Лицевой счет</returns>
        BasePersonalAccount CreateNewAccount(PersonalAccountOwner owner, Room room, DateTime dateOpen, decimal areaShare);
    }
}