/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class FsGorodImportInfoMap : BaseImportableEntityMap<FsGorodImportInfo>
///     {
///         public FsGorodImportInfoMap()
///             : base("REGOP_FS_IMPORT_INFO")
///         {
///             Map(x => x.Code, "CODE");
///             Map(x => x.DataHeadIndex, "DATA_HEAD_INDEX");
///             Map(x => x.Description, "DESCR");
///             Map(x => x.Name, "NAME");
///             Map(x => x.Delimiter, "DELIMITER");
/// 
///             Bag(x => x.MapItems, m =>
///             {
///                 m.Access(Accessor.NoSetter);
///                 m.Key(k => k.Column("INFO_ID"));
///                 m.Lazy(CollectionLazy.Lazy);
///                 m.Fetch(CollectionFetchMode.Select);
///                 m.Cascade(Cascade.Remove);
///                 m.Inverse(true);
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.FsGorodImportInfo"</summary>
    public class FsGorodImportInfoMap : BaseImportableEntityMap<FsGorodImportInfo>
    {
        
        public FsGorodImportInfoMap() : 
                base("Bars.Gkh.RegOperator.Entities.FsGorodImportInfo", "REGOP_FS_IMPORT_INFO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE").Length(250);
            Property(x => x.Name, "Name").Column("NAME").Length(250);
            Property(x => x.Description, "Description").Column("DESCR").Length(250);
            Property(x => x.DataHeadIndex, "DataHeadIndex").Column("DATA_HEAD_INDEX");
            Property(x => x.Delimiter, "Delimiter").Column("DELIMITER").Length(250);
        }
    }

    public class FsGorodImportInfoNHibernateMapping : ClassMapping<FsGorodImportInfo>
    {
        public FsGorodImportInfoNHibernateMapping()
        {
            Bag(
                x => x.MapItems,
                m =>
                    {
                        m.Access(Accessor.NoSetter);
                        m.Key(k => k.Column("INFO_ID"));
                        m.Lazy(CollectionLazy.Lazy);
                        m.Fetch(CollectionFetchMode.Select);
                        m.Cascade(Cascade.Remove);
                        m.Inverse(true);
                    },
                action => action.OneToMany());
        }
    }
}
