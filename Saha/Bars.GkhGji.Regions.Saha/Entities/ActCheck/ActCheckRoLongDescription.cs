namespace Bars.GkhGji.Regions.Saha.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ActCheckRoLongDescription : BaseEntity
    {
        public virtual ActCheckRealityObject ActCheckRo { get; set; }

        public virtual byte[] NotRevealedViolations { get; set; }
    }
}
