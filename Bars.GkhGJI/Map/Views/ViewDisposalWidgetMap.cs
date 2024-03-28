/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     public class ViewDisposalWidgetMap : PersistentObjectMap<ViewDisposalWidget>
///     {
///         public ViewDisposalWidgetMap() : base("VIEW_GJI_DISPOSAL_WIDGET")
///         {
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.Number, "DOCUMENT_NUMBER");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.OperatorId, "OPERATOR_ID");
///             Map(x => x.TypeBase, "TYPE_BASE").CustomType<TypeBase>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewDisposalWidget"</summary>
    public class ViewDisposalWidgetMap : PersistentObjectMap<ViewDisposalWidget>
    {
        
        public ViewDisposalWidgetMap() : 
                base("Bars.GkhGji.Entities.ViewDisposalWidget", "VIEW_GJI_DISPOSAL_WIDGET")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.Number, "Номер").Column("DOCUMENT_NUMBER");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Property(x => x.OperatorId, "Оператор").Column("OPERATOR_ID");
            Property(x => x.TypeBase, "Тип").Column("TYPE_BASE");
        }
    }
}
