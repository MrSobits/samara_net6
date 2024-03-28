namespace Bars.Gkh.Decisions.Nso.Domain.Decisions
{
    using B4;
    using Entities;

    /// <summary>
    /// Интерфейс решения. Инкапсулирует логику принятия решения на доме по протоколу
    /// </summary>
    public interface IDecision
    {
        /// <summary>
        /// Наименование решения
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Код решения
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Вид решения
        /// </summary>
        IDecisionType DecisionType { get; set; }

        /// <summary>
        /// Решения на одном уровне исключающие
        /// </summary>
        bool Exclusive { get; set; }
    }
}