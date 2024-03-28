namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Расчетный счет регоператора"</summary>
    public class RegopCalcAccountMap : JoinedSubClassMap<RegopCalcAccount>
    {
        
        public RegopCalcAccountMap() : 
                base("Расчетный счет регоператора", "REGOP_CALC_ACC_REGOP")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.ContragentCreditOrg, "Расчетный счет (Кредитная организация контрагента)").Column("CONTR_CREDIT_ORG_ID").Fetch();
            this.Property(x => x.IsTransit, "счет является транзитным").Column("IS_TRANSIT");
        }
    }
}
