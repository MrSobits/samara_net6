/// <mapping-converter-backup>
/// namespace Bars.Gkh1468.Map.Passport
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh1468.Entities;
/// 
///     public class OkiPassportMap : BaseEntityMap<OkiPassport>
///     {
///         public OkiPassportMap() : base("GKH_OKI_PASSPORT")
///         {
///             Map(x => x.ReportYear, "REP_YEAR", true);
///             Map(x => x.ReportMonth, "REP_MONTH", true);
///             References(x => x.Municipality, "MUNICIPALITY_ID",  ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Xml, "XML_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Signature, "SIGNATURE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Pdf, "PDF_ID", ReferenceMapConfig.Fetch);
///             Map(x => x.Percent, "PERCENT", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Сводный паспорт ОКИ"</summary>
    public class OkiPassportMap : BaseEntityMap<OkiPassport>
    {
        
        public OkiPassportMap() : 
                base("Сводный паспорт ОКИ", "GKH_OKI_PASSPORT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ReportYear, "ReportYear").Column("REP_YEAR").NotNull();
            Property(x => x.ReportMonth, "ReportMonth").Column("REP_MONTH").NotNull();
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.Xml, "Xml").Column("XML_ID").Fetch();
            Reference(x => x.Signature, "Signature").Column("SIGNATURE_ID").Fetch();
            Reference(x => x.Pdf, "Pdf").Column("PDF_ID").Fetch();
            Property(x => x.Percent, "Percent").Column("PERCENT").NotNull();
        }
    }
}
