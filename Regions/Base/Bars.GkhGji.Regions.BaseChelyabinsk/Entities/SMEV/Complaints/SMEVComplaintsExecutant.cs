namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Исполнитель обращения
    /// </summary>
    public class SMEVComplaintsExecutant : BaseEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual SMEVComplaints SMEVComplaints { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public virtual Inspector Author { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Executant { get; set; }

        /// <summary>
        /// Дата поручения
        /// </summary>
        public virtual DateTime OrderDate { get; set; }

        /// <summary>
        /// Срок исполнения
        /// </summary>
        public virtual DateTime? PerformanceDate { get; set; }

        /// <summary>
        /// Ответственный
        /// </summary>
        public virtual bool IsResponsible { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Description { get; set; }
    }
}
