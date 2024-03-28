/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.DataAccess.UserTypes;
///     using Bars.Gkh.Entities;
/// 
///     public class GkhConfigParamMap : BaseEntityMap<GkhConfigParam>
///     {
///         public GkhConfigParamMap()
///             : base("GKH_CONFIG_PARAMETER")
///         {
///             this.Map(x => x.Key, "KEY", true, 500);
///             this.Property(
///                 x => x.Value,
///                 m =>
///                     {
///                         m.Column("VALUE");
///                         m.Type<BinaryStringType>();
///                     });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Параметр конфигурации"</summary>
    public class GkhConfigParamMap : BaseEntityMap<GkhConfigParam>
    {
        
        public GkhConfigParamMap() : 
                base("Параметр конфигурации", "GKH_CONFIG_PARAMETER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Key, "Имя параметра").Column("KEY").Length(500).NotNull();
            Property(x => x.Value, "Значение").Column("VALUE");
        }
    }

    public class GkhConfigParamNHibernateMapping : ClassMapping<GkhConfigParam>
    {
        public GkhConfigParamNHibernateMapping()
        {
            Property(
                x => x.Value,
                m =>
                    {
                        m.Type<ImprovedBinaryStringType>();
                    });
        }
    }
}
