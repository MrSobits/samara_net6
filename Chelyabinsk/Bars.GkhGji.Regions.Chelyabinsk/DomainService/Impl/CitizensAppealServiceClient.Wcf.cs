namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Text;

    public partial class CitizensAppealServiceClient
    {
        private class MessageInspector: IClientMessageInspector
        {
            private MessageContainer Container { get; }

            public MessageInspector(MessageContainer container)
            {
                this.Container = container;
            }

            /// <inheritdoc />
            public object BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                this.Container.Requests.Add($"-- REQUEST[{DateTime.Now:u}] --\r\n{request}\r\n----");

                return null;
            }

            /// <inheritdoc />
            public void AfterReceiveReply(ref Message reply, object correlationState)
            {
                this.Container.Responses.Add($"-- RESPONSE[{DateTime.Now:u}] --\r\n{reply}\r\n----");
            }
        }

        private class MessageInspectorBehavior : IEndpointBehavior
        {
            private MessageContainer Container { get; }

            public MessageInspectorBehavior(MessageContainer container)
            {
                this.Container = container;
            }

            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                clientRuntime.ClientMessageInspectors.Add(new MessageInspector(this.Container));
            }

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            {
            }

            public void Validate(ServiceEndpoint endpoint)
            {
            }
        }

        private class MessageContainer
        {
            public ConcurrentBag<string> Requests { get; } = new ConcurrentBag<string>();

            public ConcurrentBag<string> Responses { get; } = new ConcurrentBag<string>();

            /// <inheritdoc />
            public override string ToString()
            {
                var requests = this.Requests.ToArray();
                var responses = this.Responses.ToArray();

                var sb = new StringBuilder();
                var requestIdx = 0;
                var responseIdx = 0;
                while (requestIdx < requests.Length || responseIdx < responses.Length)
                {
                    if (requestIdx < requests.Length)
                    {
                        sb.AppendLine(requests[requestIdx++]);
                    }

                    if (responseIdx < responses.Length)
                    {
                        sb.AppendLine(responses[responseIdx++]);
                    }
                }
                return sb.ToString();
            }
        }
    }
}