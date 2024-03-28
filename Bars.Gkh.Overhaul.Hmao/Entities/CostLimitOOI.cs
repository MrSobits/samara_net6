using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.CommonEstateObject;
using System;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    /// <summary>
    /// Предельная стоимость услуги в разрезе оои
    /// </summary>
    public class CostLimitOOI : BaseEntity
    {
        /// <summary>
        /// ООИ
        /// </summary>
        public virtual CommonEstateObject CommonEstateObject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal Cost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual short? FloorStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual short? FloorEnd { get; set; }

    }
}
