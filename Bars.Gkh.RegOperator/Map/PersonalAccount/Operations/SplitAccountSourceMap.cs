namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    public class SplitAccountSourceMap : GkhJoinedSubClassMap<SplitAccountSource>
    {
        /// <inheritdoc />
        public SplitAccountSourceMap()
            : base("REGOP_SPLIT_ACC_SOURCE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.SourceAccount, "Сливаемый лицевой счет").Column("ACCOUNT_ID").NotNull();
        }
    }

    /// <summary>
    /// Маппинг отмен начислений
    /// </summary>
    public class SplitAccountSourceNHibernateMapping : JoinedSubclassMapping<SplitAccountSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SplitAccountSourceNHibernateMapping()
        {
            this.Bag(
                x => x.SplitAccountDetails,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.NoLazy);
                    mapper.Key(k => k.Column("CHARGE_OP_ID"));
                    mapper.Cascade(Cascade.All);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(x => x.Class(typeof(SplitAccountDetail))));
        }
    }
}