namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для <see cref="CancelChargeSource"/>
    /// </summary>
    public class CancelChargeSourceMap : JoinedSubClassMap<CancelChargeSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public CancelChargeSourceMap()
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.CancelCharge", "REGOP_CHARGE_CANCEL_SOURCE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
        }
    }

    /// <summary>
    /// Маппинг отмен начислений
    /// </summary>
    public class CancelChargeSourceNHibernateMapping : JoinedSubclassMapping<CancelChargeSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public CancelChargeSourceNHibernateMapping()
        {
            this.Bag(
                x => x.CancelCharges,
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
                    mapper.Cascade(Cascade.Persist);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(x => x.Class(typeof(CancelCharge))));
        }
    }
}
