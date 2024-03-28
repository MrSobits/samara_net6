namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Справочник "Виды протоколов собрания собственников"
    /// </summary>
    public class OwnerProtocolType : BaseGkhEntity
    {
        /// <summary>
        /// Наименование типа протокола
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

    }
}