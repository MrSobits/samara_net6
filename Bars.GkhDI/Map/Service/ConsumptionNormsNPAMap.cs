/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Entities;
///     using Enums;
/// 
///     public class ConsumptionNormsNpaMap : BaseGkhEntityMap<ConsumptionNormsNpa>
///     {
///         public ConsumptionNormsNpaMap()
///             : base("DI_CONSUMPTION_NORMS_NPA")
///         {
///             Map(x => x.NpaDate, "CONS_NORM_NPA_DATE");
///             Map(x => x.NpaNumber, "CONS_NORM_NPA_NUMBER").Length(300);
///             Map(x => x.NpaAcceptor, "CONS_NORM_NPA_ACCEPTOR").Length(300);
/// 
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.ConsumptionNormsNpa"</summary>
    public class ConsumptionNormsNpaMap : BaseImportableEntityMap<ConsumptionNormsNpa>
    {
        
        public ConsumptionNormsNpaMap() : 
                base("Bars.GkhDi.Entities.ConsumptionNormsNpa", "DI_CONSUMPTION_NORMS_NPA")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.NpaDate, "NpaDate").Column("CONS_NORM_NPA_DATE");
            Property(x => x.NpaNumber, "NpaNumber").Column("CONS_NORM_NPA_NUMBER").Length(300);
            Property(x => x.NpaAcceptor, "NpaAcceptor").Column("CONS_NORM_NPA_ACCEPTOR").Length(300);
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
