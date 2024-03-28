namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Определение протокола прокуратуры"</summary>
    public class ProtocolRSODefinitionMap : BaseEntityMap<ProtocolRSODefinition>
    {
        
        public ProtocolRSODefinitionMap() : 
                base("Определение протокола РСО", "GJI_PROTOCOLRSO_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.ExecutionDate, "Дата исполнения").Column("EXECUTION_DATE");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.TypeDefinition, "Тип определения").Column("TYPE_DEFINITION").NotNull();
            Reference(x => x.ProtocolRSO, "Протокол").Column("PROTOCOLRSO_ID").NotNull().Fetch();
            Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}
