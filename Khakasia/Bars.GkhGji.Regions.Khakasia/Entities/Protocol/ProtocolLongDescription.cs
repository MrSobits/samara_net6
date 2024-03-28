namespace Bars.GkhGji.Regions.Khakasia.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ProtocolLongDescription : BaseEntity
    {
        public virtual Protocol Protocol { get; set; }

        public virtual byte[] Description { get; set; }
    }
}