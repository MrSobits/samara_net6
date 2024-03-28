namespace Bars.Gkh.RegOperator.Entities.Views
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Представление владельца ЛС на определенный период
    /// </summary>
    public class ViewAccountOwnershipHistory : PersistentObject
    {
        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Владелец ЛС
        /// </summary>
        public virtual PersonalAccountOwner AccountOwner { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Период действия
        /// </summary>
        public virtual ChargePeriod Period { get; set; }
    }
}