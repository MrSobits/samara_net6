namespace Bars.GisIntegration.Base.Entities.External.Housing.House
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;

    /// <summary>
    /// Подъезд
    /// </summary>
    public class Porch : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// Дом
        /// </summary>
        public virtual House House { get; set; }
        /// <summary>
        /// ГУИД подъезда
        /// </summary>
        public virtual string PorchGuid { get; set; }
        /// <summary>
        /// Номер подъезда
        /// </summary>
        public virtual int PorchNumber { get; set; }
        /// <summary>
        /// Этажность 
        /// </summary>
        public virtual int Floor { get; set; }
        /// <summary>
        /// Дата постройки
        /// </summary>
        public virtual DateTime? BuildDate { get; set; }
        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
        public virtual long Count { get; set; }
    }
}
