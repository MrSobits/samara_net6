
namespace Sobits.GisGkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Enums;
    public class GisGkhVersionRecord : BaseEntity
    {
        /// <summary>
        /// ГИС ЖКХ долгосрочка
        /// </summary>
        public virtual GisGkhRegionalProgramCR GisGkhRegionalProgramCR { get; set; }

        /// <summary>
        /// Работа долгосрочки
        /// </summary>
        public virtual VersionRecord VersionRecord { get; set; }

        /// <summary>
        /// Работа долгосрочки - stage1
        /// </summary>
        public virtual VersionRecordStage1 VersionRecordStage1 { get; set; }
        
        /// <summary>
        /// ГИС ЖКХ Transport Guid
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Guid
        /// </summary>
        public virtual string GisGkhGuid { get; set; }
    }
}
