namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для <see cref="ImportCancelChargeSource"/>
    /// </summary>
    public class ImportCancelChargeSourceMap : JoinedSubClassMap<ImportCancelChargeSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ImportCancelChargeSourceMap()
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations.ImportCancelChargeSource",
                  "REGOP_IMPORT_CHARGE_CANCEL_SOURCE")
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
    /// Маппинг отмен начислений из импорта
    /// </summary>
    public class ImportCancelChargeSourceNHibernateMapping : JoinedSubclassMapping<ImportCancelChargeSource>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ImportCancelChargeSourceNHibernateMapping()
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
