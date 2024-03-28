/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Entities;
/// 
///     public class RealityObjectStructuralElementAttributeValueMap : BaseImportableEntityMap<RealityObjectStructuralElementAttributeValue>
///     {
///         public RealityObjectStructuralElementAttributeValueMap() : base("OVRHL_RO_SE_VALUE")
///         {
///             References(x => x.Attribute, "ATR_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Object, "OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Value, "VALUE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Атрибут конструктивного элемента жилого дома"</summary>
    public class RealityObjectStructuralElementAttributeValueMap : BaseImportableEntityMap<RealityObjectStructuralElementAttributeValue>
    {
        
        public RealityObjectStructuralElementAttributeValueMap() : 
                base("Атрибут конструктивного элемента жилого дома", "OVRHL_RO_SE_VALUE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Attribute, "Атрибут").Column("ATR_ID").NotNull().Fetch();
            Reference(x => x.Object, "КЭ жилого дома").Column("OBJ_ID").NotNull().Fetch();
            Property(x => x.Value, "Значение").Column("VALUE").Length(250);
        }
    }
}
