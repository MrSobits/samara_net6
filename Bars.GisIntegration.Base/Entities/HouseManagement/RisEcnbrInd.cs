namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Физ лицо обременитель
    /// </summary>
    public class RisEcnbrInd: BaseRisEntity
    {
        /// <summary>
        /// Обременение
        /// </summary>
        public virtual RisEcnbr Ecnbr { get; set; }

        /// <summary>
        /// Физ лицо
        /// </summary>
        public virtual RisInd Ind { get; set; }
    }
}
