namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    /// <summary>
    /// Маппинг для "История созданных документов на оплату. Сущность нужна только для нумерации документов"
    /// </summary>
    public class PaymentDocumentLogMap : BaseEntityMap<PaymentDocumentLog>
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public PaymentDocumentLogMap() 
            :base("История сформированных документов на оплату (Количество лицевых счетов)", "REGOP_PAYMENT_DOC_LOG")
        {
        }
        
        /// <summary>
        /// Мапинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Parent, "Родительская запись").Column("PARENT_ID");
            this.Reference(x => x.Period, "Период, за который печатаются квитанции").Column("PERIOD_ID").Fetch();
            this.Property(x => x.Uid, "Уникальный идентификатор группы документов на оплату").Column("UID").NotNull();
            this.Property(x => x.Description, "Путь-группировка документов").Column("DESCRIPTION");
            this.Property(x => x.StartTime, "Время начала выполнения").Column("START_TIME");
            this.Property(x => x.Count, "Количество обработанных лицевых счетов").Column("ACC_COUNT").NotNull();
            this.Property(x => x.AllCount, "Количество обработанных лицевых счетов").Column("ALL_ACC_COUNT").NotNull();
        }
    }
}
