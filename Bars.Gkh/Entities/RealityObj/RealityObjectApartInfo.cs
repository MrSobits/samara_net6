namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Enums;
    
    /// <summary>
    /// Сведения по квартирам жилого дома
    /// </summary>
    public class RealityObjectApartInfo : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// № квартиры
        /// </summary>
        public virtual string NumApartment { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public virtual decimal? AreaLiving { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal? AreaTotal { get; set; }

        /// <summary>
        /// Количество жителей
        /// </summary>
        public virtual int? CountPeople { get; set; }

        /// <summary>
        // Приватизирована
        /// </summary>
        public virtual YesNoNotSet Privatized { get; set; }

        /// <summary>
        // Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        // ФИО собственника
        /// </summary>
        public virtual string FioOwner { get; set; }
    }
}
