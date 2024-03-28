namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;
    using System.Collections.Generic;
    using B4;
    using B4.Modules.FileStorage;
    using Entities;

    /// <summary>
    /// Интерфейс детализаций лицевого счета
    /// </summary>
    public interface IPersonalAccountDetailService
    {
        /// <summary>
        /// Детализация по периоду
        /// </summary>
        List<PeriodDetail> GetPeriodDetail(BasePersonalAccount account);

        /// <summary>
        /// Детализация по периоду
        /// </summary>
        DataResult.ListDataResult<PeriodDetail> GetPeriodDetail(BaseParams baseParams);

        /// <summary>
        /// Детализация по полям лс
        /// </summary>
        List<FieldDetail> GetFieldDetail(BasePersonalAccount account, string fieldName);

        /// <summary>
        /// Детализация по полям лс
        /// </summary>
        DataResult.ListDataResult<FieldDetail> GetFieldDetail(BaseParams baseParams);

        /// <summary>
        /// Детализация операций по периоду
        /// </summary>
        List<PeriodOperationDetail> GetPeriodOperationDetail(PersonalAccountPeriodSummary summary);

        /// <summary>
        /// Детализация операций по периоду
        /// </summary>
        DataResult.ListDataResult<PeriodOperationDetail> GetPeriodOperationDetail(BaseParams baseParams);
    }

    public class FieldDetail
    {
        public DateTime PeriodStart { get; set; }

        public string Period { get; set; }

        public decimal Amount { get; set; }
    }

    public class PeriodDetail
    {
        public long Id { get; set; }

        /// <summary>
        /// Для перехода в протокол расчета
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Для перехода в протокол расчета
        /// </summary>
        public long PeriodId { get; set; }

        /// <summary>
        /// Для сортировки по периоду
        /// </summary>
        public DateTime PeriodStart { get; set; }

        public string Period { get; set; }

        public decimal SaldoIn { get; set; }

        public decimal SaldoOut { get; set; }

        public decimal ChargedByBaseTariff { get; set; }

        public decimal Recalc { get; set; }

        public decimal TariffPayment { get; set; }

        public decimal SaldoChange { get; set; }

        public decimal CurrTariffDebt { get; set; }

        public decimal OverdueTariffDebt { get; set; }

        public decimal PerformedWorkCharged { get; set; }
    }

    public class PeriodOperationDetail
    {
        public long TransferId { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public decimal SaldoChange { get; set; }

        public string Period { get; set; }

        public FileInfo Document { get; set; }

    }
}