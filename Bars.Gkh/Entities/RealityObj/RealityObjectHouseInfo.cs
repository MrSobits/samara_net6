namespace Bars.Gkh.Entities
{
    using System;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Сведения о помещениях жилого дома
    /// </summary>
    public class RealityObjectHouseInfo : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// № объекта
        /// </summary>
        public virtual string NumObject { get; set; }

        /// <summary>
        /// № зарегистрированного права / ограничения
        /// </summary>
        public virtual string NumRegistrationOwner { get; set; }

        /// <summary>
        /// Вид права
        /// </summary>
        public virtual KindRightToObject KindRight { get; set; }

        /// <summary>
        /// Дата регистрации права
        /// </summary>
        public virtual DateTime? DateRegistration { get; set; }

        /// <summary>
        /// Дата регистрации (рождения) правообладателя
        /// </summary>
        public virtual DateTime? DateRegistrationOwner { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public virtual decimal? TotalArea { get; set; }

        /// <summary>
        /// Правообладатель
        /// </summary>
        public virtual string Owner { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }
    }
}
