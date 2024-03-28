namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг <see cref="PerformedWorkChargeSource"/>
    /// </summary>
    public class PerformedWorkChargeSourceMap : JoinedSubClassMap<PerformedWorkChargeSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PerformedWorkChargeSourceMap() : 
            base("Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations.PerformedWorkChargeSource", "REGOP_PERF_WORK_CHARGE_SOURCE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Sum, "Общая сумма").Column("SUM").NotNull();
            this.Property(x => x.Distributed, "Вся сумма распределена").Column("IS_DISTRIBUTED").NotNull();
            this.Property(x => x.DistributeForBaseTariff, "Распределеить на базовый тариф").Column("DISTRIBUTE_FOR_BASE");
            this.Property(x => x.DistributeForDecisionTariff, "Распределеить на тариф решения").Column("DISTRIBUTE_FOR_DECISION");

            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
        }
    }

    /// <summary>
    /// Маппинг
    /// </summary>
    public class PerformedWorkChargeSourceNHibernateMapping : JoinedSubclassMapping<PerformedWorkChargeSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PerformedWorkChargeSourceNHibernateMapping()
        {
            this.Bag(
                x => x.PerformedWorkCharges,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.Lazy);
                    mapper.Key(
                        k =>
                        {
                            k.Column("CHARGE_OP_ID");
                        });
                    mapper.Cascade(Cascade.Persist | Cascade.DeleteOrphans);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(x => x.Class(typeof(PerformedWorkCharge))));
        }
    }
}
