namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using Bars.B4.Utils;

    public class ProvidedDocument
    {
        [Display("Наименование")]
        public string Name { get; set; }
        
        [Display("Дата предоставления")]
        public DateTime ProvidingDate { get; set; }
    }
}
