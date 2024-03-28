/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class ControlTransitAccDebetProxyMap : BaseImportableEntityMap<ControlTransitAccDebetProxy>
///     {
///         public ControlTransitAccDebetProxyMap() : base("REGOP_TR_ACC_DEB")
///         {
///             Map(x => x.Number, "ACC_NUM");
///             Map(x => x.Date, "D_DATE");
///             Map(x => x.PaymentAgentName, "P_AGENT_NAME");
///             Map(x => x.PaymentAgentCode, "P_AGENT_CODE");
///             Map(x => x.Sum, "D_SUM");
///             Map(x => x.ConfirmSum, "CONF_SUM");
///             Map(x => x.Divergence, "DIVERGENCE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.ControlTransitAccDebetProxy"</summary>
    public class ControlTransitAccDebetProxyMap : BaseImportableEntityMap<ControlTransitAccDebetProxy>
    {
        
        public ControlTransitAccDebetProxyMap() : 
                base("Bars.Gkh.RegOperator.Entities.ControlTransitAccDebetProxy", "REGOP_TR_ACC_DEB")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Number, "Number").Column("ACC_NUM").Length(250);
            Property(x => x.Date, "Date").Column("D_DATE");
            Property(x => x.PaymentAgentName, "PaymentAgentName").Column("P_AGENT_NAME").Length(250);
            Property(x => x.PaymentAgentCode, "PaymentAgentCode").Column("P_AGENT_CODE").Length(250);
            Property(x => x.Sum, "Sum").Column("D_SUM");
            Property(x => x.ConfirmSum, "ConfirmSum").Column("CONF_SUM");
            Property(x => x.Divergence, "Divergence").Column("DIVERGENCE");
        }
    }
}
