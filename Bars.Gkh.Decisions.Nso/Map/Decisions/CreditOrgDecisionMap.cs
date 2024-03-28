/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CreditOrgDecisionMap : BaseJoinedSubclassMap<CreditOrgDecision>
///     {
///         public CreditOrgDecisionMap() : base("DEC_CREDIT_ORG", "ID")
///         {
///             Map(x => x.BankAccountNumber, "BANK_ACC_NUM");
/// 
///             References(x => x.Decision, "CREDIT_ORG_ID", ReferenceMapConfig.Fetch);
///             References(x => x.BankFile, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Решение о выборе кредитной организации"</summary>
    public class CreditOrgDecisionMap : JoinedSubClassMap<CreditOrgDecision>
    {
        
        public CreditOrgDecisionMap() : 
                base("Решение о выборе кредитной организации", "DEC_CREDIT_ORG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Decision, "Кредитная организация").Column("CREDIT_ORG_ID").Fetch();
            Reference(x => x.BankFile, "Банковская информация").Column("FILE_ID").Fetch();
            Property(x => x.BankAccountNumber, "Номер счета в банке").Column("BANK_ACC_NUM").Length(250);
        }
    }
}
