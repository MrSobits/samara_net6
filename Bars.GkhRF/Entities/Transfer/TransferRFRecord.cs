namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Запись перечисление средств рег. фонда
    /// </summary>
    public class TransferRfRecord : BaseDocument, IStatefulEntity
    {
        /// <summary>
        /// Перечисление средств рег. фонда
        /// </summary>
        public virtual TransferRf TransferRf { get; set; }

        /// <summary>
        /// Дата перечисления
        /// </summary>
        public virtual DateTime? TransferDate { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Признак расчета
        /// </summary>
        public virtual bool IsCalculation { get; set; }


        /// <summary>
        /// Признак, обозначающий, что выполняется расчет, но не более 1 часа 
        /// </summary>
        [JsonIgnore]
        public virtual bool IsCalculating
        {
            get { return this.IsCalculation && (this.ObjectEditDate > DateTime.Now.AddHours(-1)); }
        }
    }
}
