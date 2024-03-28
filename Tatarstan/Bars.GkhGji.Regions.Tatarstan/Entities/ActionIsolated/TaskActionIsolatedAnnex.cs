namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Приложение задания КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public class TaskActionIsolatedAnnex: BaseEntity
    {
        /// <summary>
        /// Задание КНМ без взаимодействия с контролируемыми лицами
        /// </summary>
        public virtual TaskActionIsolated Task { get; set; }
        
        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}