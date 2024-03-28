namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Gkh.Entities;

    /// <summary>
    /// Протоколы собственников помещений МКД
    /// </summary>
    public class PropertyOwnerProtocolsAnnex : BaseImportableEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual PropertyOwnerProtocols Protocol { get; set; }

        /// <summary>
        /// Решение
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

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

    }
}