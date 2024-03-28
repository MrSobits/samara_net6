namespace Bars.GisIntegration.Base.Entities.Infrastructure
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Коммунальная услуга
    /// </summary>
    public class RisRkiCommunalService : BaseRisEntity
    {
        /// <summary>
        /// ОКИ
        /// </summary>
        public virtual RisRkiItem RkiItem { get; set; }

        /// <summary>
        /// Код услуги
        /// </summary>
        public virtual string ServiceCode { get; set; }

        /// <summary>
        /// ГУИД услуги
        /// </summary>
        public virtual string ServiceGuid { get; set; }

        /// <summary>
        /// Наименование услуги
        /// </summary>
        public virtual string ServiceName { get; set; }
    }
}
