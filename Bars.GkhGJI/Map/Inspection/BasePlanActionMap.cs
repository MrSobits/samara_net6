/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Проверка по плану мероприятий"
///     /// </summary>
///     public class BasePlanActionMap : SubclassMap<BasePlanAction>
///     {
///         public BasePlanActionMap()
///         {
///             Table("GJI_INSPECTION_PLANACTION");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.CountDays, "COUNT_DAYS");
///             Map(x => x.PersonAddress, "PERSON_ADDRESS").Length(500);
///             Map(x => x.Requirement, "REQUIREMENT").Length(2000);
/// 
///             References(x => x.Plan, "PLAN_ID").LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание проверки план мероприятий"</summary>
    public class BasePlanActionMap : JoinedSubClassMap<BasePlanAction>
    {
        
        public BasePlanActionMap() : 
                base("Основание проверки план мероприятий", "GJI_INSPECTION_PLANACTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Property(x => x.CountDays, "Рабочих дней").Column("COUNT_DAYS");
            Property(x => x.PersonAddress, "Место нахождения (или Адрес Физ лица) Данное поле заполняется втом случае если вы" +
                    "брали тип субъект = ФизЛицо").Column("PERSON_ADDRESS").Length(500);
            Property(x => x.Requirement, "Требование").Column("REQUIREMENT").Length(2000);
            Reference(x => x.Plan, "План мероприятий").Column("PLAN_ID");
        }
    }
}
