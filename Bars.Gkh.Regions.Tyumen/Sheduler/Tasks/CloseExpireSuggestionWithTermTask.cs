namespace Bars.Gkh.Regions.Tyumen.Sheduler.Tasks
{
    using B4.IoC;
    using B4.Modules.Quartz;
    using B4.Utils;
    using DomainServices.Suggestions;

    public class CloseExpireSuggestionWithTermTask : BaseTask
    {
        public override void Execute(DynamicDictionary @params)
        {
            Container.UsingForResolved<IExpiredSuggestionWithTermCloser>((container, service) => service.Close());
        }
    }
}