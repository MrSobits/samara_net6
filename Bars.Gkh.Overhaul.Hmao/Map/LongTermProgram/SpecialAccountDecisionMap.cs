/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class SpecialAccountDecisionMap : BaseJoinedSubclassMap<SpecialAccountDecision>
///     {
///         public SpecialAccountDecisionMap()
///             : base("OVRHL_PR_DEC_SPEC_ACC", "ID")
///         {
///             Map(x => x.TypeOrganization, "TYPE_ORGANIZATION", true);
///             Map(x => x.AccountNumber, "ACC_NUMBER");
///             Map(x => x.OpenDate, "OPEN_DATE");
///             Map(x => x.CloseDate, "CLOSE_DATE");
/// 
///             References(x => x.BankHelpFile, "HELP_FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.CreditOrg, "CREDIT_ORG_ID", ReferenceMapConfig.Fetch);
///             //References(x => x.RegOperator, "REG_OPERATOR_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ManagingOrganization, "MANAGING_ORG_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Решение собственников помещений МКД (при формирования фонда КР на спец.счете)"</summary>
    public class SpecialAccountDecisionMap : JoinedSubClassMap<SpecialAccountDecision>
    {
        
        public SpecialAccountDecisionMap() : 
                base("Решение собственников помещений МКД (при формирования фонда КР на спец.счете)", "OVRHL_PR_DEC_SPEC_ACC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeOrganization, "Тип организации-владельца").Column("TYPE_ORGANIZATION").NotNull();
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAGING_ORG_ID").Fetch();
            Property(x => x.AccountNumber, "Номер счета").Column("ACC_NUMBER").Length(250);
            Property(x => x.OpenDate, "Дата открытия").Column("OPEN_DATE");
            Property(x => x.CloseDate, "Дата закрытия").Column("CLOSE_DATE");
            Reference(x => x.BankHelpFile, "Файл справки банка").Column("HELP_FILE_ID").Fetch();
            Reference(x => x.CreditOrg, "Кредитная организация").Column("CREDIT_ORG_ID").Fetch();
        }
    }
}
