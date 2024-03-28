namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;
    
    /// <summary>
    /// Результат проверки для акта
    /// </summary>
    public class ActCheckVerificationResult : BaseEntity
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// Результат проверки
        /// </summary>
        public virtual TypeVerificationResult TypeVerificationResult { get; set; }

    }
}
