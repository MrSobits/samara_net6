namespace Bars.GkhGji.Regions.Smolensk.Entities.Protocol
{
    using Bars.B4.DataAccess;

    public class ProtocolViolationDescription : BaseEntity
    {
        public virtual ProtocolSmol Protocol { get; set; }

        public virtual byte[] ViolationDescription { get; set; }

        public virtual byte[] ExplanationsComments { get; set; }
    }
}