namespace Bars.GkhGji.Regions.Nso.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип напоминания
    /// </summary>
    public enum TypeReminder
    {
        [Display("Обращение")]
        Statement = 10,

        [Display("Основание проверки")]
        BaseInspection = 20,

        [Display("Приказ")]
        Disposal = 30,

        [Display("Предписание")]
        Prescription = 40,

        // Нужно только в РТ
        [Display("Постановление")]
        Resolution = 50,

        // Нужно только в НСО
        [Display("Акт проверки")]
        ActCheck = 60,

        // Нужно только в НСО
        [Display("Уведомление о начале проверки")]
        NoticeOfInspection = 70
    }
}
