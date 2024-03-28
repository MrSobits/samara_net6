namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;

    using Gkh.Enums;
    using Entities;
    using Enums;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Банковская выписка"</summary>
    public class BankAccountStatementMap : BaseImportableEntityMap<BankAccountStatement>
    {

        public BankAccountStatementMap()
            : base("Банковская выписка", "REGOP_BANK_ACC_STMNT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExportId, "Идентификатор для экспорта").Column("EXPORT_ID").NotNull();
            this.Property(x => x.OperationDate, "Дата операции").Column("OP_DATE");
            this.Property(x => x.IsROSP, "Взыскано РОСП").Column("ROSP");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOC_DATE");
            this.Property(x => x.Sum, "Сумма по документу").Column("DOC_SUM");
            this.Property(x => x.RemainSum, "Остаток").Column("REMAIN_SUM");
            this.Property(x => x.TransferGuid, "Гуид").Column("TRANSFER_GUID").Length(40).NotNull();
            this.Property(x => x.DistributionCode, "Код распределения").Column("DISTR_CODE").Length(100);
            this.Property(x => x.DocumentNum, "Приход/Расход").Column("DOC_NUM").Length(250);
            this.Property(x => x.MoneyDirection, "Направление движения средств (приход/расход)")
                .Column("MONEY_DIRECTION")
                .DefaultValue(MoneyDirection.Income)
                .NotNull();
            this.Property(x => x.DateReceipt, "Дата прихода").Column("RECEIPT_DATE");
            this.Property(x => x.DistributionDate, "Дата распределения").Column("D_DATE");
            this.Property(x => x.PaymentDetails, "Назначение платежа").Column("P_DETAILS").Length(250);
            this.Property(x => x.DistributeState, "Статус")
                .Column("STATE")
                .DefaultValue(DistributionState.NotDistributed)
                .NotNull();
            this.Property(x => x.IsDistributable, "Распределение возможно")
                .Column("IS_DISTRIBUTABLE")
                .DefaultValue(YesNo.Yes)
                .NotNull();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Property(x => x.UserLogin, "Логин пользователя").Column("USER_LOGIN").Length(250);
            this.Reference(x => x.Payer, "Плательщик").Column("PAYER_CONTRAGENT_ID").Fetch();
            this.Property(x => x.PayerFull, "Плательщик полностью (строка)").Column("PAYER_FULL").Length(200);
            this.Property(x => x.PayerName, "Наименование плательщика").Column("PAYER_NAME").Length(200);
            this.Property(x => x.PayerAccountNum, "Расчетный счет плательщика").Column("P_ACC_NUM").Length(250);
            this.Property(x => x.PayerInn, "ИНН плательщика").Column("PAYER_INN").Length(200);
            this.Property(x => x.PayerKpp, "КПП плательщика").Column("PAYER_KPP").Length(200);
            this.Property(x => x.PayerBik, "БИК плательщика").Column("PAYER_BIK").Length(200);
            this.Property(x => x.PayerBank, "Банк плательщика").Column("PAYER_BANK").Length(200);
            this.Property(x => x.PayerCorrAccount, "Корр счет плательщика").Column("PAYER_CORR_ACC").Length(200);
            this.Reference(x => x.Recipient, "Получатель").Column("RECIPIENT_CONTRAGENT_ID").Fetch();
            this.Property(x => x.RecipientName, "Наименование плательщика").Column("RECIPIENT_NAME").Length(200);
            this.Property(x => x.RecipientAccountNum, "Расчетный счет получателя").Column("ACC_NUM").Length(250);
            this.Property(x => x.RecipientInn, "ИНН получателя").Column("RECIPIENT_INN").Length(200);
            this.Property(x => x.RecipientKpp, "КПП получателя").Column("RECIPIENT_KPP").Length(200);
            this.Property(x => x.RecipientBik, "БИК получателя").Column("RECIPIENT_BIK").Length(200);
            this.Property(x => x.RecipientBank, "Банк получателя").Column("RECIPIENT_BANK").Length(200);
            this.Property(x => x.RecipientCorrAccount, "Корр счет получателя").Column("RECIPIENT_CORR_ACC").Length(200);
            this.Property(x => x.LinkedDocuments, "Строка связанных документов").Column("LINKED_DOCUMENTS").Length(2000);
            this.Reference(x => x.Group, "Ссылка на группу выписок (устаревшее, необходимо потом выпилить)")
                .Column("STMNT_GROUP")
                .Fetch();
            this.Reference(x => x.Document, "Документ-основание").Column("DOCUMENT_ID").Fetch();
            this.Reference(x => x.SuspenseAccount, "Ссылка на SuspenseAccount").Column("SUSPENSE_ACC_ID").Fetch();
        }
    }

    public class BankAccountStatementNHibernateMapping : BaseHaveExportIdMapping<BankAccountStatement>
    {
        public BankAccountStatementNHibernateMapping()
        {
            this.Bag(
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
                        mapper.Cascade(Cascade.Persist);
                        mapper.Inverse(true);
                    },
                action => action.OneToMany());

            this.Bag(
                x => x.DistributionOperations,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.Lazy);
                    mapper.Key(
                        k =>
                        {
                            k.Column("BANK_STMNT_ID");
                            //k.PropertyRef(x => x.Id);
                        });
                    mapper.Cascade(Cascade.Persist);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(c => c.Class(typeof(DistributionOperation))));
        }
    }
}