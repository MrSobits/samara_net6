namespace Bars.Gkh.Entities.Administration.ExecutionAction
{
    using Bars.B4;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Задача для выполнения действия
    /// </summary>
    public class ExecutionActionTask : BaseSchedulableTask
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        public virtual string ActionCode { get; set; }

        /// <summary>
        /// Параметры запуска задачи
        /// </summary>
        public virtual BaseParams BaseParams { get; set; }

        /// <summary>
        /// Признак удаления задачи
        /// </summary>
        public virtual bool IsDelete { get; set; }
    }
}