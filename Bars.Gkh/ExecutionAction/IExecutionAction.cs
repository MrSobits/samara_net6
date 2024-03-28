namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Threading;

    using B4;

    using Bars.B4.Modules.Security;

    /// <summary>
    /// Интерфейс действия для выполнения
    /// </summary>
    public interface IExecutionAction
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Описание действия
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Название для отображения
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Действие
        /// </summary>
        Func<IDataResult> Action { get; }

        /// <summary>
        /// Параметры задачи
        /// </summary>
        BaseParams ExecutionParams { get; set; }

        /// <summary>
        /// Токен отмены
        /// </summary>
        CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Поставить действия в очередь после успешного выполнеиния
        /// </summary>
        void StartAfterSuccessActions();

        /// <summary>
        /// Пользователь запустивший действие
        /// <para>
        /// При автоматическом запуске при старте системы равен null
        /// </para>
        /// </summary>
        User User { get; set; }
    }
}