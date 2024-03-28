/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Инспектор документа ГЖИ"
///     /// </summary>
///     public class DocumentGjiInspectorMap : BaseGkhEntityMap<DocumentGjiInspector>
///     {
///         public DocumentGjiInspectorMap() : base("GJI_DOCUMENT_INSPECTOR")
///         {
///             References(x => x.DocumentGji, "DOCUMENT_ID").Not.Nullable().LazyLoad();
///             References(x => x.Inspector, "INSPECTOR_ID").Not.Nullable().Fetch.Join();
///             Map(x => x.Order, "INSPECTOR_ORDER");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Инспекторы документа ГЖИ"</summary>
    public class DocumentGjiInspectorMap : BaseEntityMap<DocumentGjiInspector>
    {

        public DocumentGjiInspectorMap() :
            base("Инспекторы документа ГЖИ", "GJI_DOCUMENT_INSPECTOR")
        {
        }

        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Order, "Порядковый номер инспектора для текущего документа").Column("INSPECTOR_ORDER");
            Reference(x => x.DocumentGji, "Документ ГЖИ").Column("DOCUMENT_ID").NotNull();
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.ErpGuid, "Идентификатор в ЕРП").Column("ERP_GUID").Length(36);
            Property(x => x.ErknmGuid, "Гуид ЕРКНМ").Column("ERKNM_GUID").Length(36);
        }
    }
}