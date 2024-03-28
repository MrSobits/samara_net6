namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System.Collections.Generic;
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с системными группами
    /// </summary>
    public interface IPersonalAccountSystemGroupService
    {
        /// <summary>
        /// Добавление лицевых счетов в системные группы
        /// </summary>
        /// <param name="accountsId">Идентификаторы ЛС</param>
        /// <param name="systemGroupName">Имя системной группы</param>
        /// <param name="isNeedCreateSystemGroup"></param>
        /// <returns>Результат операции</returns>
        IDataResult AddPersonalAccountsToSystemGroup(List<long> accountsId, string systemGroupName, bool isNeedCreateSystemGroup = false);

        /// <summary>
        /// Массовое исключение ЛС из системных групп
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="systemGroupName">Имя системной группы</param>
        /// <returns>Результат операции</returns>
        IDataResult RemovePersonalAccountsFromSystemGroups(BaseParams baseParams, string systemGroupName);

        /// <summary>
        /// Возвращает количество лицевых счетов которые сосотоят в текущей системной группе
        /// </summary>
        /// <param name="systemGroupName">Имя системной группы</param>
        /// <returns>Результат</returns>
        IDataResult GetCountPersonalAccountsInSystemGroup(string systemGroupName);

        /// <summary>
        /// Исключение всех ЛС из системных групп
        /// </summary>
        /// <param name="systemGroupName">Имя системной группы</param>
        /// <returns>Идентификаторы удаленных ЛС</returns>
        List<long> RemoveAllPersonalAccountsFromSystemGroup(string systemGroupName);
    }
}
