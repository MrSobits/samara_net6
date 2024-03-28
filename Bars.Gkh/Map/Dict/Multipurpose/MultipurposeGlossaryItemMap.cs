/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict.Multipurpose
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities.Dicts.Multipurpose;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class MultipurposeGlossaryItemMap : BaseImportableEntityMap<MultipurposeGlossaryItem>
///     {
///         public MultipurposeGlossaryItemMap()
///             : base("GKH_DICT_MULTIITEM")
///         {
///             Map(x => x.Key, "KEY", true, 200);
///             Map(x => x.Value, "VALUE", true, 200);
/// 
///             ManyToOne(x => x.Glossary, m =>
///              {
///                  m.Column("GLOSSARY_ID");
///                  m.NotNullable(true);
///                  m.Fetch(FetchKind.Join);
///                  m.Lazy(LazyRelation.NoLazy);
///              });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts.Multipurpose
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    
    
    /// <summary>Маппинг для "Наполнитель универсального справочника"</summary>
    public class MultipurposeGlossaryItemMap : BaseImportableEntityMap<MultipurposeGlossaryItem>
    {
        
        public MultipurposeGlossaryItemMap() : 
                base("Наполнитель универсального справочника", "GKH_DICT_MULTIITEM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Key, "Ключ").Column("KEY").Length(200).NotNull();
            Property(x => x.Value, "Значение").Column("VALUE").Length(200).NotNull();
            Reference(x => x.Glossary, "Справочник").Column("GLOSSARY_ID").NotNull().Fetch();
        }
    }
}
