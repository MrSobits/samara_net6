namespace Bars.Gkh.Domain.ParameterVersioning.Proxy
{
    public class EntityLogRecordUsed : EntityLogRecord
    {
        /// <summary>
        /// Конструктор на основе EntityLogRecord
        /// </summary>
        /// <param name="entityLogRecord"></param>
        public EntityLogRecordUsed(EntityLogRecord entityLogRecord)
        {
            this.DateActualChange = entityLogRecord.DateActualChange;
            this.DateApplied = entityLogRecord.DateApplied;
            this.EntityId = entityLogRecord.EntityId;
            this.LogId = entityLogRecord.LogId;
            this.ObjectCreateDate = entityLogRecord.ObjectCreateDate;
            this.ParameterName = entityLogRecord.ParameterName;
            this.PropertyValue = entityLogRecord.PropertyValue;
        }

        /// <summary>
        /// Конструктор на основе entityLogRecordUsed
        /// </summary>
        /// <param name="entityLogRecordUsed"></param>
        public EntityLogRecordUsed(EntityLogRecordUsed entityLogRecordUsed)
        {
            this.AccountId = entityLogRecordUsed.AccountId;
            this.DateActualChange = entityLogRecordUsed.DateActualChange;
            this.DateApplied = entityLogRecordUsed.DateApplied;
            this.EntityId = entityLogRecordUsed.EntityId;
            this.LogId = entityLogRecordUsed.LogId;
            this.ObjectCreateDate = entityLogRecordUsed.ObjectCreateDate;
            this.ParameterName = entityLogRecordUsed.ParameterName;
            this.PropertyValue = entityLogRecordUsed.PropertyValue;
        }


        public long AccountId { get; set; }
    }
}