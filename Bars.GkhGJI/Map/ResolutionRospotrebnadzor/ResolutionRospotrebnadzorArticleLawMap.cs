namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>
    /// Маппинг для "Статьи в постановлении Роспотребнадзора"
    /// </summary>
    public class ResolutionRospotrebnadzorArticleLawMap : BaseEntityMap<ResolutionRospotrebnadzorArticleLaw>
    {
        #region Названия полей
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GJI_RESOLUTION_ROSPOTREBNADZOR_ARTICLELAW";

        /// <summary>
        /// Описание
        /// </summary>
        public const string Description = "DESCRIPTION";

        /// <summary>
        /// Статьи
        /// </summary>
        public const string ArticleLaw = "ARTICLELAW_ID";

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public const string Resolution = "RESOLUTION_ID";
        #endregion

        public ResolutionRospotrebnadzorArticleLawMap() :
                base("Этап наказания за нарушение в протоколе", ResolutionRospotrebnadzorArticleLawMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Description, "Примечание").Column(ResolutionRospotrebnadzorArticleLawMap.Description);
            this.Reference(x => x.ArticleLaw, "Статьи").Column(ResolutionRospotrebnadzorArticleLawMap.ArticleLaw).Fetch();
            this.Reference(x => x.Resolution, "Постановление Роспотребнадзора").Column(ResolutionRospotrebnadzorArticleLawMap.Resolution).Fetch();
        }
    }
}