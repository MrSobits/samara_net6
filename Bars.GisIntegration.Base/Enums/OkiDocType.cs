namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа ОКИ
    /// </summary>
    public enum OkiDocType
    {
        [Display("Основание управления объектом")]
        ManagBase = 1,
        [Display("Документы, подтверждающие соответствие требованиям энергоэффективности")]
        Documents = 2
    }
}
