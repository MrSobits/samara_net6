namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Entity
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.Enums.ClaimWork;

    using Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// View Реестр документов ПИР
    /// </summary>
    public class ViewDocumentClw : PersistentObject
    {
        /// <summary>
        /// Идентификатор жилого дома по договору
        /// </summary>
        public virtual long RealityObjectId { get; set; }

        /// <summary>
        /// Идентификатор ПИР
        /// </summary>
        public virtual long ClaimWorkId { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual ClaimWorkDocumentType DocumentType { get; set; }

        /// <summary>
        /// Тип основания ПИР
        /// </summary>
        public virtual ClaimWorkTypeBase BaseType { get; set; }

        /// <summary>
        /// Основание ПИР
        /// </summary>
        public virtual string BaseInfo { get; set; }

        /// <summary>
        /// Тип должника
        /// </summary>
        public virtual DebtorType DebtorType { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата рассмотрения
        /// </summary>
        public virtual DateTime? ReviewDate { get; set; }

        /// <summary>
        /// Сумма претензии
        /// </summary>
        public virtual decimal? PretensSum { get; set; }

        /// <summary>
        /// Пени претензии
        /// </summary>
        public virtual decimal? PretensPenalty { get; set; }

        /// <summary>
        /// Планируемый срок оплаты в претензии
        /// </summary>
        public virtual DateTime? PretensPlanedDate { get; set; }

        /// <summary>
        /// Номер заявления
        /// </summary>
        public virtual string LawsuitNumber { get; set; }

        /// <summary>
        /// Дата заявления
        /// </summary>
        public virtual DateTime? LawsuitDate { get; set; }

        /// <summary>
        /// Погашенная сумма долга
        /// </summary>
        public virtual decimal? LawsuitSum { get; set; }

        /// <summary>
        /// Погашенная сумма пени
        /// </summary>
        public virtual decimal? LawsuitPenalty { get; set; }
    }
}