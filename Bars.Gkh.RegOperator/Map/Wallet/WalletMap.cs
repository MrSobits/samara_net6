namespace Bars.Gkh.RegOperator.Map.Wallet
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Wallet;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Кошелек оплат. Содержит информацию по оплатам"</summary>
    public class WalletMap : BaseImportableEntityMap<Wallet>
    {
        /// <inheritdoc />
        public WalletMap() : 
                base("Кошелек оплат. Содержит информацию по оплатам", "REGOP_WALLET")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.WalletGuid, "Уникальный гуид кошелька").Column("WALLET_GUID").Length(250).NotNull();
            this.Property(x => x.Balance, "Баланс кошелька").Column("BALANCE");
            this.Property(x => x.HasNewOperations, "Вспомогательное свойство для фильтрации по новым операциям").Column("HAS_NEW_OPS");
            this.Property(x => x.OwnerType, "Тип владельца").Column("OWNER_TYPE").NotNull();
            this.Property(x => x.WalletType, "Тип кошелька").Column("WALLET_TYPE").NotNull();
            this.Property(x => x.Repayment, "Перераспределение").Column("REPAYMENT");
        }
    }

    // <summary>Маппинг для "Кошелек оплат. Содержит информацию по оплатам"</summary>
    public class WalletNHibernateMapping : ClassMapping<Wallet>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public WalletNHibernateMapping()
        {
            this.Property(
                x => x.TransferGuid,
                m =>
                    {
                        m.Column("WALLET_GUID");
                        m.Insert(false);
                        m.Update(false);
                        m.Access(Accessor.ReadOnly);
                    });

            this.Bag(
                x => x.MoneyLocks,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.Extra);
                    mapper.Key(k => k.Column("WALLET_ID"));
                    mapper.Inverse(true);
                    mapper.Cascade(Cascade.Remove);
                },
                action => action.OneToMany());

            this.MapTransfer(x => x.RealityObjectInTransfers, "TARGET_GUID");
            this.MapTransfer(x => x.RealityObjectOutTransfers, "SOURCE_GUID");

            this.MapTransfer(x => x.AccountPaymenInTransfers, "TARGET_GUID");
            this.MapTransfer(x => x.AccountPaymenOutTransfers, "SOURCE_GUID");

            this.MapTransfer(x => x.AccountChargeInTransfers, "TARGET_GUID");
            this.MapTransfer(x => x.AccountChargeOutTransfers, "SOURCE_GUID");
        }

        private void MapTransfer<TElement>(Expression<Func<Wallet, IEnumerable<TElement>>> property, string columnName)
        {
            this.Bag(
               property,
               mapper =>
               {
                   mapper.Access(Accessor.NoSetter);
                   mapper.Fetch(CollectionFetchMode.Select);
                   mapper.Lazy(CollectionLazy.Extra);
                   mapper.Key(
                                k =>
                           {
                               k.Column(columnName);
                               k.PropertyRef(x => x.WalletGuid);
                           });
                   mapper.Cascade(Cascade.Remove);
                   mapper.Inverse(true);
               },
               action => action.OneToMany());
        }
    }
}
