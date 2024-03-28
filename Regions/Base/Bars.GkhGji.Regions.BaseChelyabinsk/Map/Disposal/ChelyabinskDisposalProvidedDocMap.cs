/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map.Disposal
/// {
/// 	using Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal;
/// 	using FluentNHibernate.Mapping;
/// 
/// 	public class ChelyabinskDisposalProvidedDocMap : SubclassMap<ChelyabinskDisposalProvidedDoc>
///     {
///         public ChelyabinskDisposalProvidedDocMap()
///         {
///             Table("GJI_DISPOSAL_PROVDOC_NSO");
///             KeyColumn("ID");
/// 
///             Map(x => x.Order, "DOC_ORDER");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal.ChelyabinskDisposalProvidedDoc"</summary>
    public class ChelyabinskDisposalProvidedDocMap : JoinedSubClassMap<ChelyabinskDisposalProvidedDoc>
    {
        
        public ChelyabinskDisposalProvidedDocMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal.ChelyabinskDisposalProvidedDoc", "GJI_DISPOSAL_PROVDOC_NSO")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Order, "Order").Column("DOC_ORDER");
        }
    }
}
