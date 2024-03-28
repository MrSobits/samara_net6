namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа для выгрузки в ЕРКНМ
    /// </summary>
    public enum ERKNMDocumentType
    {
        /// <summary>
        /// Решение
        /// </summary>
        [Display("Любой")]
        NotSet = 0,

        /// <summary>
        /// Решение
        /// </summary>
        [Display("Решение")]
        Decision = 10,

        /// <summary>
        /// Предостережение
        /// </summary>
        [Display("Предостережение")]
        Admonition = 20,

        /// <summary>
        /// Профвизит
        /// </summary>
        [Display("Профвизит")]
        PreventiveAct = 30

    }
}