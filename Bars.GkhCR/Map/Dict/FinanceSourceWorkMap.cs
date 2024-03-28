/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работы источника финансирования по КР"
///     /// </summary>
///     public class FinanceSourceWorkMap : BaseGkhEntityMap<FinanceSourceWork>
///     {
///         public FinanceSourceWorkMap() : base("CR_DICT_FIN_SOURCE_WORK")
///         {
///             References(x => x.Work, "WORK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FinanceSource, "FIN_SOURCE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Работы источника финансирования по КР"</summary>
    public class FinanceSourceWorkMap : BaseImportableEntityMap<FinanceSourceWork>
    {
        
        public FinanceSourceWorkMap() : 
                base("Работы источника финансирования по КР", "CR_DICT_FIN_SOURCE_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Work, "Работа").Column("WORK_ID").NotNull().Fetch();
            Reference(x => x.FinanceSource, "Разрез финансирования").Column("FIN_SOURCE_ID").NotNull().Fetch();
        }
    }
}
