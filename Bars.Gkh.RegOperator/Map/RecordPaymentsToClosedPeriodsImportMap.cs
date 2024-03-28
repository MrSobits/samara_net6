namespace Bars.Gkh.RegOperator.Map
{
    using B4.Modules.Mapping.Mappers;
    using Entities;

    public class RecordPaymentsToClosedPeriodsImportMap : BaseEntityMap<RecordPaymentsToClosedPeriodsImport>
    {
        public RecordPaymentsToClosedPeriodsImportMap(): 
                base("Запись о импорте оплаты в закрытый период", "PAYMENTS_TO_CLOSED_PERIODS_IMPORT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Period, "Период ").Column("PERIOD");
            this.Property(x => x.Source, "Источник поступления ").Column("SOURCE");
            this.Property(x => x.DocumentNum, "Номер документа/реестра ").Column("DOCUMENT_NUM");
            this.Property(x => x.DocumentDate, "Дата документа/реестра ").Column("DOCUMENT_DATE");
            this.Property(x => x.OperationDate, "Дата операции ").Column("OPERATION_DATE");
            this.Property(x => x.PaymentAgentName, "Наименование платежного агента ").Column("PAYMENT_AGENT_NAME");
            this.Property(x => x.PaymentNumberUs, "Номер платежа в Системе ПА ").Column("PAYMENT_NUMBER_US");
            this.Property(x => x.TransferGuid, "Гуид").Column("TRANSFER_GUID");
            this.Reference(x => x.PaymentOperation, "Базовая сущность операции оплат").Column("PAYMENT_OP_ID").Fetch();
        }
    }
}