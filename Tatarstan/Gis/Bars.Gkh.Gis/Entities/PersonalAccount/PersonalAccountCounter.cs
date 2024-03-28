namespace Bars.Gkh.Gis.Entities.PersonalAccount
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Прибор учета
    /// </summary>
    public class PersonalAccountCounter : PersistentObject
    {
        /// <summary>
        /// Идентификатор квартиры
        /// </summary>
        public virtual long ApartmentId { get; set; }
        
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual string Service { get; set; }

        /// <summary>
        /// Номер ПУ
        /// </summary>
        public virtual string CounterNumber { get; set; }

        /// <summary>
        /// Тип ПУ (марка ПУ)
        /// </summary>
        public virtual string CounterType { get; set; }

        /// <summary>
        /// Внесение показаний осуществляется в ручном режиме
        /// </summary>
        public virtual bool ManualModeMetering { get; set; }

        /// <summary>
        /// Дата снятия показания
        /// </summary>
        public virtual DateTime? ReadoutDate { get; set; }

        /// <summary>
        /// Дата учета
        /// </summary>
        public virtual DateTime StatementDate { get; set; }

        /// <summary>
        /// Предыдущая дата учета
        /// </summary>
        public virtual DateTime PrevStatementDate { get; set; }

        /// <summary>
        /// Показание
        /// </summary>
        public virtual decimal CounterValue { get; set; }

        /// <summary>
        /// Предыдущее показание
        /// </summary>
        public virtual decimal PrevCounterValue { get; set; }
    }
}
