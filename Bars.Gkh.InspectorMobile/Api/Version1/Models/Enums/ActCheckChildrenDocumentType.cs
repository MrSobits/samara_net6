namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums
{
    using Bars.B4.Utils;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Тип дочернего документа ГЖИ "Акт проверки"
    /// </summary>
    /// <remarks>
    /// Часть перечисления <see cref="TypeDocumentGji"/>. Нужен для того, чтобы в сваггере не отображались лишние значения
    /// </remarks>
    public enum ActCheckChildrenDocumentType
    {
        /// <summary>
        /// Предписание
        /// </summary>
        [Display("Предписание")]
        Prescription = 50,

        /// <summary>
        /// Протокол
        /// </summary>
        [Display("Протокол")]
        Protocol = 60,
    }
}