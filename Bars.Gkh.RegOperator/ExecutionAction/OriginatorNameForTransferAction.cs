namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Tasks.CorrectTransfers.OrigName;

    /// <summary>
    /// Действие проставления OriginatorName трансферам
    /// </summary>
    public class OriginatorNameForTransferAction : BaseExecutionAction
    {
        /// <summary>
        /// Логи
        /// </summary>
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Таск менеджер
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Код для регистрации
        /// </summary>
        public string Code => this.GetType().Name;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Проставление Плательщик/Получатель/Основание трансферам";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Проставление Плательщик/Получатель/Основание трансферам";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            try
            {
                var result = this.TaskManager.CreateTasks(new CorrectOriginatorNameTaskProvider(), new BaseParams());

                return new BaseDataResult
                {
                    Success = result.Success,
                    Data = result.Data,
                    Message = "Задача успешно поставлена в очередь на обработку.<br>Информацию по задаче можно увидеть в разделе 'Задачи'"
                };
            }
            catch (Exception e)
            {
                this.LogManager.LogError(e, string.Format("ExecutionAction: {0}\r\nError: {1}\r\nStackTrace: {2}", this.Code, e.Message, e.StackTrace));
                return BaseDataResult.Error(e.Message);
            }
        }
    }
}