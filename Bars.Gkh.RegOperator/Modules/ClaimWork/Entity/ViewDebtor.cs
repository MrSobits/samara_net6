namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Entity
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.Enums;

    using Enums;
    using Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// View Реестр неплательщиков
    /// </summary>
    public class ViewDebtor : PersistentObject
    {
        /// <summary>
        /// Идентификатор претензионно-исковой работы
        /// </summary>
        public virtual long ClwId { get; set; }

        /// <summary>
        /// Тип документов претензионно-исковой работы
        /// </summary>
        public virtual ClaimWorkDocumentType DocumentType { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// ФИО/Наименование ЮЛ
        /// </summary>
        public virtual string OwnerName { get; set; }

        /// <summary>
        /// Идентификатр "Дом"
        /// </summary>
        public virtual long RoId { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Идентификатор комнаты
        /// </summary>
        public virtual long RoomId { get; set; }

        /// <summary>
        /// Адрес улицы
        /// </summary>
        public virtual string RoomAddress { get; set; }

        /// <summary>
        /// Идентификатор Муниципального образования 
        /// </summary>
        public virtual long MuId { get; set; }

        /// <summary>
        /// Муниципальное образование 
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Идентификатор улицы
        /// </summary>
        public virtual long? StlId { get; set; }

        /// <summary>
        /// Поселок
        /// </summary>
        public virtual string Settlement { get; set; }

        /// <summary>
        /// Наименование суда
        /// </summary>
        public virtual string JurSectorNumber { get; set; }

        /// <summary>
        /// Дата искового заявления
        /// </summary>
        public virtual DateTime? LawsuitDocDate { get; set; }

        /// <summary>
        /// Номер искового заявления
        /// </summary>
        public virtual string LawsuitDocNumber { get; set; }

        /// <summary>
        /// Кем рассмотрено исковое заявление
        /// </summary>
        public virtual LawsuitConsiderationType WhoConsidered { get; set; }

        /// <summary>
        /// Дата принятия заявления
        /// </summary>
        public virtual DateTime? DateOfAdoption { get; set; }

        /// <summary>
        /// Дата рассмотрения заявления
        /// </summary>
        public virtual DateTime? DateOfReview { get; set; }

        /// <summary>
        /// Результат рассмотрения
        /// </summary>
        public virtual LawsuitResultConsideration ResultConsideration { get; set; }

        /// <summary>
        /// Сумма признанной задолженности (основной долг)
        /// </summary>
        public virtual decimal? DebtSumApproved { get; set; }

        /// <summary>
        /// Сумма признанной задолженности (пени)
        /// </summary>
        public virtual decimal? PenaltyDebtApproved { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual LawsuitDocumentType LawsuitDocType { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DateConsideration { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string NumberConsideration { get; set; }

        /// <summary>
        /// Погашено до исполнит. производства (основной долг)
        /// </summary>
        public virtual decimal? CbDebtSum { get; set; }

        /// <summary>
        /// Погашено до исполнит. производства (пени)
        /// </summary>
        public virtual decimal? CbPenaltyDebtSum { get; set; }

        /// <summary>
        /// Дата формирования претензии
        /// </summary>
        public virtual DateTime? PretensionDocDate { get; set; }

        /// <summary>
        /// Номер претензии
        /// </summary>
        public virtual string PretensionDocNumber { get; set; }

        /// <summary>
        /// Сумма претензии (основной долг)
        /// </summary>
        public virtual decimal? PretensionDebtSum { get; set; }

        /// <summary>
        /// Сумма претензии (пени)
        /// </summary>
        public virtual decimal? PretensionPenaltyDebtSum { get; set; }

        /// <summary>
        /// Погашено до решения суда
        /// </summary>
        public virtual RepaymentType RepaidBeforeDecision { get; set; }

        /// <summary>
        /// Погашено до исполнительного производства
        /// </summary>
        public virtual RepaymentType RepaidBeforeExecutionProceedings { get; set; }

        /// <summary>
        /// Поступило возражение
        /// </summary>
        public virtual bool Objection { get; set; }
    }
}