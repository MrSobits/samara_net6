namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Система коллективного приема телевидения (СКПТ)
    /// </summary>
    public class RealityObjectVidecam : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Работоспособность 
        /// </summary>
        public virtual YesNoNotSet Workability { get; set; }      

        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual string UnicalNumber { get; set; }

        /// <summary>
        /// Место установки
        /// </summary>
        public virtual string InstallPlace { get; set; }

        /// <summary>
        /// Описание камеры
        /// </summary>
        public virtual string TypeVidecam { get; set; }

        /// <summary>
        /// УРЛ камеры
        /// </summary>
        public virtual string VidecamAddress { get; set; }
        
    }
}