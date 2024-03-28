namespace Bars.Esia.OAuth20.App.Application
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Entities;
    using Bars.Esia.OAuth20.App.Enums;
    using Bars.Esia.OAuth20.App.Providers;

    using Castle.Windsor;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Класс клиента ЕСИА
    /// </summary>
    public class EsiaClient
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        private IWindsorContainer Container = ApplicationContext.Current.Container;

        /// <summary>
        /// Поставщик безопасности приложения
        /// </summary>
        private IAuthAppSecurityProvider AuthAppSecurityProvider { get; set; }

        /// <summary>
        /// Http-клиент для подключения к ЕСИА
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// Настройки ЕСИА
        /// </summary>
        public EsiaOptions Options { get; set; }

        /// <summary>
        /// Информация о маркере доступа
        /// </summary>
        public EsiaOAuthTokenInfo OAuthTokenInfo { get; set; }

        public EsiaClient()
        {
            this.AuthAppSecurityProvider = this.Container.Resolve<IAuthAppSecurityProvider>();

            var authAppOptionProvider = this.Container.Resolve<IAuthAppOptionProvider>();

            this.Options = authAppOptionProvider.GetEsiaOptions();
            this.Options.State = Guid.NewGuid();

            this.httpClient = this.CreateHttpClient();
        }

        /// <summary>
        /// Сформировать Uri для получения кода доступа
        /// </summary>
        public string BuildRedirectUri()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss +0000");
            var state = this.Options.State.ToString("D");
            var clientSecret = this.BuildClientSecret(this.Options.Scope, timestamp, this.Options.ClientId, state);
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(this.Options.RedirectUri);
            stringBuilder.AppendFormat("?client_id={0}", Uri.EscapeDataString(this.Options.ClientId));
            stringBuilder.AppendFormat("&scope={0}", Uri.EscapeDataString(this.Options.Scope));
            stringBuilder.AppendFormat("&response_type={0}", Uri.EscapeDataString(this.Options.RequestType));
            stringBuilder.AppendFormat("&state={0}", Uri.EscapeDataString(state));
            stringBuilder.AppendFormat("&timestamp={0}", Uri.EscapeDataString(timestamp));
            stringBuilder.AppendFormat("&access_type={0}", Uri.EscapeDataString(this.Options.AccessType.GetDisplayName().ToLower()));
            stringBuilder.AppendFormat("&redirect_uri={0}", Uri.EscapeDataString(this.Options.CallbackUri));
            stringBuilder.AppendFormat("&client_secret={0}", Uri.EscapeDataString(clientSecret));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Получить маркер доступа по коду авторизации
        /// </summary>
        public async Task<EsiaOAuthToken> GetOAuthTokenAsync(string code, string callbackUri = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (string.IsNullOrEmpty(callbackUri))
            {
                callbackUri = this.Options.CallbackUri;
            }

            return await this.InternalGetOAuthTokenAsync(code, callbackUri, TokenRequest.ByAuthCode);
        }

        /// <summary>
        /// Получить маркер доступа по токену обновления
        /// </summary>
        public async Task<EsiaOAuthToken> GetOAuthTokenByRefreshAsync(string refreshToken, string callbackUri = null)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            if (string.IsNullOrEmpty(callbackUri))
            {
                callbackUri = this.Options.CallbackUri;
            }

            return await this.InternalGetOAuthTokenAsync(refreshToken, callbackUri, TokenRequest.ByRefresh);
        }

        /// <summary>
        /// Формирование запроса для получения токена доступа
        /// </summary>
        private async Task<EsiaOAuthToken> InternalGetOAuthTokenAsync(string code, string callbackUri, TokenRequest request)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss +0000");
            var state = this.Options.State.ToString("D");
            var clientSecret = this.BuildClientSecret(this.Options.Scope, timestamp, this.Options.ClientId, state);

            string key;
            string value;
            string grantType;

            switch (request)
            {
                case TokenRequest.ByRefresh:
                    key = "refresh_token";
                    value = code;
                    grantType = "refresh_token";
                    break;
                case TokenRequest.ByCredential:
                    key = "response_type";
                    value = "token";
                    grantType = "client_credentials";
                    break;
                default:
                    key = nameof(code);
                    value = code;
                    grantType = "authorization_code";
                    break;
            }

            var paramsDict = new Dictionary<string, string>
            {
                { "client_id", this.Options.ClientId },
                { key, value },
                { "grant_type", grantType },
                { "state", state },
                { "scope", this.Options.Scope },
                { "timestamp", timestamp },
                { "token_type", "Bearer" },
                { "client_secret", clientSecret }
            };

            if (request != TokenRequest.ByCredential)
                paramsDict.Add("redirect_uri", callbackUri);

            var response = await this.httpClient.PostAsync(this.Options.TokenUri,
                new FormUrlEncodedContent(paramsDict));

            response.EnsureSuccessStatusCode();

            var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());

            var accessToken = jObject["access_token"].Value<string>();
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new Exception("Маркер доступа не найден");

            var stateInResponse = jObject.Value<string>("state");
            if (stateInResponse == null || stateInResponse != state)
                throw new Exception("Параметр состояния не указан или некорректен");

            return new EsiaOAuthToken
            {
                AccessToken = accessToken,
                RefreshToken = jObject.Value<string>("refresh_token"),
                ExpiresIn = jObject.Value<string>("expires_in")
            };
        }

        /// <summary>
        /// Проверить валидность токена доступа
        /// </summary>
        public bool VerifyToken(string accessToken)
        {
            var strArray = !string.IsNullOrEmpty(accessToken)
                ? accessToken.Split('.')
                : throw new ArgumentNullException(nameof(accessToken));

            var jObject = JObject.Parse(Encoding.UTF8.GetString(this.Base64Decode(strArray[0])));

            return strArray.Length > 2 && this.VerifySignature(jObject.Value<string>("alg"),
                $"{strArray[0]}.{strArray[1]}",
                strArray[2]);
        }

        /// <summary>
        /// Обновить информацию о токене для клиента
        /// </summary>
        public void UpdateClientTokenInfo(EsiaOAuthToken tokenResponse)
        {
            if (tokenResponse == null)
                throw new ArgumentNullException(nameof(tokenResponse));

            var payload = JObject.Parse(Encoding.UTF8.GetString(this.Base64Decode(tokenResponse.AccessToken.Split('.')[1])));

            this.OAuthTokenInfo = new EsiaOAuthTokenInfo(tokenResponse.AccessToken, tokenResponse.RefreshToken, tokenResponse.ExpiresIn, payload);
        }

        /// <summary>
        /// Отправить HTTP-запрос в ЕСИА
        /// </summary>
        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, SendType styles = SendType.Normal,
            IList<KeyValuePair<string, string>> requestParams = null, IList<KeyValuePair<string, string>> headers = null)
        {
            if (styles.HasFlag(SendType.CheckTokenTime))
            {
                if (this.OAuthTokenInfo.BeginDate.HasValue)
                {
                    var beginDateRatio = this.OAuthTokenInfo.BeginDate.Value - DateTime.Now;

                    // Действие еще не наступило
                    if (beginDateRatio.TotalMilliseconds > 0)
                    {
                        // Разница в рамках 15-ти секунд
                        if (beginDateRatio.TotalMilliseconds <= 15000)
                        {
                            Thread.Sleep(beginDateRatio.TotalMilliseconds.ToInt());
                        }
                        else
                        {
                            throw new Exception($"Время действия токена еще не наступило - разница в {beginDateRatio.TotalMilliseconds} миллисек");
                        }
                    }
                }
                else
                {
                    throw new Exception("Для токена не указано время начала действия");
                }

                if (this.OAuthTokenInfo.EndDate.HasValue)
                {
                    if (this.OAuthTokenInfo.EndDate.Value < DateTime.Now &&
                        styles.HasFlag(SendType.RefreshToken) &&
                        this.OAuthTokenInfo.RefreshToken.IsNotEmpty())
                    {
                        await this.UpdateToken(styles);
                    }
                }
                else
                {
                    throw new Exception("Для токена не указано время окончания действия");
                }
            }

            var httpResponseMessage = await this.InternalSendAsync(method, requestUri, requestParams, headers);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized && styles.HasFlag(SendType.RefreshToken))
            {
                await this.UpdateToken(styles);
                httpResponseMessage = await this.InternalSendAsync(method, requestUri, requestParams, headers);
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// Получить данные, отправив HTTP-запрос к ЕСИА
        /// </summary>
        public async Task<string> GetAsync(string requestUri, SendType styles = SendType.Normal)
        {
            var response = await this.SendAsync(HttpMethod.Get, requestUri, styles);

            return !response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Отправить данные для создание
        /// </summary>
        public async Task<string> PostAsync(string requestUri, SendType styles = SendType.Normal,
            IList<KeyValuePair<string, string>> requestParams = null)
        {
            var response = await this.SendAsync(HttpMethod.Post, requestUri, styles, requestParams);

            return !response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Отправить данные для обновления
        /// </summary>
        public async Task<string> PutAsync(string requestUri, SendType styles = SendType.Normal,
            IList<KeyValuePair<string, string>> requestParams = null)
        {
            var response = await this.SendAsync(HttpMethod.Put, requestUri, styles, requestParams);

            return !response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Обновить токен доступа
        /// </summary>
        private async Task UpdateToken(SendType styles)
        {
            var tokenByRefreshAsync = await this.GetOAuthTokenByRefreshAsync(this.OAuthTokenInfo.RefreshToken);

            /*if (styles.HasFlag(SendType.VerifyToken) && !this.VerifyToken(tokenByRefreshAsync.AccessToken))
                throw new Exception("Маркер доступа не прошел проверку подлинности");*/

            this.UpdateClientTokenInfo(tokenByRefreshAsync);
        }

        /// <summary>
        /// Отправка HTTP-запроса к ЕСИА
        /// </summary>
        private async Task<HttpResponseMessage> InternalSendAsync(HttpMethod method, string requestUri,
            IList<KeyValuePair<string, string>> requestParams, IList<KeyValuePair<string, string>> headers)
        {
            var request = new HttpRequestMessage(method, requestUri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.OAuthTokenInfo.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (headers != null)
            {
                foreach (var header in headers)
                    request.Headers.Add(header.Key, header.Value);
            }

            if (requestParams != null)
            {
                request.Content = new FormUrlEncodedContent(requestParams);
            }

            return await this.httpClient.SendAsync(request);
        }

        /// <summary>
        /// HTTP-клиент для отправки запросов к ЕСИА
        /// </summary>
        private HttpClient CreateHttpClient() => new HttpClient
        {
            Timeout = this.Options.RequestTimeout,
            MaxResponseContentBufferSize = this.Options.MaxResponseContentBufferSize
        };

        /// <summary>
        /// Сформировать секретный ключ
        /// </summary>
        private string BuildClientSecret(string scope, string timestamp, string clientId, string state)
        {
            var bytes = Encoding.UTF8.GetBytes($"{scope}{timestamp}{clientId}{state}");

            return this.Base64UrlEncode(this.AuthAppSecurityProvider.SignMessage(bytes,
                this.AuthAppSecurityProvider.GetSystemCertificate() ?? throw new ArgumentException("Сертификат системы не найден")));
        }

        /// <summary>
        /// Закодировать сообщение
        /// </summary>
        private string Base64UrlEncode(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Length < 1)
                return string.Empty;

            var base64String = Convert.ToBase64String(input);
            var length = base64String.Length;

            while (length > 0 && base64String[length - 1] == '=')
                --length;

            var chArray = new char[length];

            for (var index = 0; index < length; ++index)
            {
                var ch = base64String[index];
                switch (ch)
                {
                    case '+':
                        chArray[index] = '-';
                        break;
                    case '/':
                        chArray[index] = '_';
                        break;
                    default:
                        chArray[index] = ch;
                        break;
                }
            }

            return new string(chArray);
        }

        /// <summary>
        /// Раскодировать сообщение
        /// </summary>
        private byte[] Base64Decode(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');

            switch (input.Length % 4)
            {
                case 0:
                    return Convert.FromBase64String(input);
                case 2:
                    input += "==";
                    goto case 0;
                case 3:
                    input += "=";
                    goto case 0;
                default:
                    throw new Exception("Декодируемое сообщение не подписано");
            }
        }

        /// <summary>
        /// Проверить валидность содержимого сообщения
        /// </summary>
        private bool VerifySignature(string alg, string message, string signature)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            var decodedSignature = this.Base64Decode(signature);
            return this.AuthAppSecurityProvider.VerifyMessage(alg, bytes, decodedSignature,
                this.AuthAppSecurityProvider.GetEsiaCertificate() ?? throw new ArgumentException("Сертификат ЕСИА не найден"));
        }
    }
}