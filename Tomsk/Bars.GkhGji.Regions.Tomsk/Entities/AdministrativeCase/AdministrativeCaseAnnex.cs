namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Приложения адм дела ГЖИ
    /// </summary>
    public class AdministrativeCaseAnnex : BaseEntity
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual AdministrativeCase AdministrativeCase { get; set; }

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