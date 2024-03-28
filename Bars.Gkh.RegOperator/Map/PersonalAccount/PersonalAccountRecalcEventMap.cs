/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.PersonalAccount;
/// 
///     public class PersonalAccountRecalcEventMap : BaseImportableEntityMap<PersonalAccountRecalcEvent>
///     {
///         public PersonalAccountRecalcEventMap() : base("REGOP_PERS_ACC_RECALC_EVT")
///         {
///             Map(x => x.PersonalAccountId, "PERS_ACC_ID");
///             Map(x => x.RecalcProvider, "RECALC_PROV");
///             Map(x => x.RecalcType, "RECALC_TYPE");
///             Map(x => x.EventDate, "EVT_DATE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.PersonalAccount.PersonalAccountRecalcEvent"</summary>
    public class PersonalAccountRecalcEventMap : BaseEntityMap<PersonalAccountRecalcEvent>
    {
        
        public PersonalAccountRecalcEventMap() : 
                base("Bars.Gkh.RegOperator.Entities.PersonalAccount.PersonalAccountRecalcEvent", "REGOP_PERS_ACC_RECALC_EVT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.RecalcType, "Тип перерасчета. Например, Charge или Penalty. По этому типу будем отличать для к" +
                   "акого перерасчета действует").Column("RECALC_TYPE").Length(250);
            this.Property(x => x.RecalcProvider, "Провайдер даты перерасчета. Например, оплата").Column("RECALC_PROV").Length(250);
            this.Property(x => x.EventDate, "Дата, когда произошло событие, которое влияет на перерасчет").Column("EVT_DATE");
            this.Property(x => x.RecalcEventType, "Тип события для перерасчета").Column("RECALC_EVENT_TYPE").DefaultValue(0);
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID");
            this.Reference(x => x.PersonalAccount, "ЛС").Column("PERS_ACC_ID");
        }
    }
}
