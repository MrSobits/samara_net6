namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для <see cref="PaymentCorrectionSource"/>
    /// </summary>
    public class ClosedPeriodPaymentSourceMap : JoinedSubClassMap<ClosedPeriodPaymentSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ClosedPeriodPaymentSourceMap() 
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.ClosedPeriodPaymentSource", "REGOP_CLOSED_PERIOD_PAYMENT")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            // ignored
        }
    }

    /// <summary>
    /// Маппинг корректировок оплат
    /// </summary>
    public class ClosedPeriodPaymentSourceNHibernateMapping : JoinedSubclassMapping<ClosedPeriodPaymentSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ClosedPeriodPaymentSourceNHibernateMapping()
        {
            this.Bag(
                x => x.Payments,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.Lazy);
                    mapper.Key(
                        k =>
                        {
                            k.Column("PAYMENT_OP_ID");
                        });
                    mapper.Cascade(Cascade.Persist);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(x => x.Class(typeof(RecordPaymentsToClosedPeriodsImport))));
        }
    }
}