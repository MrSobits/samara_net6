/// <mapping-converter-backup>
/// // -----------------------------------------------------------------------
/// // <copyright file="ZonalInspectionInspectorMap.cs" company="">
/// // TODO: Update copyright text.
/// // </copyright>
/// // -----------------------------------------------------------------------
/// 
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Субтаблица зональной жилищной инспекции и инспекторов"
///     /// </summary>
///     public class ZonalInspectionInspectorMap : BaseGkhEntityMap<ZonalInspectionInspector>
///     {
///         public ZonalInspectionInspectorMap()
///             : base("GKH_DICT_ZONAINSP_INSPECT")
///         {
///             References(x => x.ZonalInspection, "ZONAL_INSPECTION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Inspector, "INSPECTOR_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Субтаблица зональной жилищной инспекции и инспекторов"</summary>
    public class ZonalInspectionInspectorMap : BaseImportableEntityMap<ZonalInspectionInspector>
    {
        
        public ZonalInspectionInspectorMap() : 
                base("Субтаблица зональной жилищной инспекции и инспекторов", "GKH_DICT_ZONAINSP_INSPECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ZonalInspection, "Зональная жилищная инспекция").Column("ZONAL_INSPECTION_ID").NotNull().Fetch();
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
        }
    }
}
