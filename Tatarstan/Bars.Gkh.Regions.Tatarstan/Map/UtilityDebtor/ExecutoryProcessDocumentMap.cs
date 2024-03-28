namespace Bars.Gkh.Regions.Tatarstan.Map.UtilityDebtor
{
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Маппинг для "Документы для исполнительного производства"
    /// </summary>
    public class ExecutoryProcessDocumentMap : BaseEntityMap<ExecutoryProcessDocument>
    {
        public ExecutoryProcessDocumentMap() : 
                base("Документы для исполнительного производства", "CLW_EXEC_PROC_DOCUMENT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ExecutoryProcess, "Исполнительное производство").Column("EXEC_PROC_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").NotNull().Fetch();
            this.Property(x => x.Number, "Номер документа").Column("NUMBER").Length(50).NotNull();
            this.Property(x => x.Date, "Дата документа").Column("DATE").Length(50).NotNull();
            this.Property(x => x.ExecutoryProcessDocumentType, "Тип документа").Column("DOC_TYPE").DefaultValue(ExecutoryProcessDocumentType.InitiateExecutoryProcess).NotNull();
            this.Property(x => x.Notation, "Примечание").Column("NOTATION");
        }
    }
}
