namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVERULReqNumber : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual ManOrgLicense ManOrgLicense { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public virtual ERULRequestType ERULRequestType { get; set; }
    }
}
