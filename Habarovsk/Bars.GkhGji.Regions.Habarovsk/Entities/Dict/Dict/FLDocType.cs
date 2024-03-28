﻿namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.B4.DataAccess;

    public class FLDocType : BaseEntity
    {
        /// <summary>
        /// Код региона
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        ///Наименование информационного центра
        /// </summary>
        public virtual string Name { get; set; }

    }
}
