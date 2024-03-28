namespace Bars.GkhGji.Contracts.Meta
{
    using Bars.B4.Utils;

    public class InspectionProgress
    {
        [Display("Выявлено")]
        public bool IsFound { get; set; }

        [Display("Наименование")]
        public string Name { get; set; }
    }
}
