/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.ShortTermProgram
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Enum;
///     using Entities;
/// 
///     public class ShortProgramRealityObjectMap : BaseEntityMap<ShortProgramRealityObject>
///     {
///         public ShortProgramRealityObjectMap()
///             : base("OVRHL_SHORT_PROG_OBJ")
///         {
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ProgramVersion, "VERSION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
/// 
///             Map(x => x.TypeActuality, "TYPE_ACTUALITY").Not.Nullable().CustomType<TypeActuality>();
///             Map(x => x.Year, "YEAR").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.ShortProgramRealityObject"</summary>
    public class ShortProgramRealityObjectMap : BaseEntityMap<ShortProgramRealityObject>
    {
        
        public ShortProgramRealityObjectMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.ShortProgramRealityObject", "OVRHL_SHORT_PROG_OBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeActuality, "TypeActuality").Column("TYPE_ACTUALITY").NotNull();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
