namespace Bars.GkhGji.Entities
{
    using System;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionLongText : BaseEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        public virtual byte[] Established { get; set; }

    }
}