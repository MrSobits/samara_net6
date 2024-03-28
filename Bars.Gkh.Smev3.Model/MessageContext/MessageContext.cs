using System;

namespace Bars.Gkh.Utils.Json
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
