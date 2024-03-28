/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     public class PrescriptionRealityObjectMap : BaseEntityMap<PrescriptionRealityObject>
///     {
///         public PrescriptionRealityObjectMap()
///             : base("GJI_PRESCRIPTION_REALOBJ")
///         {
///             References(x => x.Prescription, "PRESCRIPTION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.PrescriptionRealityObject"</summary>
    public class PrescriptionRealityObjectMap : BaseEntityMap<PrescriptionRealityObject>
    {
        
        public PrescriptionRealityObjectMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.PrescriptionRealityObject", "GJI_PRESCRIPTION_REALOBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Prescription, "Prescription").Column("PRESCRIPTION_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
