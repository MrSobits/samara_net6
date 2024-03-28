namespace Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Исполнительное производство
    /// </summary>
    public class ExecutoryProcess : DocumentClw
    {
        //3 поля дублируют аналогичные в UtilityDebtorClaimWork. По постановке пока непонятно, могут ли они отличаться от данных в базовой информации.

        /// <summary>
        /// Абонент
        /// </summary>
        public virtual string AccountOwner { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual OwnerType OwnerType { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }


        /// <summary>
        /// Подразделение ОСП
        /// </summary>
        public virtual JurInstitution JurInstitution { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        public virtual string RegistrationNumber { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string Document { get; set; }

        /// <summary>
        /// Сумма для погашения (долг по пени + долг по базовому)
        /// </summary>
        public virtual decimal? DebtSum { get; set; }

        /// <summary>
        /// Сумма погашения в рамках производствa (Сумма, погашенная должником в рамках производства)
        /// </summary>
        public virtual decimal? PaidSum { get; set; }

        /// <summary>
        /// Дата возбуждения
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Производство прекращено
        /// </summary>
        public virtual bool IsTerminated { get; set; }

        /// <summary>
        /// Дата прекращения
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
        
        /// <summary>
        /// Причина прекращения
        /// </summary>
        public virtual TerminationReasonType TerminationReasonType { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Notation { get; set; }

        /// <summary>
        /// Взыскатель
        /// </summary>
        public virtual string Creditor { get; set; }

        /// <summary>
        /// Адрес юр.лица
        /// </summary>
        public virtual RealityObject LegalOwnerRealityObject { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Статья
        /// </summary>
        public virtual string Clause { get; set; }

        /// <summary>
        /// Пункт
        /// </summary>
        public virtual string Paragraph { get; set; }

        /// <summary>
        /// Подпункт
        /// </summary>
        public virtual string Subparagraph { get; set; }
    }
}