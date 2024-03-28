namespace Bars.Gkh.Overhaul.Hmao.Import.ImportPublishYear
{
    /// <summary>
    /// Сведения о сроках проведения капитального ремонта в версии программы
    /// </summary>
    public class ImportPublishYearObject
    {
        /// <summary>
        /// Идентификатор строки
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Код работы
        /// </summary>
        public string WorkCode { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Адрес дома в программе
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// ООИ
        /// </summary>
        public string CommonEstateObject { get; set; }

        /// <summary>
        /// Конструктивные элементы
        /// </summary>
        public string ConstructionElement { get; set; }

        /// <summary>
        /// Год публикации программы
        /// </summary>
        public int YearPublished { get; set; }

        /// <summary>
        /// Измененный год
        /// </summary>
        public int? NewYear { get; set; }
    }
}
