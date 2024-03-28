namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;
    using Entities;
    using Gkh.Entities;

    /// <summary>
    /// Интерфейс фабрики лицевых счетов
    /// </summary>
    public interface IPersonalAccountFactory
    {
        /// <summary>
        /// Создать новый лицевой счет
        /// </summary>
        /// <param name="owner">Собственник лицевого счета</param>
        /// <param name="room">Помещение</param>
        /// <param name="dateOpen">Дата открытия счета</param>
        /// <param name="areaShare">Доля собственности</param>
        /// <returns>Лицевой счет</returns>
        BasePersonalAccount CreateNewAccount(PersonalAccountOwner owner, Room room, DateTime dateOpen, decimal areaShare);
    }
}