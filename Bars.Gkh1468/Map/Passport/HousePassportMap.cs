/// <mapping-converter-backup>
/// namespace Bars.Gkh1468.Map.Passport
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh1468.Entities.Passport;
/// 
///     public class HousePassportMap : BaseEntityMap<HousePassport>
///     {
///         public HousePassportMap() : base("GKH_HOUSE_PASSPORT")
///         {
///             Map(x => x.ReportYear, "REP_YEAR");
///             Map(x => x.ReportMonth, "REP_MONTH");
///             Map(x => x.HouseType, "HOUSE_TYPE", true);
///             Map(x => x.Percent, "PERCENT", true, 0);
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Xml, "XML_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Signature, "SIGNATURE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Pdf, "PDF_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map.Passport
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities.Passport;
    
    
    /// <summary>Маппинг для "Сводный паспорт дома"</summary>
    public class HousePassportMap : BaseEntityMap<HousePassport>
    {
        
        public HousePassportMap() : 
                base("Сводный паспорт дома", "GKH_HOUSE_PASSPORT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ReportYear, "ReportYear").Column("REP_YEAR");
            Property(x => x.ReportMonth, "ReportMonth").Column("REP_MONTH");
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
            Property(x => x.HouseType, "HouseType").Column("HOUSE_TYPE").NotNull();
            Reference(x => x.Xml, "Xml").Column("XML_ID").Fetch();
            Reference(x => x.Signature, "Signature").Column("SIGNATURE_ID").Fetch();
            Reference(x => x.Pdf, "Pdf").Column("PDF_ID").Fetch();
            Property(x => x.Percent, "Percent").Column("PERCENT").NotNull();
        }
    }
}
