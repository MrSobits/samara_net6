namespace Bars.GkhCr.Meta
{
    using System;
    using System.Xml.Serialization;

    public class МКД
    {
        [XmlElement("Муниципальное образование")]
        public string Муниципальное_образование { get; set; }

        public string Адрес { get; set; }

        public DateTime Год_ввода_в_эксплуатацию { get; set; }

        public DateTime Год_последнего_кап_ремонта { get; set; }

        public string Материал_стен { get; set; }

        public int Количество_этажей { get; set; }

        public int Количество_подъездов { get; set; }

        public decimal Общая_площадь { get; set; }

        public decimal Площадь_помещений { get; set; }

        public decimal Площадь_помещений_в_том_числе_в_собственности_граждан { get; set; }

        public int Количество_зарегистрированных_жителей { get; set; }

        public string Вид_ремонта { get; set; }

        public decimal Стоимость_итого { get; set; }

        public decimal Стоимость_за_счет_средств_Фонда { get; set; }

        public decimal Стоимость_за_счет_средств_бюджета_субъекта { get; set; }

        public decimal Стоимость_за_счет_средств_местного_бюджета { get; set; }

        public decimal Стоимость_за_счет_средств_ТСЖ_и_собственников_помещений { get; set; }

        public decimal Предельная_стоимость_1_кв_м { get; set; }

        public decimal Удельная_стоимость_1_кв_м { get; set; }

        public DateTime Плановая_дата_завершения_работ { get; set; }
    }
}
