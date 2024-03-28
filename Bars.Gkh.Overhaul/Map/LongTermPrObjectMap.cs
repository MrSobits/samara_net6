/// <mapping-converter-backup>
/// using Bars.Gkh.Overhaul.Entities;
/// 
/// namespace Bars.Gkh.Overhaul.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess;
/// 
/// 
///     /// <summary>
///      /// Маппинг для сущности "объект долгосрочной программы"
///      /// </summary>
///      public class LongTermPrObjectMap : BaseImportableEntityMap<LongTermPrObject>
///      {
///          public LongTermPrObjectMap()
///              : base("OVRHL_LONGTERM_PR_OBJECT")
///          {
///              References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///          }
///      }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Объект долгосрочной программы"</summary>
    public class LongTermPrObjectMap : BaseImportableEntityMap<LongTermPrObject>
    {
        
        public LongTermPrObjectMap() : 
                base("Объект долгосрочной программы", "OVRHL_LONGTERM_PR_OBJECT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJ_ID").NotNull().Fetch();
        }
    }
}
