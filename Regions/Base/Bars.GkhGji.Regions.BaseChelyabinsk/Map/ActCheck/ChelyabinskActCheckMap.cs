/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Акт проверки"
///     /// </summary>
///     public class ChelyabinskActCheckMap : SubclassMap<ChelyabinskActCheck>
///     {
///         public ChelyabinskActCheckMap()
///         {
///             Table("GJI_NSO_ACTCHECK");
///             KeyColumn("ID");
///             Map(x => x.AcquaintedWithDisposalCopy, "ACQUAINT_WITH_DISP");
///             Map(x => x.DocumentPlace, "DOCUMENT_PLACE").Length(1000);
/// 			Map(x => x.DocumentTime, "DOCUMENT_TIME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActCheck
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskActCheck"</summary>
    public class ChelyabinskActCheckMap : JoinedSubClassMap<ChelyabinskActCheck>
    {
        
        public ChelyabinskActCheckMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskActCheck", "GJI_NSO_ACTCHECK")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.AcquaintedWithDisposalCopy, "AcquaintedWithDisposalCopy").Column("ACQUAINT_WITH_DISP");
        }
    }
}
