/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     public class ViewPrescriptionWidgetMap : PersistentObjectMap<ViewPrescriptionWidget>
///     {
///         public ViewPrescriptionWidgetMap()
///             : base("VIEW_GJI_PRESCRIPTION_WIDGET")
///         {
///             Map(x => x.LastDateViolation, "DATE_REMOVAL");
///             Map(x => x.Number, "DOCUMENT_NUMBER");
///             Map(x => x.DatePrescription, "DOCUMENT_DATE");
///             Map(x => x.OperatorId, "OPERATOR_ID");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewPrescriptionWidget"</summary>
    public class ViewPrescriptionWidgetMap : PersistentObjectMap<ViewPrescriptionWidget>
    {
        
        public ViewPrescriptionWidgetMap() : 
                base("Bars.GkhGji.Entities.ViewPrescriptionWidget", "VIEW_GJI_PRESCRIPTION_WIDGET")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.LastDateViolation, "Последний срок нарушения").Column("DATE_REMOVAL");
            Property(x => x.Number, "Номер").Column("DOCUMENT_NUMBER");
            Property(x => x.DatePrescription, "Дата предписания").Column("DOCUMENT_DATE");
            Property(x => x.OperatorId, "Оператор").Column("OPERATOR_ID");
            Property(x => x.ContragentName, "Тип").Column("CONTRAGENT_NAME");
        }
    }
}
