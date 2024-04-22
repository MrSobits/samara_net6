namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using Bars.B4.Utils;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.SystemDataTransfer.Meta.Services;

    using CoreWCF;
    using CoreWCF.Channels;
    using CoreWCF.Dispatcher;

    public class AuthTokenHeaderInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        private readonly string accessToken;

        public AuthTokenHeaderInspector(DataTransferIntegrationConfigs confguration)
        {
            this.accessToken = confguration.AccessToken;
        }

        /// <inheritdoc />
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (this.accessToken.IsEmpty())
            {
                throw new FaultException("Access token is missing");
            }

            request.Headers.Add(MessageHeader.CreateHeader("Token", string.Empty, this.GetAccessToken()));

            return null;
        }

        /// <inheritdoc />
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        /// <inheritdoc />
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
#if !DEBUG

            if (request.Headers.All(x => x.Name != "Token"))
            {
                throw new FaultException<Result>(Result.AuthorizationFailed, "Access token header is missing");
            }

            var headerValue = request.Headers.GetHeader<string>("Token", string.Empty);

            if (!this.ValidateToken(headerValue))
            {
                throw new FaultException<Result>(Result.AuthorizationFailed, "Access token is invalid");
            }
#endif

            return null;
        }

        /// <inheritdoc />
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        private string GetAccessToken()
        {
            var token = $"{this.accessToken}_my_salt";
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }

        private bool ValidateToken(string token)
        {
            return this.GetAccessToken() == token;
        }
    }
}