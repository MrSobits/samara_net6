/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.Dict
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities.Dict;
/// 
///     public class ProgramCrTypeMap : BaseImportableEntityMap<ProgramCrType>
///     {
///         public ProgramCrTypeMap()
///             : base("REGOP_PROGRAM_CR_TYPE")
///         {
///             Map(x => x.Code, "CODE");
///             Map(x => x.Name, "NAME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Dict;
    
    
    /// <summary>Маппинг для "Типы программы КР"</summary>
    public class ProgramCrTypeMap : BaseImportableEntityMap<ProgramCrType>
    {
        
        public ProgramCrTypeMap() : 
                base("Типы программы КР", "REGOP_PROGRAM_CR_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
        }
    }
}
