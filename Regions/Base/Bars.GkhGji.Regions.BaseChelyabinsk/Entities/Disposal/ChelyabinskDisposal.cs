namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Рапоряжение ГЖИ для Томск (расширяется дополнительными полями)
    /// </summary>
    public class ChelyabinskDisposal : Bars.GkhGji.Entities.Disposal
    {
        /// <summary>
        /// Орган гос власти
        /// </summary>
        public virtual PoliticAuthority PoliticAuthority { get; set; }

        /// <summary>
        /// Дата формирования заявления
        /// </summary>
        public virtual DateTime? DateStatement { get; set; }

        /// <summary>
        /// время формирования заявляения
        /// </summary>
        public virtual DateTime? TimeStatement { get; set; }

        /// <summary>
        /// Номер мотивированного запроса
        /// </summary>
        public virtual string MotivatedRequestNumber { get; set; }

        /// <summary>
        /// Дата мотивированного запроса
        /// </summary>
        public virtual DateTime? MotivatedRequestDate { get; set; }

        /// <summary>
        /// Срок устранения - незнаю кому понадобилось строковое поле, в которое забивается срок устранения 
        /// </summary>
        public virtual string PeriodCorrect { get; set; }

        /// <summary>
        /// Дата составления протокола 
        /// </summary>
        public virtual DateTime? NoticeDateProtocol { get; set; }

        /// <summary>
        /// время составления
        /// </summary>
        public virtual DateTime? NoticeTimeProtocol { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        public virtual string NoticePlaceCreation { get; set; }

        /// <summary>
        /// время составления
        /// </summary>
        public virtual string NoticeDescription { get; set; }

        /// <summary>
        /// Номер решения прокурора
        /// </summary>
        public virtual string ProsecutorDecNumber { get; set; }

        /// <summary>
        /// Дата решения прокурора
        /// </summary>
        public virtual DateTime? ProsecutorDecDate { get; set; }
    }
}