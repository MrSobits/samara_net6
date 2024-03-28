namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>
    /// Маппинг для "Этап наказания за нарушение в постановлении Роспотребнадзора"
    /// </summary>
    public class ResolutionRospotrebnadzorViolationMap : BaseEntityMap<ResolutionRospotrebnadzorViolation>
    {
        #region Названия полей
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GJI_RESOLUTION_ROSPOTREBNADZOR_VIOLAT";

        /// <summary>
        /// Описание
        /// </summary>
        public const string Description = "DESCRIPTION";

        /// <summary>
        /// Нарушение
        /// </summary>
        public const string Violation = "VIOLATION_ID";

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public const string Resolution = "RESOLUTION_ID";
        #endregion

        public ResolutionRospotrebnadzorViolationMap() :
                base("Этап наказания за нарушение в протоколе", ResolutionRospotrebnadzorViolationMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Description, "Примечание").Column(ResolutionRospotrebnadzorViolationMap.Description);
            this.Reference(x => x.Violation, "Нарушение").Column(ResolutionRospotrebnadzorViolationMap.Violation).Fetch();
            this.Reference(x => x.Resolution, "Постановление Роспотребнадзора").Column(ResolutionRospotrebnadzorViolationMap.Resolution).Fetch();
        }
    }
}