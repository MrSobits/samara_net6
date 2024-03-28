namespace Bars.GkhRf.Entities
{
    using B4.DataAccess;

    /*
         * Данная вьюха предназначена для реестра Оплаты 
         * с агрегированными показателями из реестра Оплата КР:
         * Начислено населению(4 поля по типам оплат), 
         * Оплачено населением(4 поля по типам оплат), 
         */
    public class ViewPayment : PersistentObject
    {
        /// <summary>
        /// Начислено населению, тип оплаты КР
        /// </summary>
        public virtual decimal? ChargePopulationCr { get; set; }

        /// <summary>
        /// Оплачено населением, тип оплаты Кап. Рем.
        /// </summary>
        public virtual decimal? PaidPopulationCr { get; set; }

        /// <summary>
        /// Начислено населению, тип оплаты найм Рег.Фонда
        /// </summary>
        public virtual decimal? ChargePopulationHireRf { get; set; }

        /// <summary>
        /// Оплачено населением, тип оплаты найм Рег.Фонда
        /// </summary>
        public virtual decimal? PaidPopulationHireRf { get; set; }

        /// <summary>
        /// Начислено населению, тип оплаты Кап. Ремонт по 185 ФЗ
        /// </summary>
        public virtual decimal? ChargePopulationCr185 { get; set; }

        /// <summary>
        /// Оплачено населением, тип оплаты Кап. Ремонт по 185 ФЗ
        /// </summary>
        public virtual decimal? PaidPopulationCr185 { get; set; }

        /// <summary>
        /// Начислено населению, тип оплаты ремонт жилого здания
        /// </summary>
        public virtual decimal? ChargePopulationBldRepair { get; set; }

        /// <summary>
        /// Оплачено населением, тип оплаты  ремонт жилого здания
        /// </summary>
        public virtual decimal? PaidPopulationBldRepair { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Наименование муниципального образования
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Адрес жилого дома
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Идентификатор жилого дома
        /// </summary>
        public virtual long? RealityObjectId { get; set; }
    }
}