namespace Bars.Gkh.Decisions.Nso.Domain.Decisions
{
    using B4;
    using Entities;

    /// <summary>
    /// Интерфейс вида решения
    /// </summary>
    public interface IDecisionType
    {
        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Клиентский javascript модуль
        /// </summary>
        string Js { get; set; }

        /// <summary>
        /// Является начальным решением
        /// </summary>
        bool IsDefault { get; set; }

        /// <summary>
        /// Принять решение
        /// </summary>
        /// <param name="baseDecision">Решение, принятое по протоколу</param>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат принятия решения</returns>
        IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams);

        /// <summary>
        /// Получить данные для отображения
        /// </summary>
        /// <param name="baseDecision">Решение</param>
        IDataResult GetValue(GenericDecision baseDecision);
    }
}