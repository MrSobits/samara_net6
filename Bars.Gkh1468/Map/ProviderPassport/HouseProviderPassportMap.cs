/// <mapping-converter-backup>
/// namespace Bars.Gkh1468.Map.ProviderPassport
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh1468.Entities;
/// 
///     public class HouseProviderPassportMap : BaseEntityMap<HouseProviderPassport>
///     {
///         public HouseProviderPassportMap() : base("GKH_HOUSE_PROV_PASSPORT")
///         {
///             References(x => x.HousePassport, "HOUSE_PASP_ID", ReferenceMapConfig.NotNull);
///             
///             Map(x => x.ReportYear, "REP_YEAR", true);
///             Map(x => x.ReportMonth, "REP_MONTH", true);
/// 
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNull);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.NotNullAndFetch);
///             
///             Map(x => x.HouseType, "HOUSE_TYPE", true);
///             Map(x => x.ContragentType, "CONTRAGENT_TYPE", true);
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNull);
///             References(x => x.Xml, "XML_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Signature, "SIGNATURE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Pdf, "PDF_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Certificate, "CERTIFICATE_ID", ReferenceMapConfig.Fetch);
///             
///             Map(x => x.Percent, "PERCENT", true, 0);
///             Map(x => x.SignDate, "SIGN_DATE", false);
///             Map(x => x.UserName, "USER_NAME", false);
/// 
///             References(x => x.PassportStruct, "PASSPORT_STRUCT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Паспорт поставщика дома"</summary>
    public class HouseProviderPassportMap : BaseEntityMap<HouseProviderPassport>
    {
        
        public HouseProviderPassportMap() : 
                base("Паспорт поставщика дома", "GKH_HOUSE_PROV_PASSPORT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.HousePassport, "Паспорт дома").Column("HOUSE_PASP_ID").NotNull();
            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull();
            Property(x => x.HouseType, "Тип дома").Column("HOUSE_TYPE").NotNull();
            Property(x => x.UserName, "Последний изменивший статус пользователь").Column("USER_NAME").Length(250);
            Property(x => x.ReportYear, "ReportYear").Column("REP_YEAR").NotNull();
            Property(x => x.ReportMonth, "ReportMonth").Column("REP_MONTH").NotNull();
            Reference(x => x.State, "State").Column("STATE_ID").NotNull().Fetch();
            Property(x => x.ContragentType, "ContragentType").Column("CONTRAGENT_TYPE").NotNull();
            Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.Xml, "Xml").Column("XML_ID").Fetch();
            Reference(x => x.Signature, "Signature").Column("SIGNATURE_ID").Fetch();
            Reference(x => x.Pdf, "Pdf").Column("PDF_ID").Fetch();
            Reference(x => x.Certificate, "Certificate").Column("CERTIFICATE_ID").Fetch();
            Property(x => x.Percent, "Percent").Column("PERCENT").NotNull();
            Reference(x => x.PassportStruct, "PassportStruct").Column("PASSPORT_STRUCT_ID").Fetch();
            Property(x => x.SignDate, "SignDate").Column("SIGN_DATE");
        }
    }
}
