namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Enums;
    using Entities;

    /// <summary>
    /// Отчет по спецсчетам
    /// </summary>
    public class SpecialAccountRow : BaseGkhEntity
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
        /// Рассчетная площадь
        /// </summary>
        public virtual Decimal Tariff { get; set; }

        // <summary>
        /// Рассчетная площадь
        /// </summary>
        public virtual Decimal AccuracyArea { get; set; }

        // <summary>
        /// Поступление взносов
        /// </summary>
        public virtual Decimal Incoming { get; set; }

        // <summary>
        /// Размер задолжнности за все время 
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

        // <summary>
        ///Сведения о начислении взносов в отчетном периоде 
        /// </summary>
        public virtual Decimal Accured { get; set; }

        // <summary>
        //Всего оплачено с начала наступления обязанности внесения взносов 
        /// </summary>
        public virtual Decimal IncomingTotal { get; set; }

        // <summary>
        //Сведения о размере израсходованных средств 
        /// </summary>
        public virtual Decimal TransferTotal { get; set; }

        // <summary>
        //Сведения о заключении договора займа и(или) кредитного договора
        /// </summary>
        public virtual String Contracts { get; set; }

        // <summary>
        ///Всего начислено с начала наступления обязанности внесения взносов 
        /// </summary>
        public virtual Decimal AccuredTotal { get; set; }

        // <summary>
        /// Начало начислений
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        public virtual decimal AmountDebtForPeriod { get; set; }

        public virtual decimal AmountDebtCredit { get; set; }
    }
}