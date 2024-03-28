/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.B4.DataAccess;
/// 
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Исполнитель обращения"
///     /// </summary>
///     public class ActCheckProvidedDocMap : BaseEntityMap<ActCheckProvidedDoc>
///     {
///         public ActCheckProvidedDocMap()
///             : base("GJI_NSO_ACT_PROVDOC")
///         {
///             Map(x => x.DateProvided, "DATE_PROVIDED");
/// 
///             References(x => x.ProvidedDoc, "PROVDOC_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ActCheck, "ACT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ActCheckProvidedDoc"</summary>
    public class ActCheckProvidedDocMap : BaseEntityMap<ActCheckProvidedDoc>
    {
        
        public ActCheckProvidedDocMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ActCheckProvidedDoc", "GJI_NSO_ACT_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DateProvided, "DateProvided").Column("DATE_PROVIDED");
            this.Reference(x => x.ProvidedDoc, "ProvidedDoc").Column("PROVDOC_ID").NotNull().Fetch();
            this.Reference(x => x.ActCheck, "ActCheck").Column("ACT_ID").NotNull().Fetch();
        }
    }
}
