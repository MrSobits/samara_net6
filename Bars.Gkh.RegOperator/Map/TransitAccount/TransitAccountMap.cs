/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class TransitAccountMap : BaseImportableEntityMap<TransitAccount>
///     {
///         public TransitAccountMap() : base("REGOP_TRANSIT_ACC")
///         {
///             Map(x => x.Sum, "R_SUM");
///             Map(x => x.Date, "R_DATE");
///             Map(x => x.Number, "R_NUMBER");
///             References(x => x.PaymentAgent, "PAYMENT_AGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.TransitAccount"</summary>
    public class TransitAccountMap : BaseImportableEntityMap<TransitAccount>
    {
        
        public TransitAccountMap() : 
                base("Bars.Gkh.RegOperator.Entities.TransitAccount", "REGOP_TRANSIT_ACC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PaymentAgent, "PaymentAgent").Column("PAYMENT_AGENT_ID").NotNull().Fetch();
            Property(x => x.Number, "Number").Column("R_NUMBER").Length(250);
            Property(x => x.Date, "Date").Column("R_DATE");
            Property(x => x.Sum, "Sum").Column("R_SUM");
        }
    }
}
