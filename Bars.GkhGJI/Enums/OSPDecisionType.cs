namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы вопросов
    /// </summary>
    public enum OSPDecisionType
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Истечение срока давности ИД")]
        TimeLapsed = 10,

        [Display("Фактическое исполнение исполнительного документа")]
        Execution = 20,

        [Display("Отмена судебного акта, на основании которого выдан исполнительный документ")]
        Cancel = 30,

        [Display("Банкротство")]
        Bankrot = 40
    }
}
