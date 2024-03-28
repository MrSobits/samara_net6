/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг view "Проверка без основания"
///     /// </summary>
///     public class ViewBaseDefaultMap : PersistentObjectMap<ViewBaseDefault>
///     {
///         public ViewBaseDefaultMap() : base("VIEW_GJI_INS_BASEDEF")
///         {
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON");
///             Map(x => x.InspectionNumber, "INSPECTION_NUM");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewBaseDefault"</summary>
    public class ViewBaseDefaultMap : PersistentObjectMap<ViewBaseDefault>
    {
        
        public ViewBaseDefaultMap() : 
                base("Bars.GkhGji.Entities.ViewBaseDefault", "VIEW_GJI_INS_BASEDEF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.MunicipalityId, "Мунниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.ContragentName, "Юридическое лицо").Column("CONTRAGENT_NAME");
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON");
            Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUM");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
