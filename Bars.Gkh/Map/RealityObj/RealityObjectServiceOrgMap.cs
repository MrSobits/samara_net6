/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Обслуживающая организация жилого дома"
///     /// </summary>
///     public class RealityObjectServiceOrgMap : BaseGkhEntityMap<RealityObjectServiceOrg>
///     {
///         public RealityObjectServiceOrgMap() : base("GKH_OBJ_SERVICE_ORG")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.TypeServiceOrg, "TYPE_SERV_ORG").Not.Nullable().CustomType<TypeServiceOrg>();
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Organization, "CONTRAGENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Поставщик коммунальных услуг жилого дома"</summary>
    public class RealityObjectServiceOrgMap : BaseImportableEntityMap<RealityObjectServiceOrg>
    {
        
        public RealityObjectServiceOrgMap() : 
                base("Поставщик коммунальных услуг жилого дома", "GKH_OBJ_SERVICE_ORG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Название документа").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.TypeServiceOrg, "Тип поставщика услуги").Column("TYPE_SERV_ORG").NotNull();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Organization, "Контрагент поставщика").Column("CONTRAGENT_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
