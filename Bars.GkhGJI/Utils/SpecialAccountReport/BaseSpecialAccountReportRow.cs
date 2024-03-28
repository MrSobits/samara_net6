namespace Bars.GkhGji
{
    using System;

    using B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    public class BaseSpecialAccountReportRow : BaseEntity
    {
        /// <summary>
        /// SpecialAccountReport
        /// </summary>
        public virtual SpecialAccountReport SpecialAccountReport { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Ro
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номер специального счета
        /// </summary>
        public virtual string SpecialAccountNum { get; set; }

        // <summary>
        /// Поступление взносов
        /// </summary>
        public virtual Decimal Incoming { get; set; }

        // <summary>
        /// Размер задолжнности
        /// </summary>
        public virtual Decimal AmmountDebt { get; set; }

        // <summary>
        /// Размер остатка
        /// </summary>
        public virtual Decimal Ballance { get; set; }

        // <summary>
        /// Размер перечислений
        /// </summary>
        public virtual Decimal Transfer { get; set; }

    }

    //public class BaseSpecialAccountReportRow<T> : BaseSpecialAccountReportRow where T : SpecialAccountReport
    //{
    //    public virtual T SpecialAccountReport2 { get; set; }

    //    /// <summary>Не использовать в запросах к БД</summary>
    //    public override SpecialAccountReport SpecialAccountReport
    //    {
    //        get { return SpecialAccountReport2; }
    //        set { SpecialAccountReport2 = (T)value; }
    //    }
    //}
}