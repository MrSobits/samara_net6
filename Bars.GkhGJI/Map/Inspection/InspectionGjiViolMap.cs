/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Нарушение проверки"
///     /// </summary>
///     public class InspectionGjiViolMap : BaseGkhEntityMap<InspectionGjiViol>
///     {
///         public InspectionGjiViolMap() : base("GJI_INSPECTION_VIOLATION")
///         {
///             Map(x => x.DatePlanRemoval, "DATE_PLAN_REMOVAL");
///             Map(x => x.DateFactRemoval, "DATE_FACT_REMOVAL");
///             Map(x => x.SumAmountWorkRemoval, "SUM_AMOUNT_REMOVAL");
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.Action, "ACTION").Length(2000);
///             Map(x => x.DateCancel, "DATE_CANCEL");
/// 
///             // Может быть нарушения без домов, поэтому может быть Null
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").LazyLoad();
///             References(x => x.Inspection, "INSPECTION_ID").Not.Nullable().LazyLoad();
///             References(x => x.Violation, "VIOLATION_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Нарушение проверки Эта таблица хранит в себе нарушение проверки, без привязки к конкретному документу"</summary>
    public class InspectionGjiViolMap : BaseEntityMap<InspectionGjiViol>
    {
        
        public InspectionGjiViolMap() : 
                base("Нарушение проверки Эта таблица хранит в себе нарушение проверки, без привязки к к" +
                        "онкретному документу", "GJI_INSPECTION_VIOLATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DatePlanRemoval, "Плановая дата устранения").Column("DATE_PLAN_REMOVAL");
            Property(x => x.DateFactRemoval, "фактическая дата устранения").Column("DATE_FACT_REMOVAL");
            Property(x => x.SumAmountWorkRemoval, "сумма работ по устранению нарушений").Column("SUM_AMOUNT_REMOVAL");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Property(x => x.Action, "Мероприятие по устранению данног онарушения потребовалось для того чтобы в предпи" +
                    "сании назначать мероприятия а в приказе или акте видеть данное мероприятие").Column("ACTION").Length(2000);
            Property(x => x.DateCancel, "Дата отмены").Column("DATE_CANCEL");
            Property(x => x.ERPGuid, "ГУИД ГИС ЕРП").Column("ERP_GUID");
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID");
            Reference(x => x.Inspection, "Проверка ГЖИ").Column("INSPECTION_ID").NotNull();
            Reference(x => x.Violation, "Нарушение").Column("VIOLATION_ID").NotNull().Fetch();
        }
    }
}
