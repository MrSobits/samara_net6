namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Определение акта без взаимодействия"</summary>
    public class ActIsolatedDefinitionMap : BaseEntityMap<ActIsolatedDefinition>
    {       
        public ActIsolatedDefinitionMap() : 
                base("Определение акта без взаимодействия", "GJI_ACTISOLATED_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.ExecutionDate, "Дата исполнения").Column("EXECUTION_DATE");
            this.Property(x => x.DocumentNumber, "Номер документа (целая часть)").Column("DOC_NUMBER");
            this.Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.TypeDefinition, "Тип определения").Column("TYPE_DEFINITION").NotNull();
            this.Reference(x => x.ActIsolated, "Акт без взаимодействия").Column("ACTISOLATED_ID").NotNull().Fetch();
            this.Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}