namespace Bars.Gkh.Regions.Tatarstan.Import.UtilityDebtor
{
    using System;
    
    using Bars.Gkh.Enums;

    public sealed class UtilityDebtorRecord
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="rowNumber"></param>
        public UtilityDebtorRecord(int rowNumber)
        {
            this.RowNumber = rowNumber;
        }

        #region Задолженность по оплате ЖКУ

        /// <summary>
        /// Абонент - DEBTOR
        /// </summary>
        public string AccountOwner { get; set; }

        /// <summary>
        /// Тип абонента - SUBSCRIBERTYPE
        /// </summary>
        public OwnerType OwnerType { get; set; }

        /// <summary>
        /// Сумма долга - BASEDEBT
        /// </summary>
        public decimal? ChargeDebt { get; set; }

        /// <summary>
        /// Сумма долга по пени - PENALTYDEBT
        /// </summary>
        public decimal? PenaltyDebt { get; set; }

        /// <summary>
        /// количество дней просрочки - DELAYDAYS
        /// </summary>
        public int? CountDaysDelay { get; set; }

        /// <summary>
        /// Жилой дом - ADDRESSFIAS                 
        /// </summary>
        public Guid? HouseGuid { get; set; }

        #endregion


        #region Исполнительное производство

        /// <summary>
        /// Подразделение ОСП - OSP_SUBDIVISION
        /// </summary>
        public string JurInstitutionCode { get; set; }

        /// <summary>
        /// Регистрационный номер - REG_NUM
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Дата возбуждения - EXCITATIONDATE
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата прекращения - COMPLETIONDATE
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Взыскатель - CREDITOR
        /// </summary>
        public string Creditor { get; set; }

        /// <summary>
        /// Статья - CLAUSE
        /// </summary>
        public string Clause { get; set; }

        /// <summary>
        /// Пункт - PARAGRAPH
        /// </summary>
        public string Paragraph { get; set; }

        /// <summary>
        /// Подпункт - SUBPARAGRAPH
        /// </summary>
        public string Subparagraph { get; set; }

        /// <summary>
        /// Номер документа - EXECUTIVE_DOC_NUM
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Статус - EXECUTIONSTATUS   
        /// </summary>
        public string StateCode { get; set; }

        #endregion


        #region Постановление о временном ограничении выезда должника из Российской Федерации

        /// <summary>
        /// DEPARTURE_LIMIT_ACT
        /// </summary>
        public bool HaveDepartureRestriction { get; set; }

        /// <summary>
        /// Дата документа - DEPARTURE_LIMIT_ACT_DATE
        /// </summary>
        public DateTime? DepartureRestrictionDocDate { get; set; }

        #endregion


        #region Постановление о наложении ареста на имущество должника

        /// <summary>
        /// ARREST_PROPERTY_ACT
        /// </summary>
        public bool HaveSeizureOfProperty { get; set; }

        /// <summary>
        /// Дата документа - ARREST_PROPERTY_ACT_DATE
        /// </summary>
        public DateTime? SeizureOfPropertyDocDate { get; set; }

        #endregion


        /// <summary>
        /// Номер строки
        /// </summary>
        public int RowNumber { get; internal set; }

        /// <summary>
        /// Признак корректности строки
        /// </summary>
        public bool IsValidRecord { get; internal set; }
    }
}