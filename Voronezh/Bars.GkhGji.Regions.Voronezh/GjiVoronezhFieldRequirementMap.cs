namespace Bars.GkhGji.Regions.Voronezh
{
    using Bars.Gkh.DomainService;

    public class GjiVoronezhFieldRequirementMap : FieldRequirementMap
    {
        public GjiVoronezhFieldRequirementMap()
        {
            this.Namespace("Gkh.Suggestion.CitizenSuggestion", "Обращения граждан");
            this.Namespace("Gkh.Suggestion.CitizenSuggestion.Field", "Поля");
            this.Requirement("Gkh.Suggestion.CitizenSuggestion.Field.Executor", "Исполнитель");
        }
    }
}
