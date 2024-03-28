namespace Bars.Gkh.RegOperator.Services.DataContracts.Accounts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Сводная информация по лицевому счету
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "AccountPeriodSummary")]
    public class AccountPeriodSummary
    {
        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]

        public long Id { get; set; }
        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PersonalAccountNum")]
        public string PersonalAccountNum { get; set; }
        
        /// <summary>
        /// Айди периода
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PeriodId")]
        public long PeriodId { get; set; }

        /// <summary>
        /// Внешний номер лицевого счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExNum")]
        public string ExNum { get; set; }

        /// <summary>
        /// ФИО абонента
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OwnerName")]
        public string OwnerName { get; set; }

        /// <summary>
        /// Адрес помещения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Tariff")]
        public decimal Tariff { get; set; }

        /// <summary>
        /// Общая площадь, кв.м.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TotalArea")]
        public decimal TotalArea { get; set; }

        /// <summary>
        /// Переплата на начало периода, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "BeginOverPay")]
        public decimal BeginOverPay { get; set; }

        /// <summary>
        /// Задолженность на начало периода, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "BeginDebt")]
        public decimal BeginDebt { get; set; }

        /// <summary>
        /// Начислено за период, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ChargedSum")]
        public decimal ChargedSum { get; set; }

        /// <summary>
        /// Начислено по базовому тарифу, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ChargedBase")]
        public decimal ChargedBase { get; set; }

        /// <summary>
        /// Начислено по тарифу решения за период, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ChargedSolution")]
        public decimal ChargedSolution { get; set; }

        /// <summary>
        /// Начисление пени за период, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PenaltyCharged")]
        public decimal PenaltyCharged { get; set; }

        /// <summary>
        /// Перерасчет, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Recalc")]
        public decimal Recalc { get; set; }

        /// <summary>
        /// Перерасчет по тарифу решения, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RecalcSolution")]
        public decimal RecalcSolution { get; set; }

        /// <summary>
        /// Перерасчет по пени, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RecalcPenalty")]
        public decimal RecalcPenalty { get; set; }

        /// <summary>
        /// Оплачено за период, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PaidSum")]
        public decimal PaidSum { get; set; }

        /// <summary>
        /// Оплачено по пени за период, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PaidPenalty")]
        public decimal PaidPenalty { get; set; }

        /// <summary>
        /// Оплачено по тарифу решения, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PaidSoiution")]
        public decimal PaidSoiution { get; set; }

        /// <summary>
        /// Оплачено по базовому тарифу за период, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PaidBase")]
        public decimal PaidBase { get; set; }

        /// <summary>
        /// Пени, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PenaltySum")]
        public decimal PenaltySum { get; set; }

        /// <summary>
        /// Предоставленная МСП, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SocialSupport")]
        public decimal SocialSupport { get; set; }

        /// <summary>
        /// Итого переплата на конец периода, руб
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "EndOverPay")]
        public decimal EndOverPay { get; set; }

        /// <summary>
        /// Итого задолженность на конец периода
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "EndDebt")]
        public decimal EndDebt { get; set; }

        /// <summary>
        /// Общая сумма накопленная домом
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HouseTotalPaid")]
        public decimal HouseTotalPaid { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Message")]
        public string Message { get; set; }

        /// <summary>
        /// Код ответа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ResponseCode")]
        public int ResponseCode { get; set; }

        /// <summary>
        /// Идентификатор РКЦ лицевого счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RKC_Id")]
        public string RkcIdentifier { get; set; }

        /// <summary>
        /// Наименование РКЦ лицевого счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RKC_Name")]
        public string RkcName { get; set; }

        /// <summary>
        /// Сумма оплат нарастающим итогом
        /// с первого по выбранный включительно период ЛС
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IncreaseAccountPaidMonthSum")]
        public decimal IncreaseAccountPaidMonthSum { get; set; }
        
        /// <summary>
        /// Сумма оплат на доме всего за все время
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PaidTotalOnHouse")]
        public decimal PaidTotalOnHouse { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SaldoIn")]
        public decimal SaldoIn { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SaldoOut")]
        public decimal SaldoOut { get; set; }

        /// <summary>
        /// Начислено с учетом перерасчета пени и начисления пени
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ChargeTotal")]
        public decimal ChargeTotal { get; set; }

        /// <summary>
        /// должно начисляться за месяц
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "MonthCharge")]
        public decimal MonthCharge { get; set; }

        /// <summary>
        /// должно начисляться за месяц
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PenaltyDebt")]
        public decimal PenaltyDebt { get; set; }

        /// <summary>
        /// Внешний номер лицевого счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OwnershipType")]
        public string OwnershipType { get; set; }
    }
}
