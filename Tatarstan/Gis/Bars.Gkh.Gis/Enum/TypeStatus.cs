namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Тип результата
    /// </summary>
    public enum TypeStatus
    {
        [Display("Ставится в очередь")]
        PreQueuing = 4,

        [Display("В очереди")]
        Queuing = 5,

        [Display("Выполняется")]
        InProgress = 10,

        [Display("Завершено")]
        Done = 20,

        [Display("Завершено, но возможна ошибка в данных")]
        AlmostDone = 21,

        [Display("Ошибка")]
        Error = 30,

        [Display("Ошибка постановки в очередь")]
        QueuingError = 40,

        [Display("Ошибка при обработке")]
        ProcessingError = 50,

        [Display("Данные удалены")]
        Deleted = 60,

        [Display("Проверка загруженных данных")]
        Checking = 70,

        [Display("Нет данных для выгрузки")]
        NoData = 80
    } 
}