namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип исполнения ГЖИ
    /// </summary>
    public enum TypePrescriptionExecution
    {
        [Display("Проверка исполнения не проводилась")]
        NotSet = 0,

        [Display("Не исполнено")]
        NotExecuted = 10,

        [Display("Исполнено")]
        Executed = 20,

        [Display("Исполнено с нарушением срока")]
        Violated = 30,

        [Display("Исполнено в части")]
        Partially = 40
    }
}