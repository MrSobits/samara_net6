/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     /// <summary>
///     /// мапинг для Счета невыясненных сумм
///     /// </summary>
///     public class SuspenseAccountMap : BaseImportableEntityMap<SuspenseAccount>
///     {
///         public SuspenseAccountMap()
///             : base("REGOP_SUSPENSE_ACCOUNT")
///         {
///             Map(x => x.DateReceipt, "DATE_RECEIPT");
///             Map(x => x.SuspenseAccountTypePayment, "SUSPEN_ACCOUNT_TYPE");
///             Map(x => x.AccountBeneficiary, "ACCOUNT_BENEFICIARY");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.RemainSum, "REMAIN_SUM");
///             Map(x => x.DetailsOfPayment, "DETAILS_OF_PAYMENT");
///             Map(x => x.DistributeState, "SUSPEN_ACCOUNT_STATUS");
///             Map(x => x.Reason, "REASON");
///             Map(x => x.MoneyDirection, "MONEY_DIRECTION", true);
///             Map(x => x.TransferGuid, "C_GUID", false, length:40);
///             Map(x => x.DistributionCode, "DCODE", false, length:250);
///             Map(x => x.DistributionDate, "D_DATE");
///             References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.Fetch);
/// 
///             Bag(x => x.Operations, mapper =>
///             {
///                 mapper.Access(Accessor.NoSetter);
///                 mapper.Fetch(CollectionFetchMode.Select);
///                 mapper.Lazy(CollectionLazy.Lazy);
///                 mapper.Key(k =>
///                 {
///                     k.Column("ORIGINATOR_GUID");
///                     k.PropertyRef(x => x.TransferGuid);
///                 });
///                 mapper.Cascade(Cascade.Remove | Cascade.Persist);
///                 mapper.Inverse(true);
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Счет невыясненных сумм"</summary>
    public class SuspenseAccountMap : BaseImportableEntityMap<SuspenseAccount>
    {
        
        public SuspenseAccountMap() : 
                base("Счет невыясненных сумм", "REGOP_SUSPENSE_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateReceipt, "Дата поступления").Column("DATE_RECEIPT");
            Property(x => x.DistributionDate, "Дата распределения").Column("D_DATE");
            Property(x => x.DistributeState, "Состояние (распределен/не распределен)").Column("SUSPEN_ACCOUNT_STATUS");
            Property(x => x.SuspenseAccountTypePayment, "Тип платежа").Column("SUSPEN_ACCOUNT_TYPE");
            Property(x => x.MoneyDirection, "Направление движения средств (приход/расход)").Column("MONEY_DIRECTION").NotNull();
            Property(x => x.AccountBeneficiary, "Рас.счет получателя").Column("ACCOUNT_BENEFICIARY").Length(250);
            Property(x => x.Sum, "Сумма").Column("SUM");
            Property(x => x.RemainSum, "Остаток").Column("REMAIN_SUM");
            Property(x => x.DetailsOfPayment, "Назначение платежа").Column("DETAILS_OF_PAYMENT").Length(250);
            Property(x => x.Reason, "Причина").Column("REASON").Length(250);
            Reference(x => x.Document, "Документ-основание").Column("DOCUMENT_ID").Fetch();
            Property(x => x.DistributionCode, "Код распределения (не пуст если SuspenseAccountStatus != NotDistributed )").Column("DCODE").Length(250);
            Property(x => x.TransferGuid, "На этот гуид будут вязаться различные операции, происходящие при распределении. Н" +
                    "апример, возврат займа дома").Column("C_GUID").Length(40);
        }
    }

    public class SuspenseAccountNHibernateMapping : ClassMapping<SuspenseAccount>
    {
        public SuspenseAccountNHibernateMapping()
        {
            Bag(
                x => x.Operations,
                mapper =>
                    {
                        mapper.Access(Accessor.NoSetter);
                        mapper.Fetch(CollectionFetchMode.Select);
                        mapper.Lazy(CollectionLazy.Lazy);
                        mapper.Key(
                            k =>
                                {
                                    k.Column("ORIGINATOR_GUID");
                                    k.PropertyRef(x => x.TransferGuid);
                                });
                        mapper.Cascade(Cascade.Remove | Cascade.Persist);
                        mapper.Inverse(true);
                    },
                action => action.OneToMany());
        }
    }
}
