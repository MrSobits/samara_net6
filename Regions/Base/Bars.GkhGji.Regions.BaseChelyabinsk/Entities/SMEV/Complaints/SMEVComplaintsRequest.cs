namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Enums;
    using System;


    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVComplaintsRequest : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual long? ComplaintId { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual TypeComplainsRequest TypeComplainsRequest { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// текст запроса
        /// </summary>
        public virtual string TextReq { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        public virtual string Answer { get; set; }

    }
}
