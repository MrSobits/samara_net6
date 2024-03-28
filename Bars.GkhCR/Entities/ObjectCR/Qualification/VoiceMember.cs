namespace Bars.GkhCr.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Голос участника квалификационного отбора
    /// </summary>
    public class VoiceMember : BaseGkhEntity
    {
        /// <summary>
        /// Квалификационный отбор
        /// </summary>
        public virtual Qualification Qualification { get; set; }

        /// <summary>
        /// Участник квалификационного отбора
        /// </summary>
        public virtual QualificationMember QualificationMember { get; set; }

        /// <summary>
        /// Тип принятия голоса квалификационного отбора
        /// </summary>
        public virtual TypeAcceptQualification TypeAcceptQualification { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }
    }
}