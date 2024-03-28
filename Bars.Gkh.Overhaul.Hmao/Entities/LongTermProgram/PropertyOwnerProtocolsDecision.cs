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
    public class PropertyOwnerProtocolsDecision : BaseImportableEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual PropertyOwnerProtocols Protocol { get; set; }

        /// <summary>
        /// Решение
        /// </summary>
        public virtual OwnerProtocolTypeDecision Decision { get; set; }

        /// <summary>
        /// Файл (документ)
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

    }
}