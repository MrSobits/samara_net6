namespace Bars.Gkh.Gis.MessageContext
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json;

    public class DefaultMessageContextProvider<TMessageContext>
        where TMessageContext : IMessageContext, new()
    {

        private readonly JsonSerializer _serializer;
        protected ConcurrentDictionary<Guid, TMessageContext> ContextDictionary;
#if NETSTANDARD1_5
		private readonly AsyncLocal<Guid> _globalMsgId;
#elif NET451
		private const string GlobalMsgId = "RawRabbit:GlobalMessageId";
#endif
        public DefaultMessageContextProvider(JsonSerializer serializer)
        {
            _serializer = serializer;
            ContextDictionary = new ConcurrentDictionary<Guid, TMessageContext>();
#if NETSTANDARD1_5
			_globalMsgId = new AsyncLocal<Guid>();
#endif
        }

        public object GetMessageContext(ref Guid globalMessageId)
        {
#if NETSTANDARD1_5
			if (globalMessageId == Guid.Empty)
			{
				globalMessageId = _globalMsgId?.Value ?? globalMessageId;
			}
#elif NET451
			if (globalMessageId == Guid.Empty)
			{
				globalMessageId = (Guid?)CallContext.LogicalGetData(GlobalMsgId) ?? globalMessageId;
			}
#endif
            var context = CreateMessageContext(globalMessageId);
            var contextAsJson = SerializeContext(context);
            var contextAsBytes = (object)Encoding.UTF8.GetBytes(contextAsJson);
            return contextAsBytes;
        }

        private string SerializeContext(TMessageContext messageContext)
        {
            using (var sw = new StringWriter())
            {
                _serializer.Serialize(sw, messageContext);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected TMessageContext CreateMessageContext(Guid globalRequestId = default(Guid))
        {
            if(globalRequestId == Guid.Empty) globalRequestId = Guid.NewGuid();
            var context = new TMessageContext { GlobalRequestId = globalRequestId };
            
            return context;
        }
    }
}
