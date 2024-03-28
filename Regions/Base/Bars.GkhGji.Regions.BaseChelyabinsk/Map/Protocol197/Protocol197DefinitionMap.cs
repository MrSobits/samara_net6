
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol197
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;


    /// <summary>Маппинг для "Определение протокола ГЖИ"</summary>
    public class Protocol197DefinitionMap : BaseEntityMap<Protocol197Definition>
    {
        
        public Protocol197DefinitionMap() : 
                base("Определение протокола ГЖИ", "GJI_PROTOCOL197_DEFINITION")
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
            Reference(x => x.Protocol197, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Файл определения").Column("FILE_ID").Fetch();
            Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column("ISSUED_DEFINITION_ID").Fetch();
            Reference(x => x.SignedBy, "SignedBy").Column("SIGNER_ID").NotNull();
        }
    }
}
