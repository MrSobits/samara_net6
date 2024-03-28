namespace Bars.GisIntegration.Base.Events
{
    using Bars.B4.Events;
    using Bars.GisIntegration.Base.Events.Arguments;

    /// <summary>
    /// Событие начала выполнения подзадачи
    /// </summary>
    public class SubTaskStartExecutionEvent : Event<SubTaskEventArgs>
    {
    }
}
