/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Разрезы финансирования программы по КР"
///     /// </summary>
///     public class ProgramCrFinSourceMap : BaseGkhEntityMap<ProgramCrFinSource>
///     {
///         public ProgramCrFinSourceMap() : base("CR_DICT_PROGCR_FIN_SOURCE")
///         {
///             References(x => x.ProgramCr, "PROGRAM_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FinanceSource, "FIN_SOURCE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Разрезы финансирования программы по КР"</summary>
    public class ProgramCrFinSourceMap : BaseImportableEntityMap<ProgramCrFinSource>
    {
        
        public ProgramCrFinSourceMap() : 
                base("Разрезы финансирования программы по КР", "CR_DICT_PROGCR_FIN_SOURCE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ProgramCr, "Программа").Column("PROGRAM_ID").NotNull().Fetch();
            Reference(x => x.FinanceSource, "Разрез финансирования").Column("FIN_SOURCE_ID").NotNull().Fetch();
        }
    }
}
