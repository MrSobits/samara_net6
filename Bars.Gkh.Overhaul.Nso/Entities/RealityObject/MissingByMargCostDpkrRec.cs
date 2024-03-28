namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Отсутствующий по предельной записи
    /// </summary>
    public class MissingByMargCostDpkrRec : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Строка объектов общего имущества
        /// </summary>
        public virtual string CommonEstateObjects { get; set; }

        /// <summary>
        /// Плановая сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Площадь дома, на момент расчета (в зависимости от параметра в настройках может быть:
        ///  площадь МКД, жилая площадь, жилая и нежилая площадь)
        /// </summary>
        public virtual decimal? Area { get; set; }

        /// <summary>
        /// Предельная сумма на кв.м, на момент расчета
        /// </summary>
        public virtual decimal MargSum  { get; set; }

        /// <summary>
        /// Тип дома, на момент расчета
        /// </summary>
        public virtual string RealEstateTypeName { get; set; }
    }
}