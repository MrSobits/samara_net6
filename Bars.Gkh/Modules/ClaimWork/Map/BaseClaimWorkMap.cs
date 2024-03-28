/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class BaseClaimWorkMap : BaseEntityMap<BaseClaimWork>
///     {
///         public BaseClaimWorkMap()
///             : base("CLW_CLAIM_WORK")
///         {
///             Map(x => x.ClaimWorkTypeBase, "TYPE_BASE", true, (object)10);
///             Map(x => x.StartingDate, "START_DATE");
///             Map(x => x.CountDaysDelay, "COUNT_DAYS_DELAY");
///             Map(x => x.IsDebtPaid, "IS_DEBT_PAID", true, false);
///             Map(x => x.DebtPaidDate, "DEBT_PAID_DATE");
///             Map(x => x.BaseInfo, "BASE_INFO", false, 200);
/// 
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RealityObject, "REAL_OBJ_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using System;
    
    
    /// <summary>Маппинг для "Основание претензионно исковой работы"</summary>
    public class BaseClaimWorkMap : BaseEntityMap<BaseClaimWork>
    {
        
        public BaseClaimWorkMap() : 
                base("Основание претензионно исковой работы", "CLW_CLAIM_WORK")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ClaimWorkTypeBase, "Тип основания").Column("TYPE_BASE").DefaultValue(ClaimWorkTypeBase.Debtor).NotNull();
            this.Property(x => x.BaseInfo, "Информация из основания в зависимости от типа").Column("BASE_INFO").Length(200);
            this.Property(x => x.StartingDate, "Дата начала отсчета").Column("START_DATE");
            this.Property(x => x.CountDaysDelay, "количество дней просрочки").Column("COUNT_DAYS_DELAY");
            this.Property(x => x.IsDebtPaid, "Задолженность погашена").Column("IS_DEBT_PAID").DefaultValue(false).NotNull();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REAL_OBJ_ID");
            this.Property(x => x.DebtPaidDate, "Дата погашения задолженности").Column("DEBT_PAID_DATE");
            this.Reference(x => x.State, "количество дней просрочки").Column("STATE_ID").Fetch();
        }
    }
}
