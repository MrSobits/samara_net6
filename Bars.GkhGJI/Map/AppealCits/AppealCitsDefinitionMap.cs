namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Определение постановления ГЖИ"</summary>
    public class AppealCitsDefinitionMap : BaseEntityMap<AppealCitsDefinition>
    {
        
        public AppealCitsDefinitionMap() : 
                base("Определение постановления ГЖИ", "GJI_APPCIT_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.ExecutionDate, "Дата исполнения").Column("EXECUTION_DATE");
            Property(x => x.DocumentNumber, "Номер документа (целая часть)").Column("DOC_NUMBER");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.TypeDefinition, "Тип определения").Column("TYPE_DEFINITION").NotNull();
            Reference(x => x.AppealCits, "Постановление").Column("APPEAL_ID").NotNull();
            Reference(x => x.FileInfo, "Файл определения").Column("FILE_ID").Fetch();
            Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}
