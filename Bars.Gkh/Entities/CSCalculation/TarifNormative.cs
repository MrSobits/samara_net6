using Bars.B4.DataAccess;
using System;
using System.Collections.Generic;

namespace Bars.Gkh.Entities
{   

    /// <summary>
    /// Тариыф/нормативы
    /// </summary>
    public class TarifNormative : BaseEntity
    {

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// единица изменения
        /// </summary>
        public virtual string UnitMeasure { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual decimal Value { get; set; }

        /// <summary>
        ///МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime DateFrom { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateTo { get; set; }

        /// <summary>
        ///Категория МКД
        /// </summary>
        public virtual CategoryCSMKD CategoryCSMKD { get; set; }

    }
}