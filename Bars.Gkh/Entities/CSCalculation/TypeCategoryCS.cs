using Bars.B4.DataAccess;
using System;
using System.Collections.Generic;

namespace Bars.Gkh.Entities
{   

    /// <summary>
    /// Тип категории благоустройства
    /// </summary>
    public class TypeCategoryCS : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }       

    }
}