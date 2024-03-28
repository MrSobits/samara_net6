namespace Bars.GkhDi.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Тарифы для РСО (ресурсоснабжающей организации)
    /// </summary>
    public class TariffForRso : BaseGkhEntity
    {
        /// <summary>
        /// Базовая услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
       
        /// <summary>
        /// Номер нормативно правового акта
        /// </summary>
        public virtual string NumberNormativeLegalAct { get; set; }

        /// <summary>
        /// Дата нормативно правового акта
        /// </summary>
        public virtual DateTime? DateNormativeLegalAct { get; set; }

        /// <summary>
        /// Орган, установивший тариф
        /// </summary>
        public virtual string OrganizationSetTariff { get; set; }

        /// <summary>
        /// Стоимость тарифа
        /// </summary>
        public virtual decimal? Cost { get; set; }

        /// <summary>
        /// Стоимость ночного тарифа
        /// </summary>
        public virtual decimal? CostNight { get; set; }
    }
}