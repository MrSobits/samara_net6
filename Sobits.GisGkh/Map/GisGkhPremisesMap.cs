namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhPremises"</summary>
    public class GisGkhPremisesMap : BaseEntityMap<GisGkhPremises>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GisGkhPremisesMap()
            : base("Sobits.GisGkh.Entities", GisGkhPremisesMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_PREMISES";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.RealityObject, "Дом в системе").Column(nameof(GisGkhPremises.RealityObject).ToLower()).NotNull().Fetch();
            this.Property(x => x.RoomType, "Тип помещения").Column(nameof(GisGkhPremises.RoomType).ToLower());
            this.Property(x => x.PremisesNum, "Номер помещения").Column(nameof(GisGkhPremises.PremisesNum).ToLower());
            this.Property(x => x.Floor, "Этаж").Column(nameof(GisGkhPremises.Floor).ToLower());
            this.Property(x => x.TotalArea, "Общая площадь").Column(nameof(GisGkhPremises.TotalArea).ToLower());
            this.Property(x => x.GrossArea, "Жилая площадь").Column(nameof(GisGkhPremises.GrossArea).ToLower());
            this.Property(x => x.ModificationDate, "Дата изменения").Column(nameof(GisGkhPremises.ModificationDate).ToLower()).NotNull();
            this.Property(x => x.TerminationDate, "Дата аннулирования").Column(nameof(GisGkhPremises.TerminationDate).ToLower());
            this.Property(x => x.IsCommonProperty, "Является общим имуществом").Column(nameof(GisGkhPremises.IsCommonProperty).ToLower());
            this.Property(x => x.PremisesUniqueNumber, "Уникальный номер помещения").Column(nameof(GisGkhPremises.PremisesUniqueNumber).ToLower());
            this.Property(x => x.PremisesGUID, "GUID помещения").Column(nameof(GisGkhPremises.PremisesGUID).ToLower());
            this.Property(x => x.EntranceNum, "Номер подъезда").Column(nameof(GisGkhPremises.EntranceNum).ToLower());
            this.Property(x => x.CadastralNumber, "Кадастровый номер").Column(nameof(GisGkhPremises.CadastralNumber).ToLower());
            this.Property(x => x.No_RSO_GKN_EGRP_Data, "Нет сведений о кадастровом номере").Column(nameof(GisGkhPremises.No_RSO_GKN_EGRP_Data).ToLower());
            this.Property(x => x.No_RSO_GKN_EGRP_Registered, "Ключ связи с ГКН/ЕГРП отсутствует").Column(nameof(GisGkhPremises.No_RSO_GKN_EGRP_Registered).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class GisGkhPremisesNhMapping : ClassMapping<GisGkhPremises>
    {
        public GisGkhPremisesNhMapping()
        {
            this.Schema(GisGkhPremisesMap.SchemaName);
        }
    }
}