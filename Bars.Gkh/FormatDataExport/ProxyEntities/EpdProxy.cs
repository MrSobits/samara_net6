namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Платежный документ
    /// </summary>
    public class EpdProxy : IHaveId
    {
        private const string Yes = "1";

        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Тип документа
        /// </summary>
        public string DocumentType => EpdProxy.Yes;

        /// <summary>
        /// 3. Отчетный период
        /// </summary>
        public DateTime? ReportPeriod { get; set; }

        /// <summary>
        /// 4. Номер платежного документа
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// 5. Дата формирования платежного документа
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 6. Лицевой счет
        /// </summary>
        [ProxyId(typeof(KvarProxy))]
        public long AccountId { get; set; }

        /// <summary>
        /// 7. Идентификатор расчетного счета получателя платежа
        /// </summary>
        [ProxyId(typeof(ContragentRschetProxy))]
        public long? ContragentRschetId { get; set; }

        /// <summary>
        /// 8. Количество проживающих
        /// </summary>
        public int? ResidentCount { get; set; }

        /// <summary>
        /// 9. Жилая площадь
        /// </summary>
        public decimal? LivingArea { get; set; }

        /// <summary>
        /// 10. Отапливаемая площадь
        /// </summary>
        public decimal? HeatedArea { get; set; }

        /// <summary>
        /// 11. Общая площадь для ЛС
        /// </summary>
        public decimal? TotalArea { get; set; }

        /// <summary>
        /// 12. Статус платежного документа
        /// </summary>
        public string StateFlag => EpdProxy.Yes;

        /// <summary>
        /// 13. Сумма к оплате за расчетный период по услугам, руб. (по всем услугам за расчетный период)
        /// </summary>
        public decimal? CalcPeriodDebt { get; set; }

        /// <summary>
        /// 14. Задолженность за предыдущие периоды, руб.
        /// </summary>
        public decimal? PreviousPeriodDebtDebt { get; set; }

        /// <summary>
        /// 15. Аванс на начало расчетного периода, руб.
        /// </summary>
        public decimal? Overpayment { get; set; }

        /// <summary>
        /// 16. Сумма к оплате с учетом рассрочки платежа и процентов за рассрочку,руб.
        /// </summary>
        public decimal? TotalCharge { get; set; }

        /// <summary>
        /// 17. В документе учтены оплаты, поступившие до:
        /// </summary>
        public DateTime? PaymentsBeforeDate { get; set; }

        /// <summary>
        /// 18. Дополнительная информация
        /// </summary>
        public string Param18 { get; set; }

        /// <summary>
        /// 19. Итого к оплате за расчетный период с учетом задолженности/переплаты, руб. (по всему платежному документу)
        /// </summary>
        public decimal? TotalPayment { get; set; }

        /// <summary>
        /// 20. Итого к оплате по неустойкам и судебным издержкам, руб. (итог по всем неустойкам и судебным издержкам)
        /// </summary>
        public decimal? TotalPenaltyPayment { get; set; }

        /// <summary>
        /// 21. Итого к оплате за расчетный период всего, руб. (по всему платежному документу)
        /// </summary>
        public decimal? AllTotalPayment { get; set; }

        /// <summary>
        /// 22. Оплачено денежных средств, руб.
        /// </summary>
        public decimal Paid { get; set; }

        /// <summary>
        /// 23. Дата последней поступившей оплаты
        /// </summary>
        public DateTime? LastPaymentDate { get; set; }

        #region EPDCAPITAL

        /// <summary>
        /// EPDCAPITAL 2. Платежный документ
        /// </summary>
        public long SnapshotIdCapital => this.Id;

        /// <summary>
        /// EPDCAPITAL 3. Дата
        /// </summary>
        public DateTime DateCapital => this.Date;

        /// <summary>
        /// EPDCAPITAL 4. Всего начислено за расчётный период (руб.)
        /// </summary>
        public decimal? ChargeCapital => this.Charge;

        /// <summary>
        /// EPDCAPITAL 5. Перерасчеты, корректировки (руб.)
        /// </summary>
        public decimal? ChangeCapital => this.Correction + this.Recalc;

        /// <summary>
        /// EPDCAPITAL 6. Льготы, субсидии, скидки (руб.)
        /// </summary>
        public decimal? BenefitCapital => this.Benefit;

        /// <summary>
        /// EPDCAPITAL 7. Итого к оплате за расчётный период (руб.)
        /// </summary>
        public decimal? SaldoOutCapital => this.SaldoOut;

        /// <summary>
        /// EPDCAPITAL 8. Поставщик Услуги
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// EPDCAPITAL 9. Порядок расчетов
        /// </summary>
        public string EpdCapitalParam9 { get; set; }

        /// <summary>
        /// EPDCAPITAL 10. Статус записи
        /// </summary>
        public string EpdCapitalState => EpdProxy.Yes;
        #endregion

        /// <summary>
        /// KVISOL 3. Результат квитирования
        /// </summary>
        public int? KvisolResult { get; set; }

        /// <summary>
        /// KVISOL 6. Сумма квитирования (в копейках)
        /// </summary>
        public decimal? KvisolSum { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public decimal? Tariff { get; set; }
 
        /// <summary>
        /// Начислено за период
        /// </summary>
        public decimal? Charge { get; set; }

        /// <summary>
        /// Корректировки
        /// </summary>
        public decimal? Correction { get; set; }

        /// <summary>
        /// Льготы
        /// </summary>
        public decimal? Benefit { get; set; }

        /// <summary>
        /// Перерасчеты
        /// </summary>
        public decimal? Recalc { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public decimal? SaldoOut { get; set; }
    }
}