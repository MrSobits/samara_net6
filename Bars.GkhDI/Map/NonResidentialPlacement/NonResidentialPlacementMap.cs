/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения об использование нежилых помещений"
///     /// </summary>
///     public class NonResidentialPlacementMap : BaseGkhEntityMap<NonResidentialPlacement>
///     {
///         public NonResidentialPlacementMap()
///             : base("DI_DISINFO_RO_NONRESPLACE")
///         {
///             Map(x => x.ContragentName, "CONTRAGENT_NAME").Length(300);
///             Map(x => x.Area, "AREA");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.TypeContragentDi, "TYPE_CONTRAGENT").CustomType<TypeContragentDi>().Not.Nullable();
///             Map(x => x.DocumentNumApartment, "DOC_NUM_APARTMENT").Length(50);
///             Map(x => x.DocumentDateApartment, "DOC_DATE_APARTMENT");
///             Map(x => x.DocumentNameApartment, "DOC_NAME_APARTMENT").Length(300);
///             Map(x => x.DocumentNumCommunal, "DOC_NUM_COMMUNAL").Length(50);
///             Map(x => x.DocumentDateCommunal, "DOC_DATE_COMMUNAL");
///             Map(x => x.DocumentNameCommunal, "DOC_NAME_COMMUNAL").Length(300);
/// 
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.NonResidentialPlacement"</summary>
    public class NonResidentialPlacementMap : BaseImportableEntityMap<NonResidentialPlacement>
    {
        
        public NonResidentialPlacementMap() : 
                base("Bars.GkhDi.Entities.NonResidentialPlacement", "DI_DISINFO_RO_NONRESPLACE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.ContragentName, "ContragentName").Column("CONTRAGENT_NAME").Length(300);
            Property(x => x.Area, "Area").Column("AREA");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.TypeContragentDi, "TypeContragentDi").Column("TYPE_CONTRAGENT").NotNull();
            Property(x => x.DocumentNumApartment, "DocumentNumApartment").Column("DOC_NUM_APARTMENT").Length(50);
            Property(x => x.DocumentDateApartment, "DocumentDateApartment").Column("DOC_DATE_APARTMENT");
            Property(x => x.DocumentNameApartment, "DocumentNameApartment").Column("DOC_NAME_APARTMENT").Length(300);
            Property(x => x.DocumentNumCommunal, "DocumentNumCommunal").Column("DOC_NUM_COMMUNAL").Length(50);
            Property(x => x.DocumentDateCommunal, "DocumentDateCommunal").Column("DOC_DATE_COMMUNAL");
            Property(x => x.DocumentNameCommunal, "DocumentNameCommunal").Column("DOC_NAME_COMMUNAL").Length(300);
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
        }
    }
}
