namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус ознакомления с результатами проверки
    /// </summary>
    public enum AppealOperationType
    {
        /// <summary>
        /// Создание
        /// </summary>
        [Display("Создание")]
        AppealCreate = 10,

        /// <summary>
        /// Редактирование
        /// </summary>
        [Display("Редактирование")]
        AppealEdit = 20,

        /// <summary>
        /// Удаление
        /// </summary>
        [Display("Удаление")]
        AppealDelete = 30
    }
}