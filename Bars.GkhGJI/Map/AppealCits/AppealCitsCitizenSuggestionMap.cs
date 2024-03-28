namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг <see cref="AppealCitsCitizenSuggestion"/>
    /// </summary>
    public class AppealCitsCitizenSuggestionMap : BaseEntityMap<AppealCitsCitizenSuggestion>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AppealCitsCitizenSuggestionMap() 
            : base("Bars.GkhGji.Entities.AppealCitsCitizenSuggestion", "GJI_APP_CITS_SUG")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.AppealCits, "Обращение ГЖИ").Column("APPEAL_CITIZENS_ID").NotNull();
            this.Reference(x => x.CitizenSuggestion, "Обращение ЖКХ").Column("CIT_SUG_ID").NotNull();
        }
    }
}