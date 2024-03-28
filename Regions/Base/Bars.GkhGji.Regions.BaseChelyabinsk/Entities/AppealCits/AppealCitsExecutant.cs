namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits
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
    public class AppealCitsExecutant : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public virtual Inspector Author { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Executant { get; set; }

        /// <summary>
        /// Проверяющий инспектор
        /// </summary>
        public virtual Inspector Controller { get; set; }

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
        /// Ответственный
        /// </summary>
        public virtual bool OnApproval { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Решение
        /// </summary>
        public virtual FileInfo Resolution { get; set; }

        /// <summary>
        /// Наименование подразделения
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }
    }
}
