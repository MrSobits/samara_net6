
namespace Sobits.GisGkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    public class GisGkhRegionalProgramCR : BaseEntity
    {
        /// <summary>
        /// ГИС ЖКХ Transoirt Guid
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Guid
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// Признак работы с программой в ГИС ЖКХ
        /// </summary>
        public virtual bool WorkWith { get; set; }
    }
}
