namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    /// <summary>Маппинг для "Тематика обращения ГЖИ"</summary>
    public class StatSubjectGjiMap : BaseEntityMap<StatSubjectGji>
    {
        public StatSubjectGjiMap() : base("Тематика обращения ГЖИ", "GJI_DICT_STATEMENT_SUBJ")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            this.Property(x => x.Code, "Код").Column("CODE").Length(300);
            this.Property(x => x.QuestionCode, "Код тематики в Тематическом классификаторе").Column("QUESTION_CODE").Length(20);
            this.Property(x => x.SSTUName, "Наименование ССТУ").Column("SSTU_NAME").Length(500);
            this.Property(x => x.SSTUCode, "Код ССТУ").Column("SSTU_CODE").Length(300);
            this.Property(x => x.ISSOPR, "Выгружать в СОПР").Column("IS_SOPR");
            this.Property(x => x.TrackAppealCits, "Отслеживать обращение").Column("TRACK_APPEAL_CITS");
            this.Property(x => x.NeedInSopr, "Учитывается в CОПР").Column("NEED_IN_SOPR").NotNull().DefaultValue(false);
        }
    }
}
