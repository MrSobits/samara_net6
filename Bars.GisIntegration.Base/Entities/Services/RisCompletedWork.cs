namespace Bars.GisIntegration.Base.Entities.Services
{
    using System;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Выполненная работа
    /// </summary>
    public class RisCompletedWork : BaseRisEntity
    {
        /// <summary>
        /// Плановая работа
        /// </summary>
        public virtual WorkPlanItem WorkPlanItem { get; set; }

        /// <summary>
        /// Фотография объекта
        /// </summary>
        public virtual Attachment ObjectPhoto { get; set; }
        
        /// <summary>
        /// Файл акта
        /// </summary>
        public virtual Attachment ActFile { get; set; }

        /// <summary>
        /// Дата акта
        /// </summary>
        public virtual DateTime? ActDate { get; set; }
        
        /// <summary>
        /// Номер акта
        /// </summary>
        public virtual string ActNumber { get; set; }
    }
}
