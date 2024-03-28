/// <mapping-converter-backup>
/// namespace Bars.Gkh.Repair.Map
/// {
///     using Bars.B4.DataAccess;
/// 
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Муниципальное образование программы текущего ремонта"
///     /// </summary>
///     public class RepairProgramMunicipalityMap : BaseEntityMap<RepairProgramMunicipality>
///     {
///         public RepairProgramMunicipalityMap() : base("RP_DICT_PROGRAM_MU")
///         {
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RepairProgram, "PROGRAM_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// 
/// </mapping-converter-backup>

namespace Bars.Gkh.Repair.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Repair.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Repair.Entities.RepairProgramMunicipality"</summary>
    public class RepairProgramMunicipalityMap : BaseEntityMap<RepairProgramMunicipality>
    {
        
        public RepairProgramMunicipalityMap() : 
                base("Bars.Gkh.Repair.Entities.RepairProgramMunicipality", "RP_DICT_PROGRAM_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.RepairProgram, "RepairProgram").Column("PROGRAM_ID").NotNull().Fetch();
        }
    }
}
