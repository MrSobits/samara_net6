namespace Bars.Gkh.Modules.ClaimWork.DomainService.States
{
    using System.Collections.Generic;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Интерфейс провайдера состояний для основания ПИР
    /// </summary>
    public interface IClwStateProvider
    {
        /// <summary>
        /// Собрать кэш по основаниям ПИР
        /// </summary>
        /// <param name="claimWorksIds">Id оснований</param>
        void InitCache(IEnumerable<long> claimWorksIds);

        /// <summary>
        /// Очистить кэш
        /// </summary>
        void Clear();

        /// <summary>
        /// Получить список возможных типов документов для создания
        /// </summary>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="rules">Валидаторы перехода</param>
        /// <param name="useCache">Использовать кэш</param>
        List<ClaimWorkDocumentType> GetAvailableTransitions(
            BaseClaimWork claimWork,
            IEnumerable<IClwTransitionRule> rules,
            bool useCache = false);

        /// <summary>
        /// Получить следующее возможное состояние для основания ПИР
        /// </summary>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="rules">Валидаторы перехода</param>
        /// <param name="stateSelector">Фильтр статусов</param>
        /// <param name="useCache">Использовать кеш</param>
        State GetNextState(BaseClaimWork claimWork, IEnumerable<IClwTransitionRule> rules, IClwStateSelector stateSelector, bool useCache = false);
    }
}