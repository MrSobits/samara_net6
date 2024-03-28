namespace Bars.GisIntegration.UI.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.UI.Entities;
    using Bars.Gkh.DataAccess;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Параметр конфигурации"</summary>
    public class RisConfigParamMap : BaseEntityMap<RisConfigParam>
    {

        public RisConfigParamMap() :
                base("Параметр конфигурации", "RIS_CONFIG_PARAMETER")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Key, "Имя параметра").Column("KEY").Length(500).NotNull();
            this.Property(x => x.Value, "Значение").Column("VALUE");
        }
    }

    public class RisConfigParamNHibernateMapping : ClassMapping<RisConfigParam>
    {
        public RisConfigParamNHibernateMapping()
        {
            this.Property(
                x => x.Value,
                m =>
                {
                    m.Type<ImprovedBinaryStringType>();
                });
        }
    }
}