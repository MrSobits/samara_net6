namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    /// Операция изменения ЛС
    /// </summary>
    public class PersonalAccountChange : BaseImportableEntity
    {
        public PersonalAccountChange()
        {

        }

        public PersonalAccountChange(
            BasePersonalAccount account,
            string description,
            PersonalAccountChangeType changeType,
            DateTime operationDate,
            DateTime? actualFrom,
            string operatorName,
            string newValue,
            string oldValue,
            ChargePeriod period)
        {
            this.PersonalAccount = account;
            this.Description = description;
            this.ChangeType = changeType;
            this.Date = operationDate;
            this.ActualFrom = actualFrom;
            this.Operator = operatorName;

            this.NewValue = newValue;
            this.OldValue = oldValue;
            this.ChargePeriod = period;
        }

        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Вид изменения
        /// </summary>
        public virtual PersonalAccountChangeType ChangeType { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Дата актуальности
        /// </summary>
        public virtual DateTime? ActualFrom { get; set; }

        /// <summary>
        /// Оператор
        /// </summary>
        public virtual string Operator { get; set; }

        /// <summary>
        /// Старое значение
        /// </summary>
        public virtual string OldValue { get; set; }

        /// <summary>
        /// Новое значение
        /// </summary>
        public virtual string NewValue { get; set; }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Причина изменения
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Период расчета
        /// </summary>
        public virtual ChargePeriod ChargePeriod { get; set; }

    }
}