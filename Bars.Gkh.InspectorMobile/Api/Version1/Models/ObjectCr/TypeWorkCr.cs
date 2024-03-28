namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr
{
    /// <summary>
    /// Работы капитального ремонта объекта КР
    /// </summary>
    public class TypeWorkCr
    {
        /// <summary>
        /// Наименование работы
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// Год выполнения работы
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Объем работ
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public string Measure { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// Признак наличия ПСД
        /// </summary>
        public bool? Psd { get; set; }
    }
}