namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Организационно-техническая документация
    /// </summary>
    public class SupplyResourceOrgDocumentation : BaseGkhEntity
    {
        /// <summary>
        /// Поставщик коммунальных услуг
        /// </summary>
        public virtual SupplyResourceOrg SupplyResourceOrg { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Название документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
