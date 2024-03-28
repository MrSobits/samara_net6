/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RegOpPersAccMunicipalityMap : BaseImportableEntityMap<RegOpPersAccMunicipality>
///     {
///         public RegOpPersAccMunicipalityMap()
///             : base("OVRHL_REG_OP_PERS_ACC_MU")
///         {
///             Map(x => x.OwnerFio, "OWNER_FIO", false, 150);
///             Map(x => x.PaidContributions, "PAID_CONTRIB");
///             Map(x => x.CreditContributions, "CREDIT_CONTRIB");
///             Map(x => x.CreditPenalty, "CREDIT_PENALTY");
///             Map(x => x.PaidPenalty, "PAID_PENALTY");
///             Map(x => x.SubsidySumLocalBud, "SUM_LOCAL_BUD");
///             Map(x => x.SubsidySumSubjBud, "SUM_SUBJ_BUD");
///             Map(x => x.SubsidySumFedBud, "SUM_FED_BUD");
///             Map(x => x.SumAdopt, "SUM_ADOPT");
///             Map(x => x.RepaySumAdopt, "REPAY_SUM_ADOPT");
///             Map(x => x.PersAccountNum, "ACC_NUMBER", false, 50);
/// 
///             References(x => x.Municipality, "MU_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RegOperator, "REG_OP_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Лицевые счета по муниципальным образованиям"</summary>
    public class RegOpPersAccMunicipalityMap : BaseImportableEntityMap<RegOpPersAccMunicipality>
    {
        
        public RegOpPersAccMunicipalityMap() : 
                base("Лицевые счета по муниципальным образованиям", "OVRHL_REG_OP_PERS_ACC_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RegOperator, "Региональный оператор").Column("REG_OP_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MU_ID").NotNull().Fetch();
            Property(x => x.PersAccountNum, "Номер лицевого счета").Column("ACC_NUMBER").Length(50);
            Property(x => x.OwnerFio, "ФИО собственника").Column("OWNER_FIO").Length(150);
            Property(x => x.PaidContributions, "Оплачено взносов").Column("PAID_CONTRIB");
            Property(x => x.CreditContributions, "Начислено взносов").Column("CREDIT_CONTRIB");
            Property(x => x.CreditPenalty, "Начислено пени").Column("CREDIT_PENALTY");
            Property(x => x.PaidPenalty, "Оплачено пени").Column("PAID_PENALTY");
            Property(x => x.SubsidySumLocalBud, "Сумма субсидии из МБ (местный бюджет),руб").Column("SUM_LOCAL_BUD");
            Property(x => x.SubsidySumSubjBud, "Сумма субсидии из БС (бюджет субъекта),руб").Column("SUM_SUBJ_BUD");
            Property(x => x.SubsidySumFedBud, "Сумма субсидии из ФБ (федеральный бюджет),руб").Column("SUM_FED_BUD");
            Property(x => x.SumAdopt, "Сумма заимствования, руб").Column("SUM_ADOPT");
            Property(x => x.RepaySumAdopt, "Погашенная сумма заимствования,руб").Column("REPAY_SUM_ADOPT");
        }
    }
}
