namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Enums;
    using System;

    public class SMEVComplaintsDecisionLifeSituation : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual SMEVComplaintsDecision SMEVComplaintsDecision { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Формулировка для ТОРа
        /// </summary>
        public virtual string FullName { get; set; }

    }
}
