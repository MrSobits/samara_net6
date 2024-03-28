/// <mapping-converter-backup>
/// namespace Bars.Gkh.Reforma.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Reforma.Entities;
/// 
///     public class RefFileMap : PersistentObjectMap<RefFile>
///     {
///         public RefFileMap()
///             : base("RFRM_FILE")
///         {
///             this.Map(x => x.ExternalId, "EXTERNAL_ID", true);
///             this.References(x => x.FileInfo, "FILE_INFO_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Reforma.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Reforma.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Reforma.Entities.RefFile"</summary>
    public class RefFileMap : PersistentObjectMap<RefFile>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RefFileMap() : 
                base("Bars.Gkh.Reforma.Entities.RefFile", "RFRM_FILE")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.FileInfo, "FileInfo").Column("FILE_INFO_ID");
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").NotNull();
            this.Reference(x => x.ReportingPeriod, "ReportingPeriod").Column("REPORTING_PERIOD_ID").NotNull();
        }
    }
}
