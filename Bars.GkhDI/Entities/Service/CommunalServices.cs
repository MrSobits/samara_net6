namespace Bars.GkhDi.Entities
{
    using System.Collections;

    using Bars.GkhDi.Attributes;
    using Enums;
    using Gkh.Entities;

    /// <summary>
    /// Коммунальная услуга
    /// </summary>
    public class CommunalService : BaseService
    {
        /// <summary>
        /// Тип оказания услуги
        /// </summary>
        public virtual TypeOfProvisionServiceDi TypeOfProvisionService { get; set; }

        /// <summary>
        /// Объем закупаемых ресурсов
        /// </summary>
        public virtual decimal? VolumePurchasedResources { get; set; }

        /// <summary>
        /// Цена закупаемых ресурсов
        /// </summary>
        public virtual decimal? PricePurchasedResources { get; set; }

        /// <summary>
        /// Вид электроснабжения
        /// </summary>
        [OptionField("Вид электроснабжения")]
        public virtual KindElectricitySupplyDi KindElectricitySupply { get; set; }
        
        /// <summary>
        /// Норматив потребления коммунальной услуги в жилых помещениях.
        /// </summary>
        public virtual decimal? ConsumptionNormLivingHouse { get; set; }        

        /// <summary>
        /// Единица изменения норматива потребления коммунальной услуги в жилых помещениях.
        /// </summary>
        public virtual UnitMeasure UnitMeasureLivingHouse { get; set; }
        /// <summary>
        /// Дополнительно по нормативу потребления коммунальной услуги в жилых помещениях.
        /// </summary>
        public virtual string AdditionalInfoLivingHouse { get; set; }

        /// <summary>
        /// Норматив потребления коммунальной услуги на общедомовые нужды.
        /// </summary>
        public virtual string ConsumptionNormHousing { get; set; }
        /// <summary>
        /// Единица изменения норматива потребления коммунальной услуги на общедомовые нужды.
        /// </summary>
        public virtual UnitMeasure UnitMeasureHousing { get; set; }
        /// <summary>
        /// Дополнительно по нормативу потребления коммунальной услуги на общедомовые нужды.
        /// </summary>
        public virtual string AdditionalInfoHousing { get; set; }
    }
}