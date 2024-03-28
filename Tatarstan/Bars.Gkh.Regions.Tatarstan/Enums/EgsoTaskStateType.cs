namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус задачи по интеграции с ЕГСО ОВ
    /// </summary>
    public enum EgsoTaskStateType
    {
        /// <summary>
        /// Ожидает выполнения
        /// </summary>
        [Display("Ожидает выполнения")]
        Pending = 10,
        
        /// <summary>
        /// Выполняется
        /// </summary>
        [Display("Выполняется")]
        InProgress = 20,
        
        /// <summary>
        /// Выполнена успешно
        /// </summary>
        [Display("Выполнена успешно")]
        Completed = 30,
        
        /// <summary>
        /// Выпролнена с ошибкой
        /// </summary>
        [Display("Выполнена с ошибкой")]
        CompletedWithErrors = 40,
        
        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 50, 
        
        /// <summary>
        /// Не определен
        /// </summary>
        [Display("Не определен")]
        Undefined = 60
    }
}