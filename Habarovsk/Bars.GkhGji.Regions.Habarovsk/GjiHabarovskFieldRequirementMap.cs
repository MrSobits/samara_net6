namespace Bars.GkhGji.Regions.Habarovsk
{
    using Bars.Gkh.DomainService;

    public class GjiHabarovskFieldRequirementMap : FieldRequirementMap
    {
        public GjiHabarovskFieldRequirementMap()
        {
            this.Namespace("Gkh.Suggestion.CitizenSuggestion", "Обращения граждан");
            this.Namespace("Gkh.Suggestion.CitizenSuggestion.Field", "Поля");
            this.Requirement("Gkh.Suggestion.CitizenSuggestion.Field.Executor", "Исполнитель");
        }
    }
}
