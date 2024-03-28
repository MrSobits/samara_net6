/// <mapping-converter-backup>
/// using Bars.GkhGji.Regions.Smolensk.Entities;
/// using FluentNHibernate.Mapping;
/// 
/// namespace Bars.GkhGji.Regions.Smolensk.Map
/// {
///     /// <summary>
///     /// Маппинг для сущности "Распоряжение"
///     /// </summary>
///     public class DisposalSmolMap : SubclassMap<DisposalSmol>
///     {
///         public DisposalSmolMap()
///         {
///             Table("GJI_DISPOSAL_SMOL");
///             KeyColumn("ID");
/// 
///             Map(x => x.VerificationPurpose, "VERIF_PURPOSE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.DisposalSmol"</summary>
    public class DisposalSmolMap : JoinedSubClassMap<DisposalSmol>
    {
        
        public DisposalSmolMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.DisposalSmol", "GJI_DISPOSAL_SMOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.VerificationPurpose, "VerificationPurpose").Column("VERIF_PURPOSE");
        }
    }
}
