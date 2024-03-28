
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Легковесное сущность для хранения изменения сущности"</summary>
    public class ExternalExchangeTestingFilesMap : BaseEntityMap<ExternalExchangeTestingFiles>
    {
        
        public ExternalExchangeTestingFilesMap() : 
                base("Легковесное сущность для хранения файлов запросов тестирования СМЭВ", "EXTERNAL_EXCHANGE_TESTING_FILES")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.EntityId, "Идентификатор сущности").Column("ENTITY_ID").NotNull();
            Property(x => x.ClassName, "Класс сущности").Column("CCLASS_NAME").Length(1000);
            Property(x => x.ClassDescription, "Описание сущности").Column("CCLASS_DESC").Length(2000);         
            Property(x => x.DateApplied, "Дата поступления сведений об изменении значения").Column("CDATE_APPLIED");           
            Reference(x => x.Document, "Документ - основание").Column("FILE_ID");           
            Property(x => x.User, "Пользователь").Column("CUSER_NAME").Length(250).NotNull();            
        }
    }
}
