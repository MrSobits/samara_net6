namespace Bars.GkhGji.Regions.Saha.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;

    public class SahaResolutionDefinition : ResolutionDefinition
    {
        /// <summary>
        /// Время начала
        /// </summary>
        public virtual DateTime? TimeStart { get; set; }

        /// <summary>
        /// Время окончания
        /// </summary>
        public virtual DateTime? TimeEnd { get; set; }

        /// <summary>
        /// Фал описание
        /// </summary>
        public virtual FileInfo FileDescription { get; set; }
    }
}
