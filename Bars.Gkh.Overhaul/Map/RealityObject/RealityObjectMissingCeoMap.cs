/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealityObjectMissingCeoMap : BaseImportableEntityMap<RealityObjectMissingCeo>
///     {
///         public RealityObjectMissingCeoMap()
///             : base("OVRHL_RO_COM_EST_OBJ")
///         {
///             References(x => x.RealityObject, "REAL_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.MissingCommonEstateObject, "CMN_ESTATE_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Конструктивный элемент дома"</summary>
    public class RealityObjectMissingCeoMap : BaseImportableEntityMap<RealityObjectMissingCeo>
    {
        
        public RealityObjectMissingCeoMap() : 
                base("Конструктивный элемент дома", "OVRHL_RO_COM_EST_OBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Объект недвижимости").Column("REAL_OBJ_ID").NotNull().Fetch();
            Reference(x => x.MissingCommonEstateObject, "Конструктивный элемент").Column("CMN_ESTATE_OBJ_ID").NotNull().Fetch();
        }
    }
}
