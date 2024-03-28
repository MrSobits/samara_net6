/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.PersonalAccount;
/// 
///     public class PersonalAccountBenefitsMap : BaseImportableEntityMap<PersonalAccountBenefits>
///     {
///         public PersonalAccountBenefitsMap()
///             : base("REGOP_PERS_ACC_BENEFITS")
///         {
///             Map(x => x.Sum, "SUM", true, 0M);
/// 
///             References(x => x.PersonalAccount, "PERS_ACC_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Period, "PERIOD_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Информация по начисленным льготам"</summary>
    public class PersonalAccountBenefitsMap : BaseImportableEntityMap<PersonalAccountBenefits>
    {
        
        public PersonalAccountBenefitsMap() : 
                base("Информация по начисленным льготам", "REGOP_PERS_ACC_BENEFITS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PersonalAccount, "Лицевой счет").Column("PERS_ACC_ID").NotNull().Fetch();
            Property(x => x.Sum, "Сумма").Column("SUM").DefaultValue(0m).NotNull();
            Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull().Fetch();
        }
    }
}
