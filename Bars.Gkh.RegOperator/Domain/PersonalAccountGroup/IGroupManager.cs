namespace Bars.Gkh.RegOperator.PersonalAccountGroup
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерфейс для работы с группой
    /// </summary>
    public interface IGroupManager
    {
        /// <summary>
        /// Добавление в группу с проверкой периода
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="accountIds">Лс для добавления</param>
        void AddToGroupWithCheckPeriod(IPeriod period, List<long> accountIds);

        /// <summary>
        /// Удаление из группы
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        IDataResult RemoveFromGroup(BaseParams baseParams);

        /// <summary>
        /// Очистка группы
        /// </summary>
        void RemoveAllFromGroup(IPeriod period);

        /// <summary>
        /// Возвращает количество ЛС в группе
        /// </summary>
        /// <returns>Количество ЛС в группе</returns>
        IDataResult GetCountByGroup();
    }
}
