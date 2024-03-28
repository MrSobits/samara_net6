namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;


    /// <summary>
    /// Финансовые показатели
    /// </summary>
    public class HouseReportCommon
    {
        /// <summary>
        /// Авансовые платежи потребителей (на начало периода)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CashBalanceBeginningPeriodConsumersOverpayment")]
        public decimal CashBalanceBeginningPeriodConsumersOverpayment { get; set; }

        /// <summary>
        /// Переходящие остатки денежных средств (на начало периода)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CashBalanceBeginningPeriod")]
        public decimal CashBalanceBeginningPeriod { get; set; }

        /// <summary>
        /// Задолженность потребителей (на начало периода)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CashBalanceBeginningPeriodConsumersArrears")]
        public decimal CashBalanceBeginningPeriodConsumersArrears { get; set; }

        /// <summary>
        /// Начислено за услуги (работы) по содержанию и текущему ремонту
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ChargedForServices")]
        public decimal ChargedForServices { get; set; }

        /// <summary>
        /// Начислено за содержание дома
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ChargedForMaintenanceOfHouse")]
        public decimal ChargedForMaintenanceOfHouse { get; set; }

        /// <summary>
        /// Начислено за текущий ремонт
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ChargedForMaintenanceWork")]
        public decimal ChargedForMaintenanceWork { get; set; }

        /// <summary>
        /// Начислено за услуги управления 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ChargedForManagementService")]
        public decimal ChargedForManagementService { get; set; }

        /// <summary>
        /// Получено денежных средств всего
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReceivedCash")]
        public decimal ReceivedCash { get; set; }

        /// <summary>
        /// Получено денежных средств от собственников/нанимателей помещений 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReceivedCashFromOwners")]
        public decimal ReceivedCashFromOwners { get; set; }

        /// <summary>
        /// Получено целевых взносов от собственников/нанимателей помещений
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReceivedTargetPaymentFromOwners")]
        public decimal ReceivedTargetPaymentFromOwners { get; set; }

        /// <summary>
        /// Получено субсидий
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReceivedSubsidies")]
        public decimal ReceivedSubsidies { get; set; }

        /// <summary>
        /// Получено денежных средств от использования общего имущества
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReceivedFromUseOfCommonProperty")]
        public decimal ReceivedFromUseOfCommonProperty { get; set; }

        /// <summary>
        /// Прочие поступления
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReceivedFromOther")]
        public decimal ReceivedFromOther { get; set; }

        /// <summary>
        /// Всего денежных средств с учетом остатков
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CashTotal")]
        public decimal CashTotal { get; set; }

        /// <summary>
        /// Авансовые платежи потребителей (на конец периода)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CashBalanceEndingPeriodConsumersOverpayment")]
        public decimal CashBalanceEndingPeriodConsumersOverpayment { get; set; }

        /// <summary>
        /// Переходящие остатки денежных средств (на конец периода)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CashBalanceEndingPeriod")]
        public decimal CashBalanceEndingPeriod { get; set; }

        /// <summary>
        /// Задолженность потребителей (на конец периода)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CashBalanceEndingPeriodConsumersArrears")]
        public decimal CashBalanceEndingPeriodConsumersArrears { get; set; }

        /// <summary>
        /// Количество поступивших претензий
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ClaimsReceivedCount")]
        public int ClaimsReceivedCount { get; set; }

        /// <summary>
        /// Количество удовлетворенных претензий
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ClaimsSatisfiedCount")]
        public long ClaimsSatisfiedCount { get; set; }

        /// <summary>
        /// Количество претензий, в удовлетворении которых отказано
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ClaimsDeniedCount")]
        public int ClaimsDeniedCount { get; set; }

        /// <summary>
        /// Сумма произведенного перерасчета
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ProducedRecalculationAmount")]
        public decimal ProducedRecalculationAmount { get; set; }

        /// <summary>
        /// Направлено претензий потребителям-должникам
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "SentClaimsCount")]
        public int SentClaimsCount { get; set; }

        /// <summary>
        /// Направлено исковых заявлений
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FiledActionsCount")]
        public int FiledActionsCount { get; set; }

        /// <summary>
        /// Получено денежных средств по результатам претензионно-исковой работы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReceivedCashAmount")]
        public decimal ReceivedCashAmount { get; set; }

        /// <summary>
        /// Авансовые платежи потребителей (на начало периода), руб.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServStartAdvancePay")]
        public decimal ComServStartAdvancePay { get; set; }

        /// <summary>
        /// Переходящие остатки денежных средств (на начало периода), руб.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServStartCarryOverFunds")]
        public decimal ComServStartCarryOverFunds { get; set; }

        /// <summary>
        /// Задолженность потребителей (на начало периода), руб.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServStartDebt")]
        public decimal ComServStartDebt { get; set; }

        /// <summary>
        /// Авансовые платежи потребителей (на конец периода), руб.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServEndAdvancePay")]
        public decimal ComServEndAdvancePay { get; set; }

        /// <summary>
        /// Переходящие остатки денежных средств (на конец периода), руб
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServEndCarryOverFunds")]
        public decimal ComServEndCarryOverFunds { get; set; }

        /// <summary>
        /// Задолженность потребителей (на конец периода), руб
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServEndDebt")]
        public decimal ComServEndDebt { get; set; }

        /// <summary>
        /// Количество поступивших претензий, ед
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServReceivedPretensionCount")]
        public decimal ComServReceivedPretensionCount { get; set; }

        /// <summary>
        /// Количество удовлетворенных претензий, ед
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServApprovedPretensionCount")]
        public decimal ComServApprovedPretensionCount { get; set; }

        /// <summary>
        /// Количество претензий, в удовлетворении которых отказано, ед
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServNoApprovedPretensionCount")]
        public decimal ComServNoApprovedPretensionCount { get; set; }

        /// <summary>
        /// Сумма произведенного перерасчета, руб.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ComServPretensionRecalcSum")]
        public decimal ComServPretensionRecalcSum { get; set; }
    }
}