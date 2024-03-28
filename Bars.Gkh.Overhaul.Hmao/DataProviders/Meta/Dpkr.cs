namespace Bars.Gkh.Overhaul.Hmao.DataProviders.Meta
{
    using Bars.B4.Utils;

    public class ДПКР
    {
        [Display("МО")]
        public string МО { get; set; }

        [Display("Населенный пункт")]
        public string НаселённыйПункт { get; set; }

        [Display("Адрес")]
        public string Адрес { get; set; }

        [Display("Площадь дома")]
        public decimal ПлощадьДома { get; set; }

        [Display("ООИ")]
        public string ООИ { get; set; }

        [Display("Площадь")]
        public decimal Площадь { get; set; }

        [Display("Год ремонта")]
        public int ГодРемонта { get; set; }

        [Display("Стоимость")]
        public decimal Стоимость { get; set; }

    }
}
