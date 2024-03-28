namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Справочник "Виды протоколов собраний собственников - виды решений собственников"
    /// </summary>
    public class OwnerProtocolTypeDecision : BaseGkhEntity
    {
        /// <summary>
        /// Тип протокола собрания собственников
        /// </summary>
        public virtual OwnerProtocolType OwnerProtocolType { get; set; }

        /// <summary>
        /// Наименование решения
        /// </summary>
        public virtual string Name { get; set; }

    }
}