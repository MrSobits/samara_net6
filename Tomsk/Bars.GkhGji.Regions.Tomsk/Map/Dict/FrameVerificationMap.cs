/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map.Dict
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities.Dict;
/// 
///     public class FrameVerificationMap : BaseEntityMap<FrameVerification>
///     {
///         public FrameVerificationMap()
///             : base("GJI_TOMSK_FRAME_VERIFICATION")
///         {
///             this.Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             this.Map(x => x.Code, "CODE").Length(100);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities.Dict;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.Dict.FrameVerification"</summary>
    public class FrameVerificationMap : BaseEntityMap<FrameVerification>
    {
        
        public FrameVerificationMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.Dict.FrameVerification", "GJI_TOMSK_FRAME_VERIFICATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Code").Column("CODE").Length(100);
        }
    }
}
