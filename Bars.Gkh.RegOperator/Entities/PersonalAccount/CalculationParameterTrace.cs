namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using System.Collections.Generic;
    using B4.DataAccess;

    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    /// Трассировка параметров расчетов ЛС
    /// </summary>
    public class CalculationParameterTrace : BaseEntity
    {
        /// <summary>
        /// Тип расчета
        /// </summary>
        public virtual CalculationTraceType CalculationType { get; set; }

        /// <summary>
        /// Словарь значений параметров на шаг расчета
        /// </summary>
        public virtual Dictionary<string, object> ParameterValues { get; set; }

        /// <summary>
        /// Дата начала действия параметров
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия параметров
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Гуид связи с неподтвержденным начислением и боевым 
        /// </summary>
        public virtual string CalculationGuid { get; set; }

        /// <summary>
        /// Период расчета
        /// </summary>
        public virtual ChargePeriod ChargePeriod { get; set; }

        public CalculationParameterTrace()
        {
            ParameterValues = new Dictionary<string, object>();
        }
    }
}