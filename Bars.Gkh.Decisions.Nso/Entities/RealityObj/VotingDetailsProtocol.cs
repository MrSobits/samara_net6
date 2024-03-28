namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Decisions.Nso.Enums;
    using Bars.Gkh.Enums;

    public class VotingDetailsProtocol : BaseEntity
    {
        /// <summary>
        /// Протокол решения
        /// </summary>
        public virtual RealityObjectDecisionProtocol Protocol { get; set; }

        /// <summary>
        /// Форма проведения голосования
        /// </summary>
        public virtual FormVoting FormVoting { get; set; }

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
        public virtual string OrderTakingDcisionOwners { get; set; }

        /// <summary>
        /// Порядок ознакомления с информацией
        /// </summary>
        public virtual string OrderAcquaintanceInformation { get; set; }



        /// <summary>
        /// Ежегодное собрание
        /// </summary>
        public virtual YesNo AnnuaLMeeting { get; set; }

        /// <summary>
        /// Правомерность собрания
        /// </summary>
        public virtual LegalityMeeting LegalityMeeting { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual VotingStatus VotingStatus { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}