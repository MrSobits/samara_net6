namespace Bars.GisIntegration.Base.Entities.Bills
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Период домов 
    /// </summary>
    public class HomePeriod : BaseRisEntity
    {
        /// <summary>
        ///  Адрес ФИАС
        /// </summary>
        public virtual string FIASHouseGuid { get; set; }

        /// <summary>
        /// Закрыть для полномочия "УО"
        /// </summary>
        public virtual bool isUO { get; set; }
    }
}