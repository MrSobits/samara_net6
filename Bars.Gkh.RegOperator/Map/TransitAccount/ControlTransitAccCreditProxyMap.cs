/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class ControlTransitAccCreditProxyMap : BaseImportableEntityMap<ControlTransitAccCreditProxy>
///     {
///         public ControlTransitAccCreditProxyMap()
///             : base("REGOP_TR_ACC_CRED")
///         {
///             Map(x => x.CreditOrgName, "CR_ORG");
///             Map(x => x.Date, "D_DATE");
///             Map(x => x.CalcAccount, "CALC_ACC");
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
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.ControlTransitAccCreditProxy"</summary>
    public class ControlTransitAccCreditProxyMap : BaseImportableEntityMap<ControlTransitAccCreditProxy>
    {
        
        public ControlTransitAccCreditProxyMap() : 
                base("Bars.Gkh.RegOperator.Entities.ControlTransitAccCreditProxy", "REGOP_TR_ACC_CRED")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Date, "Date").Column("D_DATE");
            Property(x => x.CreditOrgName, "CreditOrgName").Column("CR_ORG").Length(250);
            Property(x => x.CalcAccount, "CalcAccount").Column("CALC_ACC").Length(250);
            Property(x => x.Sum, "Sum").Column("D_SUM");
            Property(x => x.ConfirmSum, "ConfirmSum").Column("CONF_SUM");
            Property(x => x.Divergence, "Divergence").Column("DIVERGENCE");
        }
    }
}
