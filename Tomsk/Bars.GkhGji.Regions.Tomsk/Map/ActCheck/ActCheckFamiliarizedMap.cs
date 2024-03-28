/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Лица, ознакомленные с копией распоряжения"
///     /// </summary>
///     public class ActCheckWitnessMap : BaseEntityMap<ActCheckFamiliarized>
///     {
///         public ActCheckWitnessMap() : base("GJI_ACTCHECK_FAMILIAR")
///         {
///             Map(x => x.Fio, "FIO").Length(300).Not.Nullable();
///             References(x => x.ActCheck, "ACTCHECK_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.ActCheckFamiliarized"</summary>
    public class ActCheckFamiliarizedMap : BaseEntityMap<ActCheckFamiliarized>
    {
        
        public ActCheckFamiliarizedMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.ActCheckFamiliarized", "GJI_ACTCHECK_FAMILIAR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Fio, "Fio").Column("FIO").Length(300).NotNull();
            Reference(x => x.ActCheck, "ActCheck").Column("ACTCHECK_ID").NotNull().Fetch();
        }
    }
}
