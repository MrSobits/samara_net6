namespace Bars.Gkh.Entities.Hcs
{
    using System;
    using Bars.Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Показания общедомовых приборов учета
    /// </summary>
    public class HouseMeterReading: BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

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
        /// Расход по нежилым помещениям
        /// </summary>
        public virtual decimal NonLivingExpense { get; set; }

        /// <summary>
        /// Составной ключ вида RealityObject.CodeErc#MeterSerial#Service, формируется в интерцепторе на Create и Update
        /// Не отображать в клиентской части
        /// </summary>
        [JsonIgnore]
        public virtual string CompositeKey { get; set; }
    }
}
