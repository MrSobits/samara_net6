/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Земельный участок жилого дома"
///     /// </summary>
///     public class RealityObjectLandMap : BaseGkhEntityMap<RealityObjectLand>
///     {
///         public RealityObjectLandMap() : base("GKH_OBJ_LAND")
///         {
///             Map(x => x.CadastrNumber, "CADASTR_NUM").Length(300);
///             Map(x => x.DateLastRegistration, "DATE_LAST_REG");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Земельный участок жилого дома"</summary>
    public class RealityObjectLandMap : BaseImportableEntityMap<RealityObjectLand>
    {
        
        public RealityObjectLandMap() : 
                base("Земельный участок жилого дома", "GKH_OBJ_LAND")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.CadastrNumber, "Кадастровый номер").Column("CADASTR_NUM").Length(300);
            Property(x => x.DateLastRegistration, "Дата регистрации").Column("DATE_LAST_REG");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(300);
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
