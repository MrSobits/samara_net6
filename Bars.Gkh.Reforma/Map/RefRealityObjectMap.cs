/// <mapping-converter-backup>
/// namespace Bars.Gkh.Reforma.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Reforma.Entities;
/// 
///     public class RefRealityObjectMap : BaseEntityMap<RefRealityObject>
///     {
///         public RefRealityObjectMap()
///             : base("RFRM_REALITY_OBJECT")
///         {
///             this.Map(x => x.ExternalId, "EXTERNAL_ID", true);
///             this.References(x => x.RefManagingOrganization, "REF_MAN_ORG_ID");
///             this.References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNull);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Reforma.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Reforma.Entities;
    
    
    /// <summary>Маппинг для "Синхронизируемый жилой дом"</summary>
    public class RefRealityObjectMap : BaseEntityMap<RefRealityObject>
    {
        
        public RefRealityObjectMap() : 
                base("Синхронизируемый жилой дом", "RFRM_REALITY_OBJECT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull();
            Property(x => x.ExternalId, "Код дома в Реформе").Column("EXTERNAL_ID").NotNull();
            Reference(x => x.RefManagingOrganization, "Текущая УО").Column("REF_MAN_ORG_ID");
        }
    }
}
