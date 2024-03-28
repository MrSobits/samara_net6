namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>Маппинг для "Вид работы КР"</summary>
    public class TypeWorkCrAddWorkMap : BaseImportableEntityMap<TypeWorkCrAddWork>
    {

        public TypeWorkCrAddWorkMap()
            : base("Вид работы КР", "CR_OBJ_TW_STAGE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.TypeWorkCr, "Работа капитального ремонта").Column("TYPEWORK_ID").NotNull().Fetch();
            this.Reference(x => x.AdditWork, "Этап работы").Column("ADDIT_WORK_ID").NotNull().Fetch();
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
            this.Property(x => x.Queue, "Очередность").Column("QUEUE").Length(2);
            this.Property(x => x.Required, "Контролируется стройконтролем").Column("UNDER_CONTROL");        
        }
    }
}