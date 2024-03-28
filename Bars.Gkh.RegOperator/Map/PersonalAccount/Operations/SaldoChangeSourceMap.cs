namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для <see cref="SaldoChangeSource"/>
    /// </summary>
    public class SaldoChangeSourceMap : JoinedSubClassMap<SaldoChangeSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SaldoChangeSourceMap()
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.SaldoChangeSource", "REGOP_SALDO_CHANGE_SOURCE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.SaldoChangeExport, "Импорт, который является инициатором изменения").Column("EXPORT_SALDO_ID").Fetch();
        }
    }

    /// <summary>
    /// Маппинг отмен начислений
    /// </summary>
    public class SaldoChangeSourceNHibernateMapping : JoinedSubclassMapping<SaldoChangeSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SaldoChangeSourceNHibernateMapping()
        {
            this.Bag(
                x => x.ChangeDetails,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.NoLazy);
                    mapper.Key(k => k.Column("CHARGE_OP_ID"));
                    mapper.Cascade(Cascade.All);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(x => x.Class(typeof(SaldoChangeDetail))));
        }
    }
}
