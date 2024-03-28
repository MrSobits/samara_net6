namespace Bars.Gkh.Modules.ClaimWork.DomainService.States
{
    using System.Collections.Generic;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Интерфейс для фильтрации статусов по основанию ПИР
    /// </summary>
    public interface IClwStateSelector
    {
        /// <summary>
        /// Установить следующий обработчик в цепочке вызовов
        /// </summary>
        /// <param name="nextSelector">Следующий фильтр статусов</param>
        void SetSuccessor(IClwStateSelector nextSelector);

        /// <summary>
        /// Отфильтровать статусы, доступные для основания ПИР
        /// </summary>
        /// <param name="statesToFilter">Начальный список статусов, который должен быть отфильтрован</param>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="documentMeta">Содержит сопоставление между типом документа и статусами</param>
        /// <param name="useCache">Использовать кэш для получения документов</param>
        void Filter(List<State> statesToFilter, BaseClaimWork claimWork, IEnumerable<DocumentMeta> documentMeta, bool useCache = false);
    }
}