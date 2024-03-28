namespace Bars.Gkh.Overhaul.Domain
{
    using Bars.Gkh.Entities;


    /// <summary>
    /// Отсутствующий по предельной записи
    /// (Сущность является нехранимой версией Bars.Gkh.Overhaul.Hmao.Entities.MissingByMargCostDpkrRec)
    /// </summary>
    public class MaxCostExeededRealty
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый Год
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Строка объектов общего имущества
        /// </summary>
        public string CommonEstateObjects { get; set; }

        /// <summary>
        /// Плановая сумма
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Площадь дома, на момент расчета (в зависимости от параметра в настройках может быть:
        ///  площадь МКД, жилая площадь, жилая и нежилая площадь)
        /// </summary>
        public decimal? Area { get; set; }

        /// <summary>
        /// Предельная сумма на кв.м, на момент расчета
        /// </summary>
        public decimal MaxSum { get; set; }

        /// <summary>
        /// Тип дома, на момент расчета
        /// </summary>
        public string RealEstateTypeName { get; set; }
    }
}
