namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197
{
    using Bars.B4.DataAccess;

    public class Protocol197LongText : BaseEntity
    {
        public virtual Protocol197 Protocol197 { get; set; }

        public virtual byte[] Description { get; set; }
        public virtual byte[] Witnesses { get; set; }
        public virtual byte[] Victims { get; set; }
    }
}