namespace Bars.GkhCr.Entities
{
    using B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using System;

    /// <summary>
    /// CrFileRegister
    /// </summary>
    public class CrFileRegister : BaseGkhEntity
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