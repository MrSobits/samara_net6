using System;

namespace Bars.Gkh.Gis.MessageContext
{
    public interface IMessageContext
    {
        Guid GlobalRequestId { get; set; }
    }

    public class MessageContext : IMessageContext
    {
        public Guid GlobalRequestId { get; set; }
    }
}
