namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public interface IPersonalAccountOperationService
    {
        IDataResult TrySetOpenRecord(BaseParams baseParams);
        
        /// <summary>
        /// Создать историю операции, проведенной над счетом
        /// </summary>
        /// <param name="account"></param>
        /// <param name="type"></param>
        /// <param name="descriptionFunc"></param>
        /// <param name="actualFrom"></param>
        void CreateAccountHistory(BasePersonalAccount account, PersonalAccountChangeType type, Func<string> descriptionFunc, DateTime? actualFrom = null);
        
        /// <summary>
        /// Экспортировать сальдо ЛС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        IDataResult ExportExcelSaldo(BaseParams baseParams);
        
        /// <summary>
        /// Закрыть ЛС
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат закрытия</returns>
        IDataResult ClosePersonalAccount(BaseParams baseParams);

        /// <summary>
        /// Закрыть несколько лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ClosePersonalAccounts(BaseParams baseParams);
        
        /// <summary>
        /// Вернуть список статусов для закрытия лс
        /// </summary>
        /// <returns></returns>
        List<State> GetCloseStates();

        /// <summary>
        /// Удаляет персональные счета с указанными идентификаторами
        /// </summary>
        /// <param name="accountIds">Идентификаторы счетом для удаления</param>
        /// <returns></returns>
        IDataResult RemoveAccounts(long[] accountIds);

        /// <summary>
        /// Массовое закрытие счетов
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="type"></param>
        /// <param name="closeDate"></param>
        /// <param name="descriptionFunc"></param>
        void MassClosingAccounts(
            long[] accounts,
            PersonalAccountChangeType type,
            DateTime? closeDate = null,
            Func<string> descriptionFunc = null);

        /// <summary>
        /// Закрытие счета
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="type"></param>
        /// <param name="closeDate"></param>
        /// <param name="descriptionFunc"></param>
        /// <returns></returns>
        IDataResult CloseAccount(BasePersonalAccount acc, PersonalAccountChangeType type, DateTime? closeDate = null, Func<string> descriptionFunc = null, PersonalAccountChangeInfo changeInfo = null, bool resetAreaShare = false);
        
        /// <summary>
        /// Сохранение легковесное сущность для хранения изменения ЛС Для установки статуса "Не активен\"", 
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <param name="dateActualChange">Дата начала действия значения</param>
        /// <param name="dateEnd">Дата окончания действия нового значения</param>
        /// <param name="reason">Причина</param>
        void LogAccountDeactivate(BasePersonalAccount account, DateTime dateActualChange, DateTime dateEnd, string reason);
        
        /// <summary>
        /// Применение распределения зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult ApplyPerformedWorkDistribution(BaseParams baseParams);

        /// <summary>
        /// Получить долю собственности по ЛС
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Доля собственности</returns>
        IDataResult RoomAccounts(BaseParams baseParams);

        /// <summary>
        /// Получение информации по лицевым счетам для зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения</returns>
        IDataResult GetAccountsInfoForPerformedWorkDistribution(BaseParams baseParams);
    }
}