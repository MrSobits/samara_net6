namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для <see cref="PaymentCorrectionSource"/>
    /// </summary>
    public class PaymentCorrectionSourceMap : JoinedSubClassMap<PaymentCorrectionSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PaymentCorrectionSourceMap() 
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.PaymentCorrection", "REGOP_PAYMENT_CORRECTION_SOURCE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "Лицевой счёт").Column("ACC_ID").NotNull().Fetch();
        }
    }

    /// <summary>
    /// Маппинг корректировок оплат
    /// </summary>
    public class PaymentCorrectionSourceNHibernateMapping : JoinedSubclassMapping<PaymentCorrectionSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PaymentCorrectionSourceNHibernateMapping()
        {
            this.Bag(
                x => x.PaymentCorrections,
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
                action => action.OneToMany(x => x.Class(typeof(PaymentCorrection))));
        }
    }
}