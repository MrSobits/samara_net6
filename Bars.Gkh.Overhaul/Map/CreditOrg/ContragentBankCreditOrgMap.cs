/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Entities;
///     using FluentNHibernate.Mapping;
/// 
///     public class ContragentBankCreditOrgMap : SubclassMap<ContragentBankCreditOrg>
///     {
///         public ContragentBankCreditOrgMap()
///         {
///             Table("GKH_CONTR_BANK_CR_ORG");
///             KeyColumn("ID");
/// 
///             References(x => x.CreditOrg, "CREDIT_ORG_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Entities.ContragentBankCreditOrg"</summary>
    public class ContragentBankCreditOrgMap : JoinedSubClassMap<ContragentBankCreditOrg>
    {
        
        public ContragentBankCreditOrgMap() : 
                base("Bars.Gkh.Overhaul.Entities.ContragentBankCreditOrg", "GKH_CONTR_BANK_CR_ORG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CreditOrg, "CreditOrg").Column("CREDIT_ORG_ID").Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
