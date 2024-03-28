/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict.Multipurpose
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities.Dicts.Multipurpose;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class MultipurposeGlossaryMap : BaseImportableEntityMap<MultipurposeGlossary>
///     {
///         public MultipurposeGlossaryMap()
///             : base("GKH_DICT_MULTIGLOSSARY")
///         {
///             Map(x => x.Code, "CODE", true, 200);
///             Map(x => x.Name, "NAME", true, 2000);
///             Bag(x => x.Items, m =>
///             {
///                 m.Key(k => k.Column("GLOSSARY_ID"));
///                 m.Lazy(CollectionLazy.NoLazy);
///                 m.Fetch(CollectionFetchMode.Select);
///                 m.Cascade(Cascade.All);
///                 m.Inverse(true);
///             }, action => action.OneToMany(c => c.Class(typeof(MultipurposeGlossaryItem))));
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts.Multipurpose
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts.Multipurpose;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Универсальный классификатор"</summary>
    public class MultipurposeGlossaryMap : BaseImportableEntityMap<MultipurposeGlossary>
    {

        public MultipurposeGlossaryMap()
            : base("Универсальный классификатор", "GKH_DICT_MULTIGLOSSARY")
        {
        }

        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(2000).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(200).NotNull();
        }
    }

    public class MultipurposeGlossaryNHibernateMapping : ClassMapping<MultipurposeGlossary>
    {
        public MultipurposeGlossaryNHibernateMapping()
        {
            Bag(
                x => x.Items,
                m =>
                    {
                        m.Key(k => k.Column("GLOSSARY_ID"));
                        m.Lazy(CollectionLazy.NoLazy);
                        m.Fetch(CollectionFetchMode.Select);
                        m.Cascade(Cascade.All);
                        m.Inverse(true);
                    },
                action => action.OneToMany(c => c.Class(typeof(MultipurposeGlossaryItem))));
        }
    }
}