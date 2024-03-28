namespace Bars.Gkh.Entities.Hcs
{
    using System;
    using Bars.Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Показания приборов учета (для лицевого счета)
    /// </summary>
    public class MeterReading: BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Лицевой счет дома
        /// </summary>
        public virtual HouseAccount Account { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual string Service { get; set; }

        /// <summary>
        /// Номер Прибора учета
        /// </summary>
        public virtual string MeterSerial { get; set; }

        /// <summary>
        /// Тип Прибора учета
        /// </summary>
        public virtual string MeterType { get; set; }

        /// <summary>
        /// Текущая дата снятия
        /// </summary>
        public virtual DateTime CurrentReadingDate { get; set; }

        /// <summary>
        /// Предыдущая дата снятия
        /// </summary>
        public virtual DateTime PrevReadingDate { get; set; }

        /// <summary>
        /// Текущие показания
        /// </summary>
        public virtual decimal CurrentReading { get; set; }

        /// <summary>
        /// Предыдущие показания
        /// </summary>
        public virtual decimal PrevReading { get; set; }

        /// <summary>
        /// Расход  
        /// </summary>
        public virtual decimal Expense { get; set; }

        /// <summary>
        /// Плановый расход   
        /// </summary>
        public virtual decimal PlannedExpense { get; set; }

        /// <summary>
        /// Составной ключ вида Account.PaymentCode#MeterSerial#Service, формируется в интерцепторе на Create и Update
        /// Не отображать в клиентской части
        /// </summary>
        [JsonIgnore]
        public virtual string CompositeKey { get; set; }
    }
}
