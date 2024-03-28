namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Акт без взаимодействия"</summary>
    public class ActIsolatedMap : JoinedSubClassMap<ActIsolated>
    {

        public ActIsolatedMap() :
                base("Акт без взаимодействия", "GJI_ACTISOLATED")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Area, "Проверяемая площадь").Column("AREA");
            this.Property(x => x.Flat, "Квартира").Column("FLAT").Length(10);
            this.Property(x => x.DocumentTime, "DocumentTime").Column("DOCUMENT_TIME");
            this.Reference(x => x.DocumentPlaceFias, "Место составления (выбор из ФИАС)").Column("DOCUMENT_PLACE_FIAS_ID");
        }
    }
}
