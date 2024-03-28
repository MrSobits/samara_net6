namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using Bars.B4.Utils;

    public class Attachment
    {
        [Display("Наименование")]
        public string Name { get; set; }

        [Display("Дата докумета")]
        public DateTime DocDate { get; set; }

        [Display("Описание")]
        public string Description { get; set; }
    }
}
