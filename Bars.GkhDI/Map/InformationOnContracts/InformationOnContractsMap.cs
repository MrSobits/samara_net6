/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using B4.DataAccess;
/// 
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     using Entities;
/// 
///     public class InformationOnContractsMap : BaseGkhEntityMap<InformationOnContracts>
///     {
///         public InformationOnContractsMap()
///             : base("DI_DISINFO_ON_CONTRACTS")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Number, "NUM").Length(300);
///             Map(x => x.From, "DATE_FROM");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.PartiesContract, "PARTIES_CONTRACT").Length(1000);
///             Map(x => x.Comments, "COMMENTS").Length(1000);
///             Map(x => x.Cost, "COST");
///             
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.InformationOnContracts"</summary>
    public class InformationOnContractsMap : BaseImportableEntityMap<InformationOnContracts>
    {
        
        public InformationOnContractsMap() : 
                base("Bars.GkhDi.Entities.InformationOnContracts", "DI_DISINFO_ON_CONTRACTS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.Number, "Number").Column("NUM").Length(300);
            Property(x => x.From, "From").Column("DATE_FROM");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.PartiesContract, "PartiesContract").Column("PARTIES_CONTRACT").Length(1000);
            Property(x => x.Comments, "Comments").Column("COMMENTS").Length(1000);
            Property(x => x.Cost, "Cost").Column("COST");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJ_ID").NotNull().Fetch();
        }
    }
}
