/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Подрядчик"
///     /// </summary>
///     public class BuilderMap : BaseGkhEntityMap<Builder>
///     {
///         public BuilderMap()
///             : base("GKH_BUILDER")
///         {
///             Map(x => x.OrgStateRole, "ORG_STATE_ROLE").Not.Nullable().CustomType<OrgStateRole>();
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.AdvancedTechnologies, "ADVANCED_TECHNOLOGIES").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.ConsentInfo, "CONSENT_INFO").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.WorkWithoutContractor, "WORK_WITHOUT_CONTR").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.Rating, "RATING");
///             Map(x => x.TaxInfoAddress, "TAX_INFO_ADDRESS").Length(1000);
///             Map(x => x.TaxInfoPhone, "TAX_INFO_PHONE").Length(50);
/// 
///             // деятельность
///             Map(x => x.ActivityDateStart, "ACTIVITY_DATE_START");
///             Map(x => x.ActivityDateEnd, "ACTIVITY_DATE_END");
///             Map(x => x.ActivityDescription, "ACTIVITY_DESCRIPTION").Length(500);
///             Map(x => x.ActivityGroundsTermination, "ACTIVITY_TERMINATION").Not.Nullable().CustomType<GroundsTermination>();
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
/// 
///             References(x => x.FileLearningPlan, "FILE_LEARN_PLAN_ID").Fetch.Join();
///             References(x => x.FileManningShedulle, "FILE_MANSHEDUL_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Подрядчики"</summary>
    public class BuilderMap : BaseImportableEntityMap<Builder>
    {
        
        public BuilderMap() : 
                base("Подрядчики", "GKH_BUILDER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.OrgStateRole, "Статус").Column("ORG_STATE_ROLE").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.AdvancedTechnologies, "Применение прогрессивных технологий").Column("ADVANCED_TECHNOLOGIES").NotNull();
            Property(x => x.ConsentInfo, "Согласие на предоставление информации").Column("CONSENT_INFO").NotNull();
            Property(x => x.WorkWithoutContractor, "Выполнение работ без субподрядчика").Column("WORK_WITHOUT_CONTR").NotNull();
            Property(x => x.Rating, "Рейтинг").Column("RATING");
            Property(x => x.TaxInfoAddress, "адрес налогового органа").Column("TAX_INFO_ADDRESS").Length(1000);
            Property(x => x.TaxInfoPhone, "телефон налогового органа").Column("TAX_INFO_PHONE").Length(50);
            Property(x => x.ActivityDateStart, "Дата начала деятельности").Column("ACTIVITY_DATE_START");
            Property(x => x.ActivityDateEnd, "Дата окончания деятельности").Column("ACTIVITY_DATE_END");
            Property(x => x.ActivityDescription, "Описание для деятельности").Column("ACTIVITY_DESCRIPTION").Length(500);
            Property(x => x.ActivityGroundsTermination, "Основание прекращения деятельности").Column("ACTIVITY_TERMINATION").NotNull();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл cогласие на предоставление информации").Column("FILE_ID").Fetch();
            Reference(x => x.FileLearningPlan, "План обучения (переподготовки) кадров").Column("FILE_LEARN_PLAN_ID").Fetch();
            Reference(x => x.FileManningShedulle, "Штатное расписание кадров").Column("FILE_MANSHEDUL_ID").Fetch();
        }
    }
}
