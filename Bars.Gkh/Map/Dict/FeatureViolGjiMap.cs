namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Группа нарушений"</summary>
    public class FeatureViolGjiMap : BaseEntityMap<FeatureViolGji>
    {
        public FeatureViolGjiMap() : base("Группа нарушений", "GJI_DICT_FEATUREVIOL") { }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(900).NotNull();
            this.Property(x => x.FullName, "Полное наименование включая и родительские группы разделенные через \'/\'").Column("FULL_NAME").Length(2000);
            this.Property(x => x.Code, "Код").Column("CODE").Length(300);
            this.Property(x => x.GkhReformCode, "Код реформы ЖКХ").Column("GKH_REFORM_CODE").Length(300);
            this.Property(x => x.QuestionCode, "Код характеристики в Тематическом классификаторе").Column("QUESTION_CODE");
            this.Property(x => x.IsActual, "Актуальность").Column("IS_ACTUAL").NotNull();
            this.Property(x => x.TrackAppealCits, "Отслеживать обращение").Column("TRACK_APPEAL_CITS");
            this.Reference(x => x.Parent, "Родительская группа нарушений").Column("PARENT_ID").Fetch();
        }
    }
}
