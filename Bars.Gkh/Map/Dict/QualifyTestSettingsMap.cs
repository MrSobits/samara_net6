namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инспектор"</summary>
    public class QualifyTestSettingsMap : BaseEntityMap<QualifyTestSettings>
    {
        
        public QualifyTestSettingsMap() : 
                base("Вопросы к экзамену", "GKH_DICT_QTEST_SETTINGS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AcceptebleRate, "Код").Column("ACC_RATE").NotNull();
            Property(x => x.CorrectBall, "Баллов за вопрос").Column("POINTS_PER_QUESTION").NotNull();
            Property(x => x.DateFrom, "Дата с").Column("DATE_FROM").NotNull();
            Property(x => x.DateTo, "Дата по").Column("DATE_TO");
            Property(x => x.QuestionsCount, "Количество вопросов").Column("Q_COUNT");
            Property(x => x.TimeStampMinutes, "Время на экзамен").Column("TIME_LIMIT");

        }
    }
}
