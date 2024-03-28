/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
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

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Prescription
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.PrescriptionActivityDirection"</summary>
    public class PrescriptionActivityDirectionMap : BaseEntityMap<PrescriptionActivityDirection>
    {
        
        public PrescriptionActivityDirectionMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.PrescriptionActivityDirection", "GJI_PRESCR_ACTIV_DIRECT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Prescription, "Prescription").Column("PRESCRIPTION_ID").NotNull();
            this.Reference(x => x.ActivityDirection, "ActivityDirection").Column("ACTIVEDIRECT_ID").NotNull();
        }
    }
}
