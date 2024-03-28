namespace Bars.GkhGji.Regions.Tula.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;

    using Gkh.Entities;

    /// <summary>
    /// Исполнитель обращения
    /// </summary>
    public class AppealCitsExecutant : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual Bars.GkhGji.Entities.AppealCits AppealCits { get; set; }

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
        public virtual DateTime PerformanceDate { get; set; }

        /// <summary>
        /// Ответственный
        /// </summary>
        public virtual bool IsResponsible { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}
