namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Подтематика обращения"</summary>
    public class StatSubsubjectGjiMap : BaseEntityMap<StatSubsubjectGji>
    {
        
        public StatSubsubjectGjiMap() : 
            base("Подтематика обращения", "GJI_DICT_STAT_SUB_SUBJECT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            this.Property(x => x.Code, "Код").Column("CODE").Length(300);
            this.Property(x => x.QuestionCode, "Код подтематики в Тематическом классификаторе").Column("QUESTION_CODE");
            this.Property(x => x.SSTUNameSub, "Наименование ССТУ").Column("SSTU_NAME_SUB").Length(500);
            this.Property(x => x.SSTUCodeSub, "Код ССТУ").Column("SSTU_CODE_SUB").Length(300);
            this.Property(x => x.ISSOPR, "Выгружать в СОПР").Column("IS_SOPR");
            this.Property(x => x.AppealAnswerText, "Текст стандартного ответа").Column("ANSWER_TEXT").Length(20000);
            this.Property(x => x.AppealAnswerText2, "Текст стандартного ответа 2").Column("ANSWER_TEXT2").Length(1500);
            this.Property(x => x.TrackAppealCits, "Отслеживать обращение").Column("TRACK_APPEAL_CITS");
            this.Property(x => x.NeedInSopr, "Учитывается в СОПР").Column("NEED_IN_SOPR").NotNull();
        }
    }
}
