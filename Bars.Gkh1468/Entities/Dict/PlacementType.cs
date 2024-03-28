namespace Bars.Gkh1468.Entities
{
    using Bars.B4.DataAccess;

    public class PlacementType : BaseEntity
    {
        public virtual string Name { get; set; }
        public virtual string ShortName { get; set; }
    }
}