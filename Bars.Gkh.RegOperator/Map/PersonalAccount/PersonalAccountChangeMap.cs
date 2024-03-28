namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Операция изменения ЛС"</summary>
    public class PersonalAccountChangeMap : BaseImportableEntityMap<PersonalAccountChange>
    {
        
        /// <summary>
        /// .ctor
        /// </summary>
        public PersonalAccountChangeMap() : 
                base("Операция изменения ЛС", "REGOP_PERS_ACC_CHANGE")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "ЛС").Column("ACC_ID").NotNull().Fetch();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(250);
            this.Property(x => x.ChangeType, "Вид изменения").Column("CHANGE_TYPE");
            this.Property(x => x.Date, "Дата операции").Column("DATE");
            this.Property(x => x.ActualFrom, "Дата актуальности").Column("ACTUAL_FROM");
            this.Property(x => x.Operator, "Оператор").Column("OPERATOR").Length(250);
            this.Property(x => x.OldValue, "Старое значение").Column("OLD_VALUE").Length(300);
            this.Property(x => x.NewValue, "Новое значение").Column("NEW_VALUE").Length(300);
            this.Reference(x => x.Document, "Документ-основание").Column("DOC_ID").Fetch();
            this.Property(x => x.Reason, "Причина изменения").Column("REASON").Length(250);
            this.Reference(x => x.ChargePeriod, "Период расчета").Column("PERIOD_ID").NotNull().Fetch();
        }
    }
}
