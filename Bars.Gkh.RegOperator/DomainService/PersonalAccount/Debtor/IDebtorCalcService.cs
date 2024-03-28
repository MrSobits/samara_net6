namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Enums;

    using Entities;
    using Log;

    public interface IDebtorCalcService
    {
        /// <summary>
        /// Размер порции при обработке данных
        /// </summary>
        int BulkSize { get; }

        /// <summary>
        /// Инициализировать кэш периодов перерасчета
        /// </summary>
        void InitRecalcHistory();

        /// <summary>
        /// Метод возвращает запрос по ЛС, которые будут обрабатываться согласно настроек
        /// </summary>
        IQueryable<BasePersonalAccount> GetQuery();

        /// <summary>
        /// Вычисление неплательщиков
        /// </summary>
        List<Entities.PersonalAccount.Debtor> GetDebtors(int skip, int take, IProcessLog log = null);

        /// <summary>
        /// Проверить, удовлетворяет ли должник настройкам попадания в реестра неплательщиков
        /// </summary>
        /// <param name="debtor">Должник</param>
        IDataResult CheckDebtor(Entities.PersonalAccount.Debtor debtor);

        /// <summary>
        /// Получить информацию о задолженности по ЛС
        /// </summary>
        IDictionary<long, DebtorInfo> GetDebtorsInfo(IQueryable<BasePersonalAccount> query, IProcessLog log = null);
    }

    public class DebtorInfo
    {
        public long PersonalAccountId { get; set; }
        public long OwnerId { get; set; }
        public PersonalAccountOwnerType OwnerType { get; set; }
        public string OwnerName { get; set; }
        public DateTime StartDate { get; set; }
        public decimal DebtSum { get; set; }
        public decimal DebtBaseTariffSum { get; set; }
        public decimal DebtDecisionTariffSum { get; set; }
        public decimal PenaltyDebt { get; set; }
        public int ExpiredDaysCount { get; set; }
        public int ExpiredMonthCount { get; set; }
        public IDataResult IsDebtPaidResult { get; set; }
        public bool IsDebtPaid => this.IsDebtPaidResult?.Success ?? false;
    }
}