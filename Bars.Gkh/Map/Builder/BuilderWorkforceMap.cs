/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Состав трудовых ресурсов подрядчиков"
///     /// </summary>
///     public class BuilderWorkforceMap : BaseGkhEntityMap<BuilderWorkforce>
///     {
///         public BuilderWorkforceMap()
///             : base("GKH_BUILDER_WORKFORCE")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.DocumentQualification, "DOCUMENT_QUALIFICATION").Length(300);
///             Map(x => x.EmploymentDate, "EMPLOYMENT_DATE");
///             Map(x => x.Fio, "FIO").Length(300);
///             Map(x => x.Position, "POSITION").Length(100);
/// 
///             References(x => x.Specialty, "SPECIALTY_ID").Fetch.Join();
///             References(x => x.Institutions, "INSTITUTIONS_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.Builder, "BUILDER_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Состав трудовых ресурсов"</summary>
    public class BuilderWorkforceMap : BaseImportableEntityMap<BuilderWorkforce>
    {
        
        public BuilderWorkforceMap() : 
                base("Состав трудовых ресурсов", "GKH_BUILDER_WORKFORCE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.DocumentQualification, "Наименование документа подтверждающего квалификацию").Column("DOCUMENT_QUALIFICATION").Length(300);
            Property(x => x.EmploymentDate, "Дата приема на работу").Column("EMPLOYMENT_DATE");
            Property(x => x.Fio, "ФИО").Column("FIO").Length(300);
            Property(x => x.Position, "Должность").Column("POSITION").Length(100);
            Reference(x => x.Specialty, "Специальность").Column("SPECIALTY_ID").Fetch();
            Reference(x => x.Institutions, "Учебное заведение").Column("INSTITUTIONS_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
        }
    }
}
