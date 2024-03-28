namespace Bars.Gkh.Domain.TechPassport
{
    /// <summary>
    /// Модель компонента техпаспорта дома из xml
    /// </summary>
    public class TechPassportComponent
    {
        /// <summary>Код формы</summary>
        public string FormCode { get; set; }

        /// <summary>Код элемента</summary>
        public string CellCode { get; set; }

        /// <summary>Наименование компонента</summary>
        public string Title { get; set; }

        /// <summary>Тип отображаемого элемента в интерфейсе</summary>
        public string Type { get; set; }
    }
}
