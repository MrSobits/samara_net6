namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Протокол собрания жильцов
    /// </summary>
    public class RealityObjectProtocol : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        public virtual FileInfo File { get; set; }

        public virtual string DocumentName { get; set; }

        public virtual string DocumentNum { get; set; }
        
        public virtual DateTime? DateFrom { get; set; }

        public virtual CouncilResult CouncilResult { get; set; }
    }
}