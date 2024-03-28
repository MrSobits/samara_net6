namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Взнос на капремонт дома, у которого в протоколе решений
    /// действующее решение о способе формирования фонда на спец.счете и владелец НЕ регоператор
    /// </summary>
    public class PaymentCrSpecAccNotRegop : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Период начислений
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Дата ввода
        /// </summary>
        public virtual DateTime? InputDate { get; set; }

        /// <summary>
        /// Сумма поступления
        /// </summary>
        public virtual decimal? AmountIncome { get; set; }

        /// <summary>
        /// Остаток на конец года
        /// </summary>
        public virtual decimal? EndYearBalance { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}