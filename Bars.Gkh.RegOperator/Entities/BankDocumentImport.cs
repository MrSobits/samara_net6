namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;

    using DomainModelServices;
    using Enums;
    using Gkh.Enums;
    using ValueObjects;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Документы, загруженные из банка
    /// </summary>
    public class BankDocumentImport : BaseImportableEntity, ITransferParty, IMoneyOperationSource, IHaveExportId
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BankDocumentImport()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Идентификатор для экспорта
        /// </summary>
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Дата загрузки документа
        /// </summary>
        public virtual DateTime ImportDate { get; set; }

        /// <summary>
        /// Информация о типе загруженного документа
        /// </summary>
        public virtual string DocumentType { get; set; }

        /// <summary>
        /// Дата, указанная в загружаемом документе
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата подтверждения
        /// </summary>
        public virtual DateTime? AcceptDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Сумма по реестру
        /// </summary>
        public virtual decimal? ImportedSum { get; set; }

        /// <summary>
        /// Подтвержденная сумма
        /// </summary>
        public virtual decimal? AcceptedSum { get; set; }

        /// <summary>
        /// Учтенная при подтверждении сумма
        /// </summary>
        public virtual decimal? DistributedSum { get; set; }

        /// <summary>
        /// Состояние импорта
        /// </summary>
        public virtual BankDocumentImportStatus Status { get; set; }

        /// <summary>
        /// Ссылка на лог загрузки и сам документ
        /// </summary>
        public virtual LogImport LogImport { get; set; }

        /// <summary>
        /// Код платежного агента
        /// </summary>
        public virtual string PaymentAgentCode { get; set; }

        /// <summary>
        /// Наименование платежного агента
        /// </summary>
        public virtual string PaymentAgentName { get; set; }

        /// <summary>
        /// Распределять на задолженность по пени
        /// </summary>
        public virtual YesNo DistributePenalty { get; set; }

        /// <summary>
        /// Статус определения ЛС
        /// </summary>
        public virtual PersonalAccountDeterminationState PersonalAccountDeterminationState { get; set; }

        /// <summary>
        /// Статус подтверждения оплат
        /// </summary>
        public virtual PaymentConfirmationState PaymentConfirmationState { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public virtual PaymentOrChargePacketState State { get; protected set; }

        /// <summary>
        /// Строка связанной Банковской выписки
        /// </summary>
        public virtual string BankStatement { get; set; }

        /// <summary>
        /// Тип импорта
        /// </summary>
        public virtual string ImportType { get; set; }

        /// <summary>
        /// Проверка пройдена
        /// </summary>
        public virtual BankDocumentImportCheckState CheckState { get; set; }

        /// <summary>
        /// Дата выгрузки отчета по документу
        /// </summary>
        public virtual DateTime? ReportDate { get; set; }

        /// <summary>
        /// Поставить состояние Обрабатывается
        /// </summary>
        public virtual void SetInProgress()
        {
            this.State = PaymentOrChargePacketState.InProgress;
        }

        /// <summary>
        /// Поставить состояние Ожидание
        /// </summary>
        public virtual void SetInPending()
        {
            this.State = PaymentOrChargePacketState.Pending;
            this.AcceptDate = null;
        }

        /// <summary>
        /// Поставить состояние Подтвержден
        /// </summary>
        public virtual void SetAccepted()
        {
            this.State = PaymentOrChargePacketState.Accepted;
            this.AcceptDate = DateTime.Now;
        }

        /// <summary>
        /// Создать операцию
        /// </summary>
        /// <returns></returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period)
        {
            return new MoneyOperation(this.TransferGuid, period)
            {
                OperationDate = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer
        /// </summary>
        public virtual string TransferGuid { get; protected set; }

        /// <summary>
        /// Костыль, написан для того чтобы смигрировать данные с реестров непотвержденные оплаты НВС
        /// </summary>
        public virtual void SetGuid(string value)
        {
            this.TransferGuid = value;
        }
    }
}
