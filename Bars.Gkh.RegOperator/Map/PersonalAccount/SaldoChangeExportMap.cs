namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.PersonalAccount.SaldoChangeExport"</summary>
    public class SaldoChangeExportMap : BaseImportableEntityMap<SaldoChangeExport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SaldoChangeExportMap() : 
                base("Bars.Gkh.RegOperator.Entities.PersonalAccount.SaldoChangeExport", "REGOP_SALDO_CHANGE_EXPORT")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.FileName, "Имя выгруженного ранее файла").Column("FILE_NAME").Length(30).NotNull();
            this.Property(x => x.Imported, "Производился ли импорт указанного файла").Column("IMPORTED").NotNull();
            this.Reference(x => x.Period, "Период выгрузки").Column("PERIOD_ID").NotNull();
        }
    }

    /// <summary>
    /// Маппинг лицевых счетов
    /// </summary>
    public class PaymentCorrectionSourceNHibernateMapping : ClassMapping<SaldoChangeExport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PaymentCorrectionSourceNHibernateMapping()
        {
            this.Bag(
                x => x.Accounts,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.NoLazy);
                    mapper.Key(k => k.Column("CHANGE_ID"));
                    mapper.Cascade(Cascade.All | Cascade.DeleteOrphans);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(x => x.Class(typeof(AccountExcelSaldoChange))));
        }
    }
}
