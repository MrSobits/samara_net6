namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Причина изменения вида работы объекта КР"</summary>
    public class SpecialTypeWorkCrRemovalMap : BaseImportableEntityMap<SpecialTypeWorkCrRemoval>
    {
        public SpecialTypeWorkCrRemovalMap() : 
                base("Причина изменения вида работы объекта КР", "CR_SPECIAL_OBJ_TYPE_WORK_REMOVAL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.NumDoc, "Номер документа").Column("NUM_DOC").Length(100);
            this.Property(x => x.Description, "Документ Основание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.StructElement, "Конструктивный элемент").Column("STRUCT_EL").Length(500);
            this.Property(x => x.DateDoc, "Дата документа От").Column("DATE_DOC");
            this.Property(x => x.YearRepair, "Год выполнения по долгосрочной программе").Column("YEAR_REPAIR");
            this.Property(x => x.NewYearRepair, "Новый год выполнения").Column("NEW_YEAR_REPAIR");
            this.Property(x => x.TypeReason, "Причина удаления").Column("TYPE_REASON").NotNull();

            this.Reference(x => x.TypeWorkCr, "Вид работы объекта КР").Column("TYPE_WORK_ID").NotNull().Fetch();
            this.Reference(x => x.FileDoc, "Документ (основание)").Column("FILE_DOC_ID").Fetch();
        }
    }
}
