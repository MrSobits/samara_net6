namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    /// <summary>Маппинг для "Нарушение ГЖИ"</summary>
    public class ViolationGjiMap : BaseEntityMap<ViolationGji>
    {
        public ViolationGjiMap() : base("Нарушение ГЖИ", "GJI_DICT_VIOLATION") { }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(2000);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.GkRf, "ЖК РФ").Column("GKRF").Length(2000);
            this.Property(x => x.CodePin, "Код ПИН").Column("CODEPIN").Length(2000);
            this.Property(x => x.NormativeDocNames, "Строка в которую будет сохранятся все Нормативные документы через запятую").Column("NPD_NAME").Length(2000);
            this.Property(x => x.PpRf25, "ПП РФ №25").Column("PPRF25").Length(2000);
            this.Property(x => x.PpRf307, "ПП РФ №307").Column("PPRF307").Length(2000);
            this.Property(x => x.PpRf491, "ПП РФ №491").Column("PPRF491").Length(2000);
            this.Property(x => x.PpRf170, "ПП РФ №170").Column("PPRF170").Length(2000);
            this.Property(x => x.OtherNormativeDocs, "Прочие нормативные документы").Column("OTHER_DOCS").Length(2000);
            this.Property(x => x.IsActual, "Актуальность").Column("IS_ACTUAL").NotNull();
        }
    }
}
