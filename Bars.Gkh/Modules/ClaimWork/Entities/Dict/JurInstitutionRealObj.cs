namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using B4.DataAccess;
    using Gkh.Entities;

    /// <summary>
    /// Связь Учреждения в судебной практике и Жилого дома
    /// </summary>
    public class JurInstitutionRealObj : BaseEntity
    {
        /// <summary>
        /// Учреждение в судебной практике
        /// </summary>
        public virtual JurInstitution JurInstitution { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}