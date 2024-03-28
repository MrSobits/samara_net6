namespace Bars.GkhGji.Contracts.Meta
{
    using Bars.B4.Utils;

    public class Witness
    {
        [Display("ФИО")]
        public string FullName { get; set; }

        [Display("Должность")]
        public string Position { get; set; }

        [Display("С актом ознакомлен")]
        public bool IsFamiliarized { get; set; }
    }
}
