namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Сервис конвертации <see cref="SpecialAccountDecision"/> / <see cref="RegOpAccountDecision"/>
    /// в <see cref="CalcAccount"/>
    /// </summary>
    public interface ICalcAccountOwnerDecisionService
    {
        /// <summary>
        /// Создать спец. счет на основе <see cref="SpecialAccountDecision"/>
        /// </summary>
        IDataResult SaveSpecialCalcAccount(SpecialAccountDecision decision);

        /// <summary>
        /// Создать счет регопа на основе <see cref="RegOpAccountDecision"/>
        /// </summary>
        IDataResult SaveRegopCalcAccount(RegOpAccountDecision decision);

        /// <summary>
        /// Массовое создание спец. счетов на основе <see cref="SpecialAccountDecision"/>
        /// </summary>
        IDataResult SaveSpecialCalcAccount(IQueryable<SpecialAccountDecision> decisionQuery);

        /// <summary>
        /// Массовое создание счетов регопа на основе <see cref="RegOpAccountDecision"/>
        /// </summary>
        IDataResult SaveRegopCalcAccount(IQueryable<RegOpAccountDecision> decisionQuery);
    }
}