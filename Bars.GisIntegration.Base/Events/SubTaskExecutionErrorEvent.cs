namespace Bars.GisIntegration.Base.Events
{
    using Bars.B4.Events;
    using Bars.GisIntegration.Base.Events.Arguments;

    /// <summary>
    /// Событие ошибки выполнения подзадачи
    /// </summary>
    public class SubTaskExecutionErrorEvent : Event<SubTaskExecutionErrorEventArgs>
    {
    }
}
