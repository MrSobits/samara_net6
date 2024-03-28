namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сведения по блокам жилого дома
    /// </summary>
    public class RealityObjectBlock : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// № блока
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public virtual decimal? AreaLiving { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal? AreaTotal { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastralNumber { get; set; }
    }
}
