namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.Gkh.Entities;
    using System;
    using Enums;

    /// <summary>
    /// Виды КНД
    /// </summary>
    public class KindKNDDict : BaseGkhEntity
    {
        /// <summary>
        /// Тип КНД
        /// </summary>
        public virtual KindKND KindKND { get; set; }

        /// <summary>
        /// Дата действия с
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата действия по
        /// </summary>
        public virtual DateTime? DateTo { get; set; }

       
    }
}