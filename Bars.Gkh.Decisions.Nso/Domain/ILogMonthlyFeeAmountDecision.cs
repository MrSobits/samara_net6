namespace Bars.Gkh.Decisions.Nso.Domain
{
    using Bars.B4;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Decisions.Nso.Entities;

    public interface ILogMonthlyFeeAmountDecisionService
    {
        /// <summary>
        /// Логирования сущности в место NHibernateChangeLog
        /// </summary>
        /// <param name="entity">Сущность после изменения</param>
        /// <param name="roId">Id жилога дома</param>
        /// <param name="type">Тип сохранения</param>
        void Log(MonthlyFeeAmountDecision entity, long roId, ActionKind type);
    }
}