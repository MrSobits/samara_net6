namespace Bars.Gkh.Gis.Entities.Register.ServiceRecalculationRegister
{
    using System;
    using B4.DataAccess;
    using HouseServiceRegister;

    /// <summary>
    /// Дом
    /// </summary>
    [Serializable]
    public class ServiceRecalculationRegister : BaseEntity
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual HouseServiceRegister Service { get; set; }
        /// <summary>
        /// Месяц перерасчета
        /// </summary>
        public virtual DateTime RecalculationMonth { get; set; }

        /// <summary>
        /// Сумма перерасчета
        /// </summary>
        public virtual decimal RecalculationSum { get; set; }

        /// <summary>
        /// В том числе НДС(налоги)
        /// </summary>
        public virtual decimal RecalculationNds { get; set; }

        /// <summary>
        /// Объем услуги по перерасчету
        /// </summary>
        public virtual decimal RecalculationVolume { get; set; }
    }
}
