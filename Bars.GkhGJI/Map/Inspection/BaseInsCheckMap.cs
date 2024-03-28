/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Инспекционная проверка"
///     /// </summary>
///     public class BaseInsCheckMap : SubclassMap<BaseInsCheck>
///     {
///         public BaseInsCheckMap()
///         {
///             Table("GJI_INSPECTION_INSCHECK");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.InsCheckDate, "INSCHECK_DATE");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.Area, "AREA");
///             Map(x => x.Reason, "REASON").Length(500);
///             Map(x => x.TypeFact, "TYPE_FACT").Not.Nullable().CustomType<TypeFactInspection>();
///             Map(x => x.TypeDocument, "TYPE_DOCUMENT").Not.Nullable().CustomType<TypeDocumentInsCheck>();
///             Map(x => x.DocumentDate, "DATE_DOCUMENT");
///             Map(x => x.DocumentNumber, "NUM_DOCUMENT").Length(300);
/// 
///             References(x => x.Plan, "PLAN_ID").LazyLoad();
///             References(x => x.DocFile, "DOC_FILE_ID").LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание инспекционная проверка ГЖИ"</summary>
    public class BaseInsCheckMap : JoinedSubClassMap<BaseInsCheck>
    {
        
        public BaseInsCheckMap() : 
                base("Основание инспекционная проверка ГЖИ", "GJI_INSPECTION_INSCHECK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.InsCheckDate, "Дата").Column("INSCHECK_DATE");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.CountDays, "Срок проверки (Количество дней)").Column("COUNT_DAYS");
            Property(x => x.Area, "Площадь МКД (кв. м)").Column("AREA");
            Property(x => x.Reason, "Причина").Column("REASON").Length(500);
            Property(x => x.TypeFact, "Факт проверки").Column("TYPE_FACT").NotNull();
            Property(x => x.TypeDocument, "Тип документа").Column("TYPE_DOCUMENT").NotNull();
            Property(x => x.DocumentDate, "Дата документа").Column("DATE_DOCUMENT");
            Property(x => x.DocumentNumber, "Номер документа").Column("NUM_DOCUMENT").Length(300);
            Reference(x => x.Plan, "План инспекционных проверок").Column("PLAN_ID");
            Reference(x => x.DocFile, "Файл").Column("DOC_FILE_ID");
        }
    }
}
