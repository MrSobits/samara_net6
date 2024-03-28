namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;

    public interface IDebtorStateProvider
    {
        /// <summary>
        /// Инициализировать кэш
        /// </summary>
        /// <param name="debtorQuery"></param>
        void InitCache(IQueryable<DebtorClaimWork> debtorQuery);

        /// <summary>
        /// Инициализировать кэш
        /// </summary>
        /// <param name="debtorIds">Идентификаторы <see cref="DebtorClaimWork"/></param>
        void InitCache(IEnumerable<long> debtorIds);

        /// <summary>
        /// Получить статус документа
        /// </summary>
        /// <param name="debtor">Dto должника</param>
        /// <param name="avaiableDocs">Доступные для создания документы</param>
        DebtorState GetState(DebtorClaimWork debtor, IEnumerable<ClaimWorkDocumentType> avaiableDocs);
    }

    public class DebtorClaimWorkDto
    {
        public long Id { get; set; }
        public bool IsDebtPaid { get; set; }
        public bool IsInitiated { get; set; }
    }
}