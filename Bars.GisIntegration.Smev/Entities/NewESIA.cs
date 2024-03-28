using EsiaNET;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bars.B4.Modules.ESIA.OAuth20.Entities
{
    public class EsiaClientSobits
    {
        private HttpClient _httpClient;

        public EsiaOptions Options { get; }

        public EsiaToken Token { get; set; }

        public HttpStatusCode LastStatusCode { get; private set; }

        public string LastStatusMessage { get; private set; }

        protected HttpClient HttpClient => _httpClient ?? (_httpClient = CreateHttpClient());

        public EsiaClientSobits(EsiaOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            Options = options;
            if (string.IsNullOrWhiteSpace(Options.ClientId))
            {
                
            }

            if (string.IsNullOrWhiteSpace(Options.Scope))
            {
                
            }

            if (string.IsNullOrWhiteSpace(Options.RequestType))
            {
                
            }

            if (Options.SignProvider == null)
            {
               
            }
        }

        public EsiaClientSobits(EsiaOptions options, string accessToken)
            : this(options, new EsiaToken(accessToken))
        {
        }

        public EsiaClientSobits(EsiaOptions options, EsiaToken token)
            : this(options)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            Token = token;
        }

        public virtual string BuildRedirectUri(string callbackUri = null)
        {
            if (string.IsNullOrEmpty(callbackUri))
            {
                callbackUri = Options.CallbackUri;
            }

            string text = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss +0000");
            string text2 = Options.State.ToString("D");
            string stringToEscape = BuildClientSecret(Options.Scope, text, Options.ClientId, text2);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Options.RedirectUri);
            stringBuilder.AppendFormat("?client_id={0}", Uri.EscapeDataString(Options.ClientId));
            stringBuilder.AppendFormat("&scope={0}", Uri.EscapeDataString(Options.Scope));
            stringBuilder.AppendFormat("&response_type={0}", Uri.EscapeDataString(Options.RequestType));
            stringBuilder.AppendFormat("&state={0}", Uri.EscapeDataString(text2));
            stringBuilder.AppendFormat("&timestamp={0}", Uri.EscapeDataString(text));
            stringBuilder.AppendFormat("&access_type={0}", Uri.EscapeDataString((Options.AccessType == AccessType.Online) ? "online" : "offline"));
            stringBuilder.AppendFormat("&redirect_uri={0}", Uri.EscapeDataString(callbackUri));
            stringBuilder.AppendFormat("&client_secret={0}", Uri.EscapeDataString(stringToEscape));
            return stringBuilder.ToString();
        }

        public virtual async Task<EsiaTokenResponse> GetOAuthTokenAsync(string authCode, string callbackUri = null)
        {
            if (string.IsNullOrEmpty(authCode))
            {
                throw new ArgumentNullException("authCode");
            }

            if (string.IsNullOrEmpty(callbackUri))
            {
                callbackUri = Options.CallbackUri;
            }

            return await InternalGetOAuthTokenAsync(authCode, callbackUri, TokenRequest.ByAuthCode);
        }

        public virtual async Task<EsiaTokenResponse> GetOAuthTokenByRefreshAsync(string refreshToken, string callbackUri = null)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException("refreshToken");
            }

            if (string.IsNullOrEmpty(callbackUri))
            {
                callbackUri = Options.CallbackUri;
            }

            return await InternalGetOAuthTokenAsync(refreshToken, callbackUri, TokenRequest.ByRefresh);
        }

        public virtual async Task<EsiaTokenResponse> GetOAuthTokenByCredentialsAsync(string callbackUri = null)
        {
            if (string.IsNullOrEmpty(callbackUri))
            {
                callbackUri = Options.CallbackUri;
            }

            return await InternalGetOAuthTokenAsync(string.Empty, callbackUri, TokenRequest.ByCredential);
        }

        protected virtual async Task<EsiaTokenResponse> InternalGetOAuthTokenAsync(string code, string callbackUri, TokenRequest request)
        {
            string text = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss +0000");
            string state = Options.State.ToString("D");
            string value = BuildClientSecret(Options.Scope, text, Options.ClientId, state);
            string key;
            string value2;
            string value3;
            switch (request)
            {
                case TokenRequest.ByRefresh:
                    key = "refresh_token";
                    value2 = code;
                    value3 = "refresh_token";
                    break;
                case TokenRequest.ByCredential:
                    key = "response_type";
                    value2 = "token";
                    value3 = "client_credentials";
                    break;
                default:
                    key = "code";
                    value2 = code;
                    value3 = "authorization_code";
                    break;
            }

            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", Options.ClientId),
                new KeyValuePair<string, string>(key, value2),
                new KeyValuePair<string, string>("grant_type", value3),
                new KeyValuePair<string, string>("state", state),
                new KeyValuePair<string, string>("scope", Options.Scope),
                new KeyValuePair<string, string>("timestamp", text),
                new KeyValuePair<string, string>("token_type", "Bearer"),
                new KeyValuePair<string, string>("client_secret", value)
            };
            if (request != TokenRequest.ByCredential)
            {
                list.Add(new KeyValuePair<string, string>("redirect_uri", callbackUri));
            }

            FormUrlEncodedContent content = new FormUrlEncodedContent(list);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpResponseMessage response = await HttpClient.PostAsync(Options.TokenUri, content);
            await UpdateLastStatusCode(response);
            response.EnsureSuccessStatusCode();
            JObject jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            string text2 = jObject["access_token"].Value<string>();
            if (string.IsNullOrWhiteSpace(text2))
            {
                throw new Exception("Access token was not found");
            }

            string text3 = jObject.Value<string>("state");
            if (text3 == null || text3 != state)
            {
                throw new Exception("State parameter is missing or invalid");
            }

            return new EsiaTokenResponse
            {
                AccessToken = text2,
                RefreshToken = jObject.Value<string>("refresh_token"),
                ExpiresIn = jObject.Value<string>("expires_in")
            };
        }

        private async Task UpdateLastStatusCode(HttpResponseMessage response)
        {
            LastStatusCode = response.StatusCode;
            LastStatusMessage = null;
            if (!response.IsSuccessStatusCode)
            {
                string text2 = (LastStatusMessage = await response.Content.ReadAsStringAsync());
            }
        }

        public virtual bool VerifyToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException("accessToken");
            }

            string[] array = accessToken.Split('.');
            JObject jObject = JObject.Parse(Encoding.UTF8.GetString(Base64Decode(array[0])));
            if (array.Length > 2)
            {
                return VerifySignature(jObject.Value<string>("alg"), $"{array[0]}.{array[1]}", array[2]);
            }

            return false;
        }

        public static EsiaToken CreateToken(EsiaTokenResponse tokenResponse)
        {
            if (tokenResponse == null)
            {
                throw new ArgumentNullException("tokenResponse");
            }

            string[] array = tokenResponse.AccessToken.Split('.');
            JObject payload = JObject.Parse(Encoding.UTF8.GetString(Base64Decode(array[1])));
            return new EsiaToken(tokenResponse.AccessToken, tokenResponse.RefreshToken, tokenResponse.ExpiresIn, payload);
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, SendStyles styles = SendStyles.Normal, IList<KeyValuePair<string, string>> requestParams = null, IList<KeyValuePair<string, string>> headers = null)
        {
            CheckTokenExist();
            DateTime now = DateTime.Now;
            if (styles.HasFlag(SendStyles.CheckTokenTime) && Token.BeginDate.HasValue)
            {
                DateTime value = now;
                DateTime? beginDate = Token.BeginDate;
                if (value < beginDate)
                {
                    throw new Exception("Token start time has not come");
                }
            }

            if (styles.HasFlag(SendStyles.CheckTokenTime) && Token.EndDate.HasValue)
            {
                DateTime value = now;
                DateTime? beginDate = Token.EndDate;
                if (value > beginDate && styles.HasFlag(SendStyles.RefreshToken) && !string.IsNullOrEmpty(Token.RefreshToken))
                {
                    await UpdateToken(styles);
                }
            }

            HttpResponseMessage httpResponseMessage = await InternalSendAsync(method, requestUri, requestParams, headers);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized && styles.HasFlag(SendStyles.RefreshToken))
            {
                await UpdateToken(styles);
                httpResponseMessage = await InternalSendAsync(method, requestUri, requestParams, headers);
            }

            return httpResponseMessage;
        }

        private void CheckTokenExist()
        {
            if (Token == null)
            {
                throw new ArgumentNullException("Token");
            }
        }

        public virtual async Task<string> GetAsync(string requestUri, SendStyles styles = SendStyles.Normal)
        {
            HttpResponseMessage httpResponseMessage = await SendAsync(HttpMethod.Get, requestUri, styles);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public virtual async Task<string> PostAsync(string requestUri, SendStyles styles = SendStyles.Normal, IList<KeyValuePair<string, string>> requestParams = null)
        {
            HttpResponseMessage httpResponseMessage = await SendAsync(HttpMethod.Post, requestUri, styles, requestParams);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public virtual async Task<string> PutAsync(string requestUri, SendStyles styles = SendStyles.Normal, IList<KeyValuePair<string, string>> requestParams = null)
        {
            HttpResponseMessage httpResponseMessage = await SendAsync(HttpMethod.Put, requestUri, styles, requestParams);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        private async Task UpdateToken(SendStyles styles)
        {
            EsiaTokenResponse esiaTokenResponse = await GetOAuthTokenByRefreshAsync(Token.RefreshToken);
            if (styles.HasFlag(SendStyles.VerifyToken) && !VerifyToken(esiaTokenResponse.AccessToken))
            {
                throw new Exception("Token signature is invalid");
            }

            Token = CreateToken(esiaTokenResponse);
        }

        protected virtual async Task<HttpResponseMessage> InternalSendAsync(HttpMethod method, string requestUri, IList<KeyValuePair<string, string>> requestParams, IList<KeyValuePair<string, string>> headers)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, requestUri);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            if (requestParams != null)
            {
                httpRequestMessage.Content = new FormUrlEncodedContent(requestParams);
            }

            HttpResponseMessage response = await HttpClient.SendAsync(httpRequestMessage);
            await UpdateLastStatusCode(response);
            return response;
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                Timeout = Options.BackchannelTimeout,
                MaxResponseContentBufferSize = 10485760L
            };
        }

        private string BuildClientSecret(string scope, string timestamp, string clientId, string state)
        {
            string s = $"{scope}{timestamp}{clientId}{state}";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            X509Certificate2 certificate = Options.SignProvider.GetCertificate();
            if (certificate == null)
            {
               
            }

            byte[] input = Options.SignProvider.SignMessage(bytes, certificate);
            return Base64UrlEncode(input);
        }

        private string Base64UrlEncode(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (input.Length < 1)
            {
                return string.Empty;
            }

            string text = null;
            int num = 0;
            char[] array = null;
            text = Convert.ToBase64String(input);
            num = text.Length;
            while (num > 0 && text[num - 1] == '=')
            {
                num--;
            }

            array = new char[num + 1];
            array[num] = (char)(48 + text.Length - num);
            for (int i = 0; i < num; i++)
            {
                char c = text[i];
                switch (c)
                {
                    case '+':
                        array[i] = '-';
                        break;
                    case '/':
                        array[i] = '_';
                        break;
                    case '=':
                        array[i] = c;
                        break;
                    default:
                        array[i] = c;
                        break;
                }
            }

            return new string(array);
        }

        private static byte[] Base64Decode(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');
            switch (input.Length % 4)
            {
                case 2:
                    input = $"{input}==";
                    break;
                case 3:
                    input = $"{input}=";
                    break;
                default:
                    throw new Exception("Illegal base64url string!");
                case 0:
                    break;
            }

            return Convert.FromBase64String(input);
        }

        private bool VerifySignature(string alg, string message, string signature)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            byte[] signature2 = Base64Decode(signature);
            X509Certificate2 esiaCertificate = Options.SignProvider.GetEsiaCertificate();
            if (esiaCertificate == null)
            {
               
            }

            return Options.SignProvider.VerifyMessage(alg, bytes, signature2, esiaCertificate);
        }
    }
}
