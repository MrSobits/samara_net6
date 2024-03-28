namespace Bars.Gkh.MasterApp
{
    using System.Reflection;

    using Bars.B4.Application;
    using Bars.B4.Modules.Console;
    using Bars.B4.Modules.Console.WindowsService;
    using Bars.B4.Modules.Tasks.MasterApp.App;
    using Bars.B4.Modules.Tasks.MasterApp.Executor;

    /// <summary>
    /// Переопределение контекста для логгирования падения сервера расчётов
    /// </summary>
    internal class GkhMasterAppContext : MasterAppContext
    {
        /// <inheritdoc />
        protected override void StartContext()
        {
            base.StartContext();

            var processField = typeof(MasterAppContext).GetField("_process", BindingFlags.Instance | BindingFlags.NonPublic);
            if (processField != null)
            {
                var process = (ExecutorProcess)processField.GetValue(this);
                process.OnExit += x => AppContext.Log.Error(process.Exception, $"Процесс завершен, код ошибки: {x}");
            }
        }
    }

    /// <summary>
    /// Приложение
    /// </summary>
    internal class GkhApplication : B4Application<GkhMasterAppContext>
    {
    }

    /// <summary>
    /// Служба мастера
    /// </summary>
    internal class GkhWindowsService : B4ServiceBase<GkhApplication, GkhMasterAppContext>
    {
    }
}