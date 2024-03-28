namespace Bars.GkhGji.Regions.Smolensk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ActCheckRealityObjectDescription : BaseEntity
    {
        public virtual ActCheckRealityObject ActCheckRealityObject { get; set; }

        public virtual byte[] Description { get; set; }
    }
}