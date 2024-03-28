namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Gkh.Entities;

    /// <summary>
    /// Протоколы собственников помещений МКД
    /// </summary>
    public class PropertyOwnerProtocols : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Тип протокола
        /// </summary>
        public virtual PropertyOwnerProtocolType TypeProtocol { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description  { get; set; }

        /// <summary>
        /// Файл (документ)
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Количество голосов (кв.м.)
        /// </summary>
        public virtual decimal? NumberOfVotes { get; set; }

        /// <summary>
        /// Общее количество голосов (кв.м.)
        /// </summary>
        public virtual decimal? TotalNumberOfVotes { get; set; }

        /// <summary>
        /// Доля принявших участие (%)
        /// </summary>
        public virtual decimal? PercentOfParticipating { get; set; }

        /// <summary>
        /// Сумма займа
        /// </summary>
        public virtual decimal? LoanAmount { get; set; }

        /// <summary>
        /// Заемщик
        /// </summary>
        public virtual Contragent Borrower { get; set; }

        /// <summary>
        /// Кредитор
        /// </summary>
        public virtual Contragent Lender { get; set; }

        /// <summary>
        /// Форма проведения голосования
        /// </summary>
        public virtual FormVoting? FormVoting { get; set; }

        /// <summary>
        /// Дата окончания приема решений
        /// </summary>
        public virtual DateTime? EndDateDecision { get; set; }

        /// <summary>
        /// Место приема решений
        /// </summary>
        public virtual string PlaceDecision { get; set; }

        /// <summary>
        /// Место проведения
        /// </summary>
        public virtual string PlaceMeeting { get; set; }

        /// <summary>
        /// Дата проведения собрания
        /// </summary>
        public virtual DateTime? DateMeeting { get; set; }

        /// <summary>
        /// Время проведения собрания
        /// </summary>
        public virtual DateTime? TimeMeeting { get; set; }

        /// <summary>
        /// Дата начала проведения голосования
        /// </summary>
        public virtual DateTime? VotingStartDate { get; set; }

        /// <summary>
        /// Время начала проведения голосования 
        /// </summary>
        public virtual DateTime? VotingStartTime { get; set; }

        /// <summary>
        /// Дата окончания проведения голосования
        /// </summary>
        public virtual DateTime? VotingEndDate { get; set; }

        /// <summary>
        /// Время окончания проведения голосования
        /// </summary>
        public virtual DateTime? VotingEndTime { get; set; }

        /// <summary>
        /// Порядок приема решений собственников
        /// </summary>
        public virtual string OrderTakingDecisionOwners { get; set; }

        /// <summary>
        /// Порядок ознакомления с информацией
        /// </summary>
        public virtual string OrderAcquaintanceInfo { get; set; }

        /// <summary>
        /// Ежегодное собрание
        /// </summary>
        public virtual YesNo? AnnuaLMeeting { get; set; }

        /// <summary>
        /// Правомерность собрания
        /// </summary>
        public virtual LegalityMeeting? LegalityMeeting { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual VotingStatus? VotingStatus { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string NpaName { get; set; }

        /// <summary>
        /// Дата принятия документа
        /// </summary>
        public virtual DateTime? NpaDate { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string NpaNumber { get; set; }

        /// <summary>
        /// Тип информации в НПА
        /// </summary>
        public virtual TypeInformationNpa TypeInformationNpa { get; set; }

        /// <summary>
        /// Тип НПА
        /// </summary>
        public virtual TypeNpa TypeNpa { get; set; }

        /// <summary>
        /// Вид нормативного акта
        /// </summary>
        public virtual TypeNormativeAct TypeNormativeAct { get; set; }

        /// <summary>
        /// Орган, принявший НПА
        /// </summary>
        public virtual Contragent NpaContragent { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo NpaFile { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual NpaStatus? NpaStatus { get; set; }

        /// <summary>
        /// Причина аннулирования
        /// </summary>
        public virtual string NpaCancellationReason { get; set; }
    }
}