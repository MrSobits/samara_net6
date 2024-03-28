using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    using Bars.B4.DataAccess;

    public class ContributionCollection : BaseEntity
    {

        /// <summary>
        /// Объект долгосрочной программы
        /// </summary>
        public virtual LongTermPrObject LongTermPrObject { get; set; }

        /// <summary>
        ///  Дата сбора взносов
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// ЛС собственника дома
        /// </summary>
        public virtual string PersonalAccount { get; set; }

        /// <summary>
        /// Площадь помещения собственника ЛС
        /// </summary>
        public virtual decimal AreaOwnerAccount { get; set; }
    }
}