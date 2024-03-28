namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class ServiceOrgFuelEnergyResourcePeriodSummMap : BaseEntityMap<ServiceOrgFuelEnergyResourcePeriodSumm>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ServiceOrgFuelEnergyResourcePeriodSummMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.ServiceOrgFuelEnergyResourcePeriodSumm", "GKH_SERV_ORG_FUEL_ENERGY_PERIOD_SUMM")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY_ID").NotNull();
            this.Reference(x => x.ContractPeriod, "Отчетный период договора").Column("PERIOD_ID").NotNull();
            this.Reference(x => x.PublicServiceOrg, "РСО").Column("PUB_SERV_ORG_ID").NotNull();
        }
    }

    /// <summary>
    /// NH маппинг
    /// </summary>
    public class ServiceOrgFuelEnergyResourcePeriodSummNHibernateMapping : ClassMapping<ServiceOrgFuelEnergyResourcePeriodSumm>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ServiceOrgFuelEnergyResourcePeriodSummNHibernateMapping()
        {
            this.Bag(
                x => x.Details,
                m =>
                {
                    m.Access(Accessor.Property);
                    m.Key(k => k.Column("PER_SUMM_ID"));
                    m.Lazy(CollectionLazy.Lazy);
                    m.Fetch(CollectionFetchMode.Select);
                    m.Inverse(true);
                },
                action => action.OneToMany());
        }
    }
}