namespace Bars.GisIntegration.Base.Events
{
    using Bars.B4.Events;
    using Bars.GisIntegration.Base.Events.Arguments;

    /// <summary>
    /// Событие завершения выполнения подзадачи
    /// </summary>
    public class SubTaskEndExecutionEvent :  Event<SubTaskEventArgs>
    {
    }
}
