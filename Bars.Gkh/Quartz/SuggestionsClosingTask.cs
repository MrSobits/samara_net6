namespace Bars.Gkh.Quartz
{
    using B4.IoC;
    using Bars.B4.Application;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.Suggestions;

    /// <summary>
    /// Таск для закрытия обращений граждан, которые были отправлены на портал и 
    /// по ним не было ответа продолжительное время (по умолчанию 10 дней, конфигурируется в AppSettings).
    /// </summary>
    public class SuggestionsClosingTask : BaseTask
    {
        public override void Execute(DynamicDictionary @params)
        {
            var waitDays = ApplicationContext.Current.Configuration.AppSettings.GetAs("PortalResponseWaitDays", () => 10);
            Container.UsingForResolved<IExpiredSuggestionCloser>((container, service) => service.Close(waitDays));
        }

    }
}
