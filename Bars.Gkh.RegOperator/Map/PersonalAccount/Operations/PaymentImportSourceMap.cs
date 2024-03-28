namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для <see cref="PaymentImportSource"/>
    /// </summary>
    public class PaymentImportSourceMap : JoinedSubClassMap<PaymentImportSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PaymentImportSourceMap()
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations.PaymentImportSource", "REGOP_PAYMENT_IMPORT_SOURCE")
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
    /// Маппинг имопрта оплат
    /// </summary>
    public class PaymentImportSourceNHibernateMapping : JoinedSubclassMapping<PaymentImportSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PaymentImportSourceNHibernateMapping()
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
                action => action.OneToMany(x => x.Class(typeof(PaymentImport))));
        }
    }
}