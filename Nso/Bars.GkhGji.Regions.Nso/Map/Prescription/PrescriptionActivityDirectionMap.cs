/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PrescriptionActivityDirectionMap : BaseEntityMap<PrescriptionActivityDirection>
///     {
///         public PrescriptionActivityDirectionMap()
///             : base("GJI_PRESCR_ACTIV_DIRECT")
///         {
///             References(x => x.ActivityDirection, "ACTIVEDIRECT_ID", ReferenceMapConfig.NotNull);
///             References(x => x.Prescription, "PRESCRIPTION_ID", ReferenceMapConfig.NotNull);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.PrescriptionActivityDirection"</summary>
    public class PrescriptionActivityDirectionMap : BaseEntityMap<PrescriptionActivityDirection>
    {
        
        public PrescriptionActivityDirectionMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.PrescriptionActivityDirection", "GJI_PRESCR_ACTIV_DIRECT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Prescription, "Prescription").Column("PRESCRIPTION_ID").NotNull();
            Reference(x => x.ActivityDirection, "ActivityDirection").Column("ACTIVEDIRECT_ID").NotNull();
        }
    }
}
