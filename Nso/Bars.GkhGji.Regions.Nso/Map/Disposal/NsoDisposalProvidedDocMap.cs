/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map.Disposal
/// {
/// 	using Bars.GkhGji.Regions.Nso.Entities.Disposal;
/// 	using FluentNHibernate.Mapping;
/// 
/// 	public class NsoDisposalProvidedDocMap : SubclassMap<NsoDisposalProvidedDoc>
///     {
///         public NsoDisposalProvidedDocMap()
///         {
///             Table("GJI_DISPOSAL_PROVDOC_NSO");
///             KeyColumn("ID");
/// 
///             Map(x => x.Order, "DOC_ORDER");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.Disposal.NsoDisposalProvidedDoc"</summary>
    public class NsoDisposalProvidedDocMap : JoinedSubClassMap<NsoDisposalProvidedDoc>
    {
        
        public NsoDisposalProvidedDocMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.Disposal.NsoDisposalProvidedDoc", "GJI_DISPOSAL_PROVDOC_NSO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Order, "Order").Column("DOC_ORDER");
        }
    }
}
