namespace Bars.GkhGji.Contracts.Meta
{
    using Bars.B4.Utils;

    public class Inspector
    {
        [Display("Код")]
        public string Code { get; set; }

        [Display("ФИО")]
        public string FullName { get; set; }

        [Display("ФамилияИО")]
        public string ShortName { get; set; }

        [Display("ФИО (творительный падеж)")]
        public string FullName_ТворПадеж { get; set; }

        [Display("Должность")]
        public string Position { get; set; }
    }
}
