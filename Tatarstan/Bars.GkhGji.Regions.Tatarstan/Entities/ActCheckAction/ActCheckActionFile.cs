namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities.Base;

    /// <summary>
    /// Файл действия акта проверки
    /// </summary>
    public class ActCheckActionFile : BaseEntity, IAnnexEntity
    {
        /// <summary>
        /// Действие акта проверки
        /// </summary>
        public virtual ActCheckAction ActCheckAction { get; set; }
        
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

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