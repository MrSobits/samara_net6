/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     /// <summary>
///     /// мапинг для Реестра субсидий
///     /// </summary>
///     public class SubsidyIncomeMap : BaseImportableEntityMap<SubsidyIncome>
///     {
///         public SubsidyIncomeMap()
///             : base("REGOP_SUBSIDY_INCOME")
///         {
///             Map(x => x.DateReceipt, "DATE_RECEIPT");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.RemainSum, "REMAIN_SUM");
///             Map(x => x.DistributeState, "SUBSIDY_INCOME_STATUS");
///             Map(x => x.MoneyDirection, "MONEY_DIRECTION", true);
///             Map(x => x.TransferGuid, "C_GUID", false, length:40);
///             Map(x => x.DistributionCode, "DCODE", false, length:250);
///             Map(x => x.TypeSubsidyDistr, "SUBSIDY_DISTR_TYPE", false, length: 250);
///             Map(x => x.DistributionDate, "D_DATE");
///             Map(x => x.DetailsCount, "DETAILS_CNT");
///             Map(x => x.SubsidyIncomeDefineType, "DEFINE_TYPE");
///             References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.Fetch);
///             References(x => x.BankAccountStatement, "BANK_ACC_STMNT_ID", ReferenceMapConfig.Fetch);
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

    /// <summary>Маппинг для "Реестр субсидий"</summary>
    public class SubsidyIncomeMap : BaseImportableEntityMap<SubsidyIncome>
    {
        
        public SubsidyIncomeMap() : 
                base("Реестр субсидий", "REGOP_SUBSIDY_INCOME")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateReceipt, "Дата поступления").Column("DATE_RECEIPT");
            Property(x => x.DistributionDate, "Дата распределения").Column("D_DATE");
            Property(x => x.DistributeState, "Состояние (распределен/не распределен)").Column("SUBSIDY_INCOME_STATUS");
            Property(x => x.MoneyDirection, "Направление движения средств (приход/расход)").Column("MONEY_DIRECTION").NotNull();
            Property(x => x.Sum, "Сумма").Column("SUM");
            Property(x => x.RemainSum, "Остаток").Column("REMAIN_SUM");
            Reference(x => x.Document, "Документ-основание").Column("DOCUMENT_ID").Fetch();
            Property(x => x.DistributionCode, "Коды распределения (не пуст если SuspenseAccountStatus != NotDistributed )").Column("DCODE").Length(250);
            Property(x => x.TypeSubsidyDistr, "Предполагаемые коды распределения").Column("SUBSIDY_DISTR_TYPE").Length(250);
            Property(x => x.TransferGuid, "На этот гуид будут вязаться различные операции, происходящие при распределении. Н" +
                    "апример, возврат займа дома").Column("C_GUID").Length(40);
            Property(x => x.DetailsCount, "Количество записей").Column("DETAILS_CNT");
            Reference(x => x.BankAccountStatement, "Банковская выписка").Column("BANK_ACC_STMNT_ID").Fetch();
            Property(x => x.SubsidyIncomeDefineType, "Определение домов ЛС").Column("DEFINE_TYPE");
        }
    }

    public class SubsidyIncomeNHibernateMapping : ClassMapping<SubsidyIncome>
    {
        public SubsidyIncomeNHibernateMapping()
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
