namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Квалификационный отбор"</summary>
    public class SpecialQualificationMap : BaseImportableEntityMap<SpecialQualification>
    {
        public SpecialQualificationMap() : 
                base("Квалификационный отбор", "CR_SPECIAL_QUALIFICATION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Sum, "Сумма").Column("SUM");

            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
        }
    }
}
