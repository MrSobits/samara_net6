namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    /// <summary>
    /// Информация о выполняемом действии
    /// </summary>
    public class ExecutionActionInfo
    {
        /// <summary>
        /// Скрытое
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}