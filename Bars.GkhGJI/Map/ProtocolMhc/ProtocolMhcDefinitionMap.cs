/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Определения протокола ГЖИ"
///     /// </summary>
///     public class ProtocolMhcDefinitionMap : BaseGkhEntityMap<ProtocolMhcDefinition>
///     {
///         public ProtocolMhcDefinitionMap()
///             : base("GJI_PROTOCOLMHC_DEFINITION")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.ExecutionDate, "EXECUTION_DATE");
///             Map(x => x.DocumentNumber, "DOC_NUMBER");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.TypeDefinition, "TYPE_DEFINITION").Not.Nullable().CustomType<TypeDefinitionProtocolMhc>();
/// 
///             References(x => x.ProtocolMhc, "PROTOCOLMHC_ID").Not.Nullable().Fetch.Join();
///             References(x => x.IssuedDefinition, "ISSUED_DEFINITION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Определение протокола МЖК"</summary>
    public class ProtocolMhcDefinitionMap : BaseEntityMap<ProtocolMhcDefinition>
    {
        
        public ProtocolMhcDefinitionMap() : 
                base("Определение протокола МЖК", "GJI_PROTOCOLMHC_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.ExecutionDate, "Дата исполнения").Column("EXECUTION_DATE");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOC_NUMBER");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.TypeDefinition, "Тип определения").Column("TYPE_DEFINITION").NotNull();
            Reference(x => x.ProtocolMhc, "Протокол").Column("PROTOCOLMHC_ID").NotNull().Fetch();
            Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}
