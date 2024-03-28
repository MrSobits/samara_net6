namespace Bars.GkhGji.Regions.Smolensk.Enums
{
    using B4.Utils;

    public enum TypePrescriptionCancel
    {
        /// <summary>
        /// Решение о продлении срока исполнения предписания
        /// </summary>
        [Display("Решение о продлении срока исполнения предписания")]
        ExecutionProlongation = 10,

        /// <summary>
        /// Решение об отмене предписания
        /// </summary>
        [Display("Решение об отмене предписания")]
        Cancel = 20
    }
}