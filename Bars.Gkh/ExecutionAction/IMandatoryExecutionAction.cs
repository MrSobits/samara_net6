namespace Bars.Gkh.ExecutionAction
{
    /// <summary>
    /// Обязательное выполняемое действие
    /// </summary>
    public interface IMandatoryExecutionAction : IExecutionAction
    {
        /// <summary>
        /// Проверка необходимости выполнения действия
        /// </summary>
        bool IsNeedAction();
    }
}