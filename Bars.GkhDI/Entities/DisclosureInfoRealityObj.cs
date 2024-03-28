namespace Bars.GkhDi.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Gkh.Entities;
    using Gkh.Enums;

    /// <summary>
    /// Объект недвижимости деятельности управляющей организации в периоде раскрытия информации
    /// </summary>
    public class DisclosureInfoRealityObj : BaseGkhEntity
    {
        /// <summary>
        /// Период раскрытия информации
        /// </summary>
        public virtual PeriodDi PeriodDi { get; set; }

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        #region Общие сведения о доме

        /// <summary>
        /// Иски по компенсации нанесенного ущерба
        /// </summary>
        public virtual decimal? ClaimCompensationDamage { get; set; }

        /// <summary>
        /// Иски по снижению платы в связи с неоказанием услуг
        /// </summary>
        public virtual decimal? ClaimReductionPaymentNonService { get; set; }

        /// <summary>
        /// Иски по снижению платы в связи с недопоставкой ресурсов
        /// </summary>
        public virtual decimal? ClaimReductionPaymentNonDelivery { get; set; }

        /// <summary>
        /// Выполняемые работы
        /// </summary>
        public virtual string ExecutionWork { get; set; }

        /// <summary>
        /// Выполнение обязательств
        /// </summary>
        public virtual string ExecutionObligation { get; set; }

        /// <summary>
        /// Перечень работ по содержанию и ремонту. Описание стоимости услуг
        /// </summary>
        public virtual string DescriptionServiceCatalogRepair { get; set; }

        /// <summary>
        /// Перечень работ по содержанию и ремонту. Описание стоимости тарифов
        /// </summary>
        public virtual string DescriptionTariffCatalogRepair { get; set; }

        /// <summary>
        /// Файл выполняемых работ
        /// </summary>
        public virtual FileInfo FileExecutionWork { get; set; }

        /// <summary>
        /// Файл выполнения обязательств
        /// </summary>
        public virtual FileInfo FileExecutionObligation { get; set; }

        /// <summary>
        /// Файл описания стоимости услуг
        /// </summary>
        public virtual FileInfo FileServiceCatalogRepair { get; set; }

        /// <summary>
        /// Файл описания стоимости тарифов
        /// </summary>
        public virtual FileInfo FileTariffCatalogRepair { get; set; }

        #endregion

        #region Финансовые показатели по ремонту, содержанию дома

        /// <summary>
        /// Объем работ по ремонту
        /// </summary>
        public virtual decimal? WorkRepair { get; set; }

        /// <summary>
        /// Объем работ по благоустройству
        /// </summary>
        public virtual decimal? WorkLandscaping { get; set; }

        /// <summary>
        /// Субсидии
        /// </summary>
        public virtual decimal? Subsidies { get; set; }

        /// <summary>
        /// Кредит
        /// </summary>
        public virtual decimal? Credit { get; set; }

        /// <summary>
        /// Финансирование по договорам лизинга
        /// </summary>
        public virtual decimal? FinanceLeasingContract { get; set; }

        /// <summary>
        /// Финансирование по энергосервисным договорам
        /// </summary>
        public virtual decimal? FinanceEnergServContract { get; set; }

        /// <summary>
        /// Целевые взносы жителей
        /// </summary>
        public virtual decimal? OccupantContribution { get; set; }

        /// <summary>
        /// Иные источники
        /// </summary>
        public virtual decimal? OtherSource { get; set; }

        #endregion

        #region Единичные поля над гридами разделов

        /// <summary>
        /// Данные об использование нежилых помещений
        /// </summary>
        public virtual YesNoNotSet NonResidentialPlacement { get; set; }

        /// <summary>
        /// Были случаи снижения платы
        /// </summary>
        public virtual YesNoNotSet ReductionPayment { get; set; }

        /// <summary>
        /// Договоры на использование мест общего пользования
        /// </summary>
        public virtual YesNoNotSet PlaceGeneralUse { get; set; }

        #endregion

        #region Претензии по качеству работ

        /// <summary>
        /// Количество поступивших претензий
        /// </summary>
        public virtual int? ReceivedPretensionCount { get; set; }

        /// <summary>
        /// Количество удовлетворенных претензий
        /// </summary>
        public virtual int? ApprovedPretensionCount { get; set; }

        /// <summary>
        /// Количество поступивших претензий
        /// </summary>
        public virtual int? NoApprovedPretensionCount { get; set; }

        /// <summary>
        /// Сумма произведенного перерасчета
        /// </summary>
        public virtual decimal? PretensionRecalcSum { get; set; }

        #endregion Претензии по качеству работ

        #region Претензионно-исковая работа

        /// <summary>
        /// Направлено претензий потребителям-должникам
        /// </summary>
        public virtual int? SentPretensionCount { get; set; }

        /// <summary>
        /// Направлено исковых заявлений
        /// </summary>
        public virtual int? SentPetitionCount { get; set; }

        /// <summary>
        /// Получено денежных средств по результатам претензионно-исковой работы 
        /// </summary>
        public virtual decimal? ReceiveSumByClaimWork { get; set; }

        #endregion Претензионно-исковая работа

        #region Финансовые показатели  по содержанию и текущему ремонту

        /// <summary>
        /// Авансовые платежи на старт периода
        /// </summary>
        public virtual decimal? AdvancePayments { get; set; } 

        /// <summary>
        /// переходящие остатки средств
        /// </summary>
        public virtual decimal? CarryOverFunds { get; set; } 

        /// <summary>
        ///  задолженность на начало периода
        /// </summary>
        public virtual decimal? Debt   { get; set; } 

        /// <summary>
        /// начисление за  содержание
        /// </summary>
        public virtual decimal? ChargeForMaintenanceAndRepairsMaintanance { get; set; }

        /// <summary>
        /// начисление за текущий ремонт
        /// </summary>
        public virtual decimal? ChargeForMaintenanceAndRepairsRepairs { get; set; }

        /// <summary>
        /// начисление за управление
        /// </summary>
        public virtual decimal? ChargeForMaintenanceAndRepairsManagement { get; set; }

        /// <summary>
        /// начисление всего
        /// </summary>
        public virtual decimal? ChargeForMaintenanceAndRepairsAll { get; set; }

        /// <summary>
        /// Всего получено денежных средств от владельцев
        /// </summary>
        public virtual decimal? ReceivedCashFromOwners { get; set; }

        /// <summary>
        /// Всего получено денежных средств от владельцев на целевые
        /// </summary>
        public virtual decimal? ReceivedCashFromOwnersTargeted { get; set; }

        /// <summary>
        /// Всего получено денежных средств как субсидии
        /// </summary>
        public virtual decimal? ReceivedCashAsGrant { get; set; }

        /// <summary>
        /// Получено средств за использование общей собственности
        /// </summary>
        public virtual decimal? ReceivedCashFromUsingCommonProperty { get; set; }

        /// <summary>
        /// Получено средств как другие типы платежей
        /// </summary>
        public virtual decimal? ReceivedCashFromOtherTypeOfPayments { get; set; }

        /// <summary>
        /// Средства на балансе по окончанию периода
        /// </summary>
        public virtual decimal?  CashBalanceAll { get; set; }

        /// <summary>
        /// Авансовые платежи потребителей на конец периода
        /// </summary>
        public virtual decimal? CashBalanceAdvancePayments { get; set; }

        /// <summary>
        /// Переходящие остатки денежных средств на конец периода
        /// </summary>
        public virtual decimal? CashBalanceCarryOverFunds { get; set; }

        /// <summary>
        /// Задолженность потребителей на конец периода
        /// </summary>
        public virtual decimal? CashBalanceDebt { get; set; }

        /// <summary>
        /// Всего получено денежных средств
        /// </summary>
        public virtual decimal? ReceivedCashAll { get; set; }

        #endregion

        #region Финансовые показатели - Коммунальные услуги

        /// <summary>
        /// Коммунальные услуги - Авансовые платежи на начало периода
        /// </summary>
        public virtual decimal? ComServStartAdvancePay{ get; set; }

        /// <summary>
        /// Коммунальные услуги - Переходящие остатки средств на начало периода
        /// </summary>
        public virtual decimal? ComServStartCarryOverFunds { get; set; }

        /// <summary>
        ///  Коммунальные услуги - Задолженность на начало периода
        /// </summary>
        public virtual decimal? ComServStartDebt { get; set; }

        /// <summary>
        /// Коммунальные услуги - Авансовые платежи на конец периода
        /// </summary>
        public virtual decimal? ComServEndAdvancePay { get; set; }

        /// <summary>
        /// Коммунальные услуги - Переходящие остатки средств на конец периода
        /// </summary>
        public virtual decimal? ComServEndCarryOverFunds { get; set; }

        /// <summary>
        ///  Коммунальные услуги - Задолженность на конец периода
        /// </summary>
        public virtual decimal? ComServEndDebt { get; set; }

        /// <summary>
        /// Количество поступивших претензий
        /// </summary>
        public virtual int? ComServReceivedPretensionCount { get; set; }

        /// <summary>
        /// Количество удовлетворенных претензий
        /// </summary>
        public virtual int? ComServApprovedPretensionCount { get; set; }

        /// <summary>
        /// Количество поступивших претензий
        /// </summary>
        public virtual int? ComServNoApprovedPretensionCount { get; set; }

        /// <summary>
        /// Сумма произведенного перерасчета
        /// </summary>
        public virtual decimal? ComServPretensionRecalcSum { get; set; }

        #endregion Финансовые показатели - Коммунальные услуги

    }
}
