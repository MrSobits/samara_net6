namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Suggestion;

    /// <summary>
    /// Сущность связи обращений ЖКХ и ГЖИ
    /// </summary>
    public class AppealCitsCitizenSuggestion : BaseGkhEntity
    {
        /// <summary>
        /// Обращение ГЖИ
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Обращение ЖКХ
        /// </summary>
        public virtual CitizenSuggestion CitizenSuggestion { get; set; }
    }
}