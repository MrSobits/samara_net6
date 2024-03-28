namespace Bars.GkhGji.Regions.Tatarstan.Dto
{
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    /// <summary>
    /// Привязка доп. сущностей к <see cref="RapidResponseSystemAppeal"/>
    /// </summary>
    public class RapidResponseSystemAppealLink
    {
        /// <summary>
        /// Обращение СОПР
        /// </summary>
        public RapidResponseSystemAppeal Appeal { get; set; }

        /// <summary>
        /// Детализация обращения СОПР
        /// </summary>
        public RapidResponseSystemAppealDetails AppealDetails { get; set; }
    }
}