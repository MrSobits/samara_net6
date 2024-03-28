namespace Bars.GisIntegration.Base.Entities.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Экспортируемая из ГИС-а работа в КПР
    /// </summary>
    public class RisCrPlanWork : BaseRisEntity
    {
        /// <summary>
        /// Guid плана в ГИС-е
        /// </summary>
        public virtual string PlanGuid { get; set; }

        /// <summary>
        /// Многоквартирный дом
        /// </summary>
        public virtual string ApartmentBuildingFiasGuid { get; set; }

        /// <summary>
        /// Вид работ капитального ремонта (НСИ 219). Код.
        /// </summary>
        public virtual string WorkKindCode { get; set; }

        /// <summary>
        /// Вид работ капитального ремонта (НСИ 219). Guid.
        /// </summary>
        public virtual string WorkKindGuid { get; set; }

        /// <summary>
        /// Месяц и год окончания работ
        /// </summary>
        public virtual string EndMonthYear { get; set; }

        /// <summary>
        /// Муниципальное образование, в котором располагается многоквартирный дом. Код по ОКТМО.
        /// </summary>
        public virtual string MunicipalityCode { get; set; }

        /// <summary>
        /// Муниципальное образование, в котором располагается многоквартирный дом. Полное наименование.
        /// </summary>
        public virtual string MunicipalityName { get; set; }
    }
}
