/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities;
/// 
///     public class MonthlyFeeAmountDecHistoryMap : BaseImportableEntityMap<MonthlyFeeAmountDecHistory>
///     {
///         public MonthlyFeeAmountDecHistoryMap()
///             : base("DEC_MONTHLY_FEE_HISTORY")
///         {
///             Property(x => x.Decision, m =>
///             {
///                 m.Column("DECISION_VALUE");
///                 m.NotNullable(false);
///                 m.Type<ImprovedJsonSerializedType<List<PeriodMonthlyFee>>>();
///             });
/// 
///             References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.UserName, "USER_NAME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Map;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Размер ежемесячного взноса на КР"</summary>
    public class MonthlyFeeAmountDecHistoryMap : BaseImportableEntityMap<MonthlyFeeAmountDecHistory>
    {
        
        public MonthlyFeeAmountDecHistoryMap() : 
                base("Размер ежемесячного взноса на КР", "DEC_MONTHLY_FEE_HISTORY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decision, "Помесячные взносы").Column("DECISION_VALUE");
            Property(x => x.UserName, "Ответственный").Column("USER_NAME").Length(250);
            Reference(x => x.Protocol, "Решения собственников жилья").Column("PROTOCOL_ID").NotNull().Fetch();
        }
    }

    public class MonthlyFeeAmountDecHistoryNHibernateMapping : ClassMapping<MonthlyFeeAmountDecHistory>
    {
        public MonthlyFeeAmountDecHistoryNHibernateMapping()
        {
            Property(
                x => x.Decision,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<List<PeriodMonthlyFee>>>();
                    });
        }
    }
}
