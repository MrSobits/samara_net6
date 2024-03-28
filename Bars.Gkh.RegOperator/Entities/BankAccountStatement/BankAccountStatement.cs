namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using DomainModelServices;
    using Enums;
    using Newtonsoft.Json;
    using ValueObjects;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Банковская выписка
    /// </summary>
    public partial class BankAccountStatement : BaseImportableEntity, IDistributable, IHaveExportId
    {
        private readonly IList<MoneyOperation> operations;
        private readonly IList<DistributionOperation> distributionOperations;

        public BankAccountStatement()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
            this.operations = new List<MoneyOperation>();
            this.distributionOperations = new List<DistributionOperation>();
        }

        /// <summary>
        /// Идентификатор для экспорта
        /// </summary>
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Сумма по документу
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Остаток
        /// </summary>
        public virtual decimal RemainSum { get; set; }

        /// <summary>
        /// Гуид
        /// </summary>
        public virtual string TransferGuid { get; protected set; }

        /// <summary>
        /// Источник распределения
        /// </summary>
        DistributionSource IDistributable.Source
        {
            get { return DistributionSource.BankStatement; }
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public virtual string DistributionCode { get; set; }

        /// <summary>
        /// Приход/Расход
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Направление движения средств (приход/расход)
        /// </summary>
        public virtual MoneyDirection MoneyDirection { get; set; }

        /// <summary>
        /// Дата прихода
        /// </summary>
        public virtual DateTime DateReceipt { get; set; }

        /// <summary>
        /// Дата распределения
        /// </summary>
        public virtual DateTime? DistributionDate { get; set; }

        /// <summary>
        /// Назначение платежа
        /// </summary>
        public virtual string PaymentDetails { get; set; }

        /// <summary>
        /// взыскан РОСП
        /// </summary>
        public virtual bool IsROSP { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual DistributionState DistributeState { get; set; }

        /// <summary>
        /// Распределение возможно
        /// </summary>
        public virtual YesNo IsDistributable { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public virtual string UserLogin { get; set; }

        #region Payer

        /// <summary>
        /// Плательщик
        /// </summary>
        public virtual Contragent Payer { get; set; }

        /// <summary>
        /// Плательщик полностью (строка)
        /// </summary>
        public virtual string PayerFull { get; set; }

        /// <summary>
        /// Наименование плательщика
        /// </summary>
        public virtual string PayerName { get; set; }

        /// <summary>
        /// Расчетный счет плательщика
        /// </summary>
        public virtual string PayerAccountNum { get; set; }

        /// <summary>
        /// ИНН плательщика
        /// </summary>
        public virtual string PayerInn { get; set; }

        /// <summary>
        /// КПП плательщика
        /// </summary>
        public virtual string PayerKpp { get; set; }

        /// <summary>
        /// БИК плательщика
        /// </summary>
        public virtual string PayerBik { get; set; }

        /// <summary>
        /// Банк плательщика
        /// </summary>
        public virtual string PayerBank { get; set; }

        /// <summary>
        /// Корр счет плательщика
        /// </summary>
        public virtual string PayerCorrAccount { get; set; }

        #endregion Payer

        #region Recipient

        /// <summary>
        /// Получатель
        /// </summary>
        public virtual Contragent Recipient { get; set; }

        /// <summary>
        /// Наименование плательщика
        /// </summary>
        public virtual string RecipientName { get; set; }

        /// <summary>
        /// Расчетный счет получателя
        /// </summary>
        public virtual string RecipientAccountNum { get; set; }

        /// <summary>
        /// ИНН получателя
        /// </summary>
        public virtual string RecipientInn { get; set; }

        /// <summary>
        /// КПП получателя
        /// </summary>
        public virtual string RecipientKpp { get; set; }

        /// <summary>
        /// БИК получателя
        /// </summary>
        public virtual string RecipientBik { get; set; }

        /// <summary>
        /// Банк получателя
        /// </summary>
        public virtual string RecipientBank { get; set; }

        /// <summary>
        /// Корр счет получателя
        /// </summary>
        public virtual string RecipientCorrAccount { get; set; }

        #endregion Recipient

        /// <summary>
        /// Строка связанных документов
        /// </summary>
        public virtual string LinkedDocuments { get; set; }

        /// <summary>
        /// Проведенные операции
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<MoneyOperation> Operations
        {
            get { return this.operations; }
        }

        /// <summary>
        /// Проведенные виды распределений со связью с операциями
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DistributionOperation> DistributionOperations
        {
            get { return this.distributionOperations; }
        }

        /// <summary>
        /// Ссылка на группу выписок (устаревшее, необходимо потом выпилить)
        /// </summary>
        public virtual BankAccountStatementGroup Group { get; set; }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Ссылка на SuspenseAccount
        /// </summary>
        [Obsolete("Ссылка на SuspenseAccount, после окончательной миграции всех счетов невыясненных сумм выпилить")]
        public virtual SuspenseAccount SuspenseAccount { get; set; }

        /// <summary>
        /// Костыль, написан для того чтобы смигрировать данные с реестра непотвержденные оплаты НВС
        /// </summary>
        public virtual void SetGuid(string value)
        {
            this.TransferGuid = value;
        }
    }
}
