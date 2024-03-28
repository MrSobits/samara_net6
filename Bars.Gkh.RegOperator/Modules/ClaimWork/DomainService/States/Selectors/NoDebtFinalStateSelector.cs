namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.Selectors
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// В случае успешности проверки оставляет только финальный статус
    /// </summary>
    public class NoDebtFinalStateSelector : IClwStateSelector
    {
        #region Implementation of IClwStateSelector

        /// <summary>
        /// Установить следующий обработчик в цепочке вызовов
        /// </summary>
        /// <param name="nextSelector">Следующий фильтр статусов</param>
        public void SetSuccessor(IClwStateSelector nextSelector)
        {
        }

        /// <summary>
        /// Отфильтровать статусы, доступные для основания ПИР
        /// </summary>
        /// <param name="statesToFilter">Начальный список статусов, который должен быть отфильтрован</param>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="documentMeta">Содержит сопоставление между типом документа и статусами</param>
        /// <param name="useCache">Использовать кэш для получения документов</param>
        public void Filter(List<State> statesToFilter, BaseClaimWork claimWork, IEnumerable<DocumentMeta> documentMeta, bool useCache = false)
        {
            ArgumentChecker.NotNull(claimWork, "claimWork");
            ArgumentChecker.NotNull(documentMeta, "documentMeta");

            var clw = claimWork as DebtorClaimWork;
            if (clw != null)
            {
                if (clw.IsDebtPaid && clw.DebtPaidDate.HasValue && clw.DebtPaidDate <= DateTime.Now)
                {
                    statesToFilter.RemoveAll(x => !x.FinalState);
                }
            }
        }

        #endregion
    }
}