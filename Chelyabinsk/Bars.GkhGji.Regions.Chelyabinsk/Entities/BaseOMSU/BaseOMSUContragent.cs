namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using System;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Entities;
    using Enums;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Контрагенты, проводящие проверку ОМСУ
    /// </summary>
    public class BaseOMSUContragent : BaseEntity
    {
        /// <summary>
        /// Проверка Юр. лица
        /// </summary>
        public virtual BaseOMSU BaseOMSU { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}