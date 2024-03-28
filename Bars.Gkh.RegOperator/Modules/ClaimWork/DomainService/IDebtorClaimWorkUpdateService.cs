namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс для создания и обновления ПИР
    /// </summary>
    public interface IDebtorClaimWorkUpdateService
    {
        /// <summary>
        /// Создать ПИР
        /// </summary>
        /// <param name="accountQuery">Список лс</param>
        IDataResult CreateClaimWorks(IQueryable<BasePersonalAccount> accountQuery);

        /// <summary>
        /// Пересчитать ПИР
        /// </summary>
        /// <param name="claimworks">Список ПИР</param>
        void RecalcClaimWorks(IEnumerable<DebtorClaimWork> claimworks);

        /// <summary>
        /// Обновить состояния ПИР
        /// </summary>
        /// <param name="baseParams">
        /// id - по одному документу ПИР
        /// ids - по нескольким документам ПИР
        /// <see cref="DebtorClaimWork.Id"/>
        /// </param>
        IDataResult UpdateStates(BaseParams baseParams);

        /// <summary>
        /// Обновление состояния ПИР
        /// </summary>
        /// <param name="ids">Идентификаторы ПИР <see cref="DebtorClaimWork.Id"/></param>
        IDataResult UpdateStates(long[] ids = null);

        /// <summary>
        /// Установить начальный статус
        /// </summary>
        IDataResult SetDefaultState(IQueryable<DebtorClaimWork> debtorClaimWorkQuery);
    }
}