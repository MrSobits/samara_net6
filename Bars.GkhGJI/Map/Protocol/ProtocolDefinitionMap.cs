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
///     public class ProtocolDefinitionMap : BaseGkhEntityMap<ProtocolDefinition>
///     {
///         public ProtocolDefinitionMap()
///             : base("GJI_PROTOCOL_DEFINITION")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.ExecutionDate, "EXECUTION_DATE");
///             Map(x => x.DocumentNumber, "DOC_NUMBER");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.TypeDefinition, "TYPE_DEFINITION").Not.Nullable().CustomType<TypeDefinitionProtocol>();
///             Map(x => x.TimeDefinition, "DEF_TIME");
///             Map(x => x.DateOfProceedings, "DATE_PROC");
/// 
///             References(x => x.Protocol, "PROTOCOL_ID").Not.Nullable().Fetch.Join();
///             References(x => x.IssuedDefinition, "ISSUED_DEFINITION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Определение протокола ГЖИ"</summary>
    public class ProtocolDefinitionMap : BaseEntityMap<ProtocolDefinition>
    {
        
        public ProtocolDefinitionMap() : 
                base("Определение протокола ГЖИ", "GJI_PROTOCOL_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.ExecutionDate, "Дата исполнения").Column("EXECUTION_DATE");
            Property(x => x.DocumentNumber, "Номер документа (целая часть)").Column("DOC_NUMBER");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.TypeDefinition, "Тип определения").Column("TYPE_DEFINITION").NotNull();
            Property(x => x.TimeDefinition, "Время начала (Данное поле используется в какомто регионе)").Column("DEF_TIME");
            Property(x => x.DateOfProceedings, "Дата рассмотрения дела (Даное поле используется в каком то регионе)").Column("DATE_PROC");
            Reference(x => x.Protocol, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Файл определения").Column("FILE_ID").Fetch();
            Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
            Reference(x => x.SignedBy, "SignedBy").Column("SIGNER_ID").NotNull();
        }
    }
}
