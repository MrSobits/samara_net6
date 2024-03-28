/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг связи Решения и Нарушения.
///     /// </summary>
///     public class PrescriptionCancelViolReferenceMap : BaseEntityMap<PrescriptionCancelViolReference>
///     {
///         public PrescriptionCancelViolReferenceMap()
///             : base("GJI_PRES_CANCELVIOL_REF")
///         {
///             Map(x => x.NewDatePlanRemoval, "NEW_DATE_REMOV");
///             References(x => x.PrescriptionCancel, "PRES_CANCEL_ID").Not.Nullable().LazyLoad();
///             References(x => x.InspectionViol, "INSPECT_VIOL_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Сущность связи Решения и Нарушения."</summary>
    public class PrescriptionCancelViolReferenceMap : BaseEntityMap<PrescriptionCancelViolReference>
    {
        
        public PrescriptionCancelViolReferenceMap() : 
                base("Сущность связи Решения и Нарушения.", "GJI_PRES_CANCELVIOL_REF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.NewDatePlanRemoval, "Новый срок устранения").Column("NEW_DATE_REMOV");
            Reference(x => x.PrescriptionCancel, "Решение об отмене в предписании ГЖИ").Column("PRES_CANCEL_ID").NotNull();
            Reference(x => x.InspectionViol, "Этап указания к устранению нарушения в предписании").Column("INSPECT_VIOL_ID").NotNull();
        }
    }
}
