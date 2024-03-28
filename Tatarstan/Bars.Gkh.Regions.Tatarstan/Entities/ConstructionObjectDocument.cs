namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Документ для объекта строительства
    /// </summary>
    public class ConstructionObjectDocument : BaseEntity
    {
        /// <summary>
        /// Объект строительства
        /// </summary>
        public virtual ConstructionObject ConstructionObject { get; set; }
        
        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual ConstructionObjectDocumentType Type { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual int? Number { get; set; }

        /// <summary>
        /// Файл 
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Участник процесса
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
