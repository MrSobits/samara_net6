namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// наименование файла документа ПИР
    /// </summary>
    public enum DocumentClwFileName
    {
        /// <summary>
        /// Без адреса
        /// </summary>
        [Display("Без адреса")]
        WithoutAddress = 0,

        /// <summary>
        /// С адресом
        /// </summary>
        [Display("С адресом")]
        WithAddress = 1
    }
}