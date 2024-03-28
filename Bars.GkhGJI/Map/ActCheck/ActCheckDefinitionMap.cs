/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Определения акта проверки ГЖИ"
///     /// </summary>
///     public class ActCheckDefinitionMap : BaseGkhEntityMap<ActCheckDefinition>
///     {
///         public ActCheckDefinitionMap()
///             : base("GJI_ACTCHECK_DEFINITION")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.ExecutionDate, "EXECUTION_DATE");
///             Map(x => x.DocumentNumber, "DOC_NUMBER");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.TypeDefinition, "TYPE_DEFINITION").Not.Nullable().CustomType<TypeDefinitionAct>();
/// 
///             References(x => x.ActCheck, "ACTCHECK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.IssuedDefinition, "ISSUED_DEFINITION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Определение акта проверки ГЖИ"</summary>
    public class ActCheckDefinitionMap : BaseEntityMap<ActCheckDefinition>
    {
        
        public ActCheckDefinitionMap() : 
                base("Определение акта проверки ГЖИ", "GJI_ACTCHECK_DEFINITION")
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
            Reference(x => x.ActCheck, "Акт проверки").Column("ACTCHECK_ID").NotNull().Fetch();
            Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.DocumentSend, "Дата документа").Column("DOCUMENT_SEND");
            Property(x => x.DocumentDelivered, "Дата документа").Column("DOCUMENT_DELIV");
        }
    }
}
