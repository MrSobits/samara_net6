namespace Bars.Gkh.Decisions.Nso.Domain.Decisions
{
    using System.Collections.Generic;

    /// <summary>
    /// Контейнер для регистрации переходов решений
    /// </summary>
    public interface IDecisionContainer
    {
        /// <summary>
        /// Переходы решений
        /// </summary>
        IDictionary<IDecision, ICollection<IDecision>> DecisionTransfers { get; }

        /// <summary>
        /// Все решения. Для удобства
        /// </summary>
        ICollection<IDecision> AllDecisions { get; }

        /// <summary>
        /// Все типы решений. Для удобства
        /// </summary>
        ICollection<IDecisionType> AllTypes { get; }

        IList<string> GetSiblings(string code);
    }
}