/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealObjStructElementIdHistoryMap : BaseImportableEntityMap<RealObjStructElementIdHistory>
///     {
///         public RealObjStructElementIdHistoryMap()
///             : base("OVRHL_RO_STR_EL_HISTORY")
///         {
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.RealObjStructElId, "RO_SE_ID", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Идентификаторы конструктивных элементов дома, в том числ и удаленных (нужен для подтягивания истории изменений)"</summary>
    public class RealObjStructElementIdHistoryMap : BaseImportableEntityMap<RealObjStructElementIdHistory>
    {
        
        public RealObjStructElementIdHistoryMap() : 
                base("Идентификаторы конструктивных элементов дома, в том числ и удаленных (нужен для п" +
                        "одтягивания истории изменений)", "OVRHL_RO_STR_EL_HISTORY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Объект недвижимости").Column("RO_ID").NotNull().Fetch();
            Property(x => x.RealObjStructElId, "Идентификатор конструктивного элемента").Column("RO_SE_ID").NotNull();
        }
    }
}
