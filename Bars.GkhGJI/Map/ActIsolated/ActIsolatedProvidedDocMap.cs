namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для "Предоставленные документы акта без взаимодействия"
    /// </summary>
    public class ActIsolatedProvidedDocMap : BaseEntityMap<ActIsolatedProvidedDoc>
    {
        
        public ActIsolatedProvidedDocMap() : 
                base("Предоставленные документы акта без взаимодействия", "GJI_ACTISOLATED_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DateProvided, "Дата предосталвения").Column("DATE_PROVIDED");
            this.Reference(x => x.ActIsolated, "Акт без взаимодействия").Column("ACTISOLATED_ID").NotNull().Fetch();
            this.Reference(x => x.ProvidedDoc, "Предоставляемый документ").Column("PROVDOC_ID").NotNull().Fetch();
        }
    }
}
