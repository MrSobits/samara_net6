namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActRemoval
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    /// <summary>Маппинг для "Определение акта проверки предписания ГЖИ"</summary>
    public class ActRemovalDefinitionMap : BaseEntityMap<ActRemovalDefinition>
    {
		public ActRemovalDefinitionMap() : 
                base("Определение акта проверки предписания ГЖИ", "GJI_NSO_ACTREMOVAL_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.ExecutionDate, "Дата исполнения").Column("EXECUTION_DATE");
            this.Property(x => x.DocumentNumber, "Номер документа (целая часть)").Column("DOC_NUMBER");
            this.Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.TypeDefinition, "Тип определения").Column("TYPE_DEFINITION").NotNull();
            this.Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
            this.Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}
