namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Regions.Nso.Entities;

	/// <summary>Маппинг для "Определение акта проверки предписания ГЖИ"</summary>
    public class ActRemovalDefinitionMap : BaseEntityMap<ActRemovalDefinition>
    {
		public ActRemovalDefinitionMap() : 
                base("Определение акта проверки предписания ГЖИ", "GJI_NSO_ACTREMOVAL_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.ExecutionDate, "Дата исполнения").Column("EXECUTION_DATE");
            Property(x => x.DocumentNumber, "Номер документа (целая часть)").Column("DOC_NUMBER");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.TypeDefinition, "Тип определения").Column("TYPE_DEFINITION").NotNull();
            Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
            Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}
