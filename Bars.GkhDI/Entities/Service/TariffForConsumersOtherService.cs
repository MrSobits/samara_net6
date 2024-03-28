namespace Bars.GkhDi.Entities.Service
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    public class TariffForConsumersOtherService : BaseGkhEntity
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual OtherService OtherService { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Тариф установлен для
        /// </summary>
        public virtual TariffIsSetForDi TariffIsSetFor { get; set; }

        /// <summary>
        /// Орган, установивший тариф
        /// </summary>
        public virtual string OrganizationSetTariff { get; set; }

        /// <summary>
        /// Тип организации установившей тариф
        /// </summary>
        public virtual TypeOrganSetTariffDi TypeOrganSetTariffDi { get; set; }

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
