namespace Bars.GkhGji.Regions.Tyumen.StateChange
{
    using B4.Modules.States;

    using Gkh.Entities.Suggestion;

    using Utils.EntityExceptions;

    public class CitizenSuggestionStateChangeHandler : IStateChangeHandler
    {
        public void OnStateChange(IStatefulEntity entity, State oldState, State newState)
        {
            if (entity.State.TypeId != "gkh_citizen_suggestion")
            {
                return;
            }
            
            var suggestion = entity as CitizenSuggestion;

            if (newState.Code.ToLower() != "check")
            {
               suggestion.SendApplicantNotification(newState);
            }
        }
    }
}