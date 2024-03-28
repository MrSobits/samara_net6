/// <mapping-converter-backup>
/// // -----------------------------------------------------------------------
/// // <copyright file="ZonalInspectionMunicipalityMap.cs" company="">
/// // TODO: Update copyright text.
/// // </copyright>
/// // -----------------------------------------------------------------------
/// 
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Субтаблица зональной жилищной инспекции и Муниципального образования"
///     /// </summary>
///     public class ZonalInspectionMunicipalityMap : BaseGkhEntityMap<ZonalInspectionMunicipality>
///     {
///         public ZonalInspectionMunicipalityMap()
///             : base("GKH_DICT_ZONAINSP_MUNIC")
///         {
///             References(x => x.ZonalInspection, "ZONAL_INSPECTION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Субтаблица зональной жилищной инспекции и Муниципального образования"</summary>
    public class ZonalInspectionMunicipalityMap : BaseImportableEntityMap<ZonalInspectionMunicipality>
    {
        
        public ZonalInspectionMunicipalityMap() : 
                base("Субтаблица зональной жилищной инспекции и Муниципального образования", "GKH_DICT_ZONAINSP_MUNIC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ZonalInspection, "Зональная жилищная инспекция").Column("ZONAL_INSPECTION_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
