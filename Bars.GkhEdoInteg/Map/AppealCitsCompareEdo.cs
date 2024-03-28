/// <mapping-converter-backup>
/// namespace Bars.GkhEdoInteg.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhEdoInteg.Entities;
/// 
///     public class AppealCitsCompareEdoMap : BaseGkhEntityMap<AppealCitsCompareEdo>
///     {
///         public AppealCitsCompareEdoMap()
///             : base("INTGEDO_APPCITS")
///         {
///             References(x => x.AppealCits, "APPEAL_CITS_ID").Not.Nullable().Fetch.Join();
/// 
///             Map(x => x.AddressEdo, "ADDRESS_EDO").Length(2000);
///             Map(x => x.CodeEdo, "CODE_EDO").Not.Nullable();
///             Map(x => x.IsEdo, "IS_EDO").Not.Nullable();
///             Map(x => x.DateActual, "DATE_ACTUAL");
/// 
///             Map(x => x.DateDocLoad, "DATE_LOAD_DOC");
///             Map(x => x.IsDocEdo, "IS_DOC_EDO");
/// 
///             Map(x => x.CountLoadDoc, "COUNT_LOAD_DOC");
///             Map(x => x.MsgLoadDoc, "MSG_LOAD_DOC");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.AppealCitsCompareEdo"</summary>
    public class AppealCitsCompareEdoMap : BaseEntityMap<AppealCitsCompareEdo>
    {
        
        public AppealCitsCompareEdoMap() : 
                base("Bars.GkhEdoInteg.Entities.AppealCitsCompareEdo", "INTGEDO_APPCITS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.AddressEdo, "AddressEdo").Column("ADDRESS_EDO").Length(2000);
            Property(x => x.CodeEdo, "CodeEdo").Column("CODE_EDO").NotNull();
            Property(x => x.IsEdo, "IsEdo").Column("IS_EDO").NotNull();
            Property(x => x.DateActual, "DateActual").Column("DATE_ACTUAL");
            Property(x => x.DateDocLoad, "DateDocLoad").Column("DATE_LOAD_DOC");
            Property(x => x.IsDocEdo, "IsDocEdo").Column("IS_DOC_EDO");
            Property(x => x.CountLoadDoc, "CountLoadDoc").Column("COUNT_LOAD_DOC");
            Property(x => x.MsgLoadDoc, "MsgLoadDoc").Column("MSG_LOAD_DOC");
            Reference(x => x.AppealCits, "AppealCits").Column("APPEAL_CITS_ID").NotNull().Fetch();
        }
    }
}
