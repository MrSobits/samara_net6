/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Contracts.Enums;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Напоминание"
///     /// </summary>
///     public class ReminderMap : BaseEntityMap<Reminder>
///     {
///         public ReminderMap()
///             : base("GJI_REMINDER")
///         {
/// 
///             Map(x => x.CheckDate, "CHECK_DATE");
///             Map(x => x.Actuality, "ACTUALITY").Not.Nullable();
///             Map(x => x.CategoryReminder, "CATEGORY_REMINDER").Not.Nullable().CustomType<CategoryReminder>();
///             Map(x => x.TypeReminder, "TYPE_REMINDER").Not.Nullable().CustomType<TypeReminder>();
///             Map(x => x.Num, "NUM").Length(500);
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID").LazyLoad();
///             References(x => x.InspectionGji, "INSPECTION_ID").LazyLoad();
///             References(x => x.DocumentGji, "DOCUMENT_ID").LazyLoad();
///             References(x => x.AppealCits, "APPEAL_CITS_ID").LazyLoad();
///             References(x => x.Inspector, "INSPECTOR_ID").LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Напоминание по действиям ГЖИ"</summary>
    public class ReminderMap : BaseEntityMap<Reminder>
    {
        
        public ReminderMap() : 
                base("Напоминание по действиям ГЖИ", "GJI_REMINDER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CheckDate, "Контрольный срок").Column("CHECK_DATE");
            Property(x => x.Actuality, "Актуально ли данное напоминание").Column("ACTUALITY").NotNull();
            Property(x => x.CategoryReminder, "Категория").Column("CATEGORY_REMINDER").NotNull();
            Property(x => x.TypeReminder, "Тип напоминания").Column("TYPE_REMINDER").NotNull();
            Property(x => x.Num, "Номер напоминаиния").Column("NUM").Length(500);
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            Reference(x => x.InspectionGji, "Проверка ГЖИ").Column("INSPECTION_ID");
            Reference(x => x.DocumentGji, "Документ ГЖИ").Column("DOCUMENT_ID");
            Reference(x => x.AppealCits, "Обращение граждан").Column("APPEAL_CITS_ID");
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID");
            Reference(x => x.Guarantor, "Инспектор").Column("GUARANTOR_ID");
            Reference(x => x.CheckingInspector, "Инспектор").Column("CHECKINGINSPECTOR_ID");
        }
    }
}
