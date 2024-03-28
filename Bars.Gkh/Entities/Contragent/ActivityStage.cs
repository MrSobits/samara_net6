namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Стадия деятельности
    /// </summary>
    public class ActivityStage : BaseEntity
    {
        /// <summary>
        /// Id сущности
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Тип сущности
        /// </summary>
        public virtual ActivityStageOwner EntityType { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }
        
        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Стадия
        /// </summary>
        public virtual ActivityStageType ActivityStageType { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo Document { get; set; }
    }
}