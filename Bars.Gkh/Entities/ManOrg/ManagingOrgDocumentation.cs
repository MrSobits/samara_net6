using Bars.B4.Modules.FileStorage;

namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    /// Организационно-техническая документация
    /// </summary>
    public class ManagingOrgDocumentation : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

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
        public virtual DateTime DocumentDate { get; set; }

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
