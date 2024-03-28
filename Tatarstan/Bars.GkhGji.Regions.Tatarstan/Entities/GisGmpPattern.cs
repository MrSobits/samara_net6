using System;
using Bars.B4.DataAccess;
using Bars.Gkh.Entities;

namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    /// <summary>
    /// Сущность для связки Муниципальное образование - Код шаблона
    /// </summary>
    public class GisGmpPattern : BaseEntity
    {
        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }
        
        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        /// <summary>
        /// Код шаблона
        /// </summary>
        public virtual string PatternCode { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}