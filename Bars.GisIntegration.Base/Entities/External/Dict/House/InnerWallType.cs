﻿namespace Bars.GisIntegration.Base.Entities.External.Dict.House
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Тип внутренних стен
    /// </summary>
    public class InnerWallType : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string DictCode { get; set; }
        /// <summary>
        /// ГУИД из ГИС
        /// </summary>
        public virtual string GisGuid { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
