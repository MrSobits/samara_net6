/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map.AppealCits
/// {
///     using Entities.AppealCits;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class ChelyabinskAppealCitsMap : SubclassMap<ChelyabinskAppealCits>
///     {
///         public ChelyabinskAppealCitsMap()
///         {
///             Table("CHEL_GJI_APPEAL_CITIZENS");
///             KeyColumn("ID");
/// 
///             Map(x => x.Comment, "COMMENT").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.AppealCits
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.AppealCits.ChelyabinskAppealCits"</summary>
    public class ChelyabinskAppealCitsMap : JoinedSubClassMap<ChelyabinskAppealCits>
    {
        
        public ChelyabinskAppealCitsMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.AppealCits.ChelyabinskAppealCits", "CHEL_GJI_APPEAL_CITIZENS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Comment, "Comment").Column("COMMENT").Length(2000);
        }
    }
}
