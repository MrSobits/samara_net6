namespace Bars.GkhGji.Regions.Voronezh.Entities.SMEVOwnershipProperty
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Enums;
    using Bars.B4.Modules.FileStorage;

    public class SMEVOwnershipProperty : BaseEntity
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
        /// RealityObject
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Room
        /// </summary>
        public virtual Room Room { get; set; }

        /// <summary>
        ///PublicPropertyLevel
        /// </summary>
        public virtual PublicPropertyLevel PublicPropertyLevel { get; set; }

        /// <summary>
        ///QueryType
        /// </summary>
        public virtual QueryTypeType QueryType { get; set; }
         
        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string CadasterNumber { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string RegisterNumber { get; set; }


        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

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
        /// xml
        /// </summary>
        public virtual FileInfo AttachmentFile { get; set; }

        /// <summary>
        /// муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
