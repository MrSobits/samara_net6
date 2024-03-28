namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using System;

    /// <summary>
    /// FileRegister
    /// </summary>
    public class FileRegister : BaseGkhEntity
    {
        /// <summary>
        /// RealityObject
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Архив
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Дата с
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата по
        /// </summary>
        public virtual DateTime? DateTo { get; set; }
    }
}