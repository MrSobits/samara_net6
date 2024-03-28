namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Сервис для работы с группами ЛС
    /// </summary>
    public interface IPersonalAccountGroupService
    {
        /// <summary>
        /// Список групп, в которых состоит указанный ЛС
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список ЛС</returns>
        DataResult.ListDataResult<PersAccGroup> ListGroupsByAccount(BaseParams baseParams);

        /// <summary>
        /// Добавить лицевой счёт в группу
        /// </summary>
        /// <param name="baseParams">Баозовые параметры запроса</param>
        /// <returns></returns>
        IDataResult AddPersonalAccountToGroups(BaseParams baseParams);

        /// <summary>
        /// Удалить лицевой счёт из групп
        /// </summary>
        /// <param name="baseParams">Баозовые параметры запроса</param>
        /// <returns></returns>
        IDataResult RemovePersonalAccountFromGroups(BaseParams baseParams);

        /// <summary>
        /// Массовое исключение ЛС из групп
        /// </summary>
        /// <param name="accounts">Лицевые счета</param>
        /// <param name="groupIds">Группы</param>
        /// <returns>Результат операции</returns>
        IDataResult RemovePersonalAccountsFromGroups(IQueryable<PersonalAccountDto> accounts, long[] groupIds);

        /// <summary>
        /// Массовое включени ЛС в группы
        /// </summary>
        /// <param name="accounts">Аккаунты</param>
        /// <param name="groupIds">Идентификаторы групп</param>
        /// <returns>Результат операции</returns>
        IDataResult AddPersonalAccountsToGroups(IQueryable<PersonalAccountDto> accounts, long[] groupIds);
    }
}