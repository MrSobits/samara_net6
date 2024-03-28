namespace Bars.GkhGji.Regions.Habarovsk.Entities.SMEVEmergencyHouse
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVEmergencyHouse : BaseEntity
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
        /// RO
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// RO
        /// </summary>
        public virtual Room Room { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual EmergencyTypeSGIO EmergencyTypeSGIO { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// xml
        /// </summary>
        public virtual FileInfo AnswerFile { get; set; }

        /// <summary>
        /// муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
