namespace Bars.Esia.OAuth20.App.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Application;
    using Bars.Esia.OAuth20.App.Entities;
    using Bars.Esia.OAuth20.App.Enums;
    using Bars.Esia.OAuth20.App.Extensions;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Сервис операций приложения
    /// </summary>
    public class AuthAppOperationService : IAuthAppOperationService
    {
        /// <summary>
        /// Клиент ЕСИА
        /// </summary>
        private EsiaClient esiaClient = new EsiaClient();

        /// <inheritdoc />
        public string GetRedirectUri(DynamicDictionary operationParams)
        {
            this.SetClientCallbackUriOption(operationParams);

            return this.esiaClient.BuildRedirectUri();
        }

        /// <inheritdoc />
        public EsiaOAuthToken GetOAuthToken(DynamicDictionary operationParams)
        {
            var code = operationParams.GetAs("code", string.Empty);

            if (code.IsEmpty())
            {
                throw new Exception("Не передан код доступа (code)");
            }

            this.SetClientCallbackUriOption(operationParams);

            return this.esiaClient.GetOAuthTokenAsync(code).Result;
        }

        /// <inheritdoc />
        public EsiaPersonInfo GetPersonInfo(DynamicDictionary operationParams)
        {
            this.SetClientOAuthTokenInfo(operationParams);

            return this.GetPersonInfoAsync().Result;
        }

        /// <inheritdoc />
        public IEnumerable<EsiaPersonOrganizationInfo> GetPersonOrganizations(DynamicDictionary operationParams)
        {
            this.SetClientOAuthTokenInfo(operationParams);

            return this.GetPersonOrganizationsAsync().Result;
        }

        /// <inheritdoc />
        public IEnumerable<EsiaContactInfo> GetPersonContacts(DynamicDictionary operationParams)
        {
            this.SetClientOAuthTokenInfo(operationParams);

            return this.GetPersonContactsAsync().Result;
        }

        /// <inheritdoc />
        public IEnumerable<EsiaAddressInfo> GetPersonAddresses(DynamicDictionary operationParams)
        {
            this.SetClientOAuthTokenInfo(operationParams);

            return this.GetPersonAddressesAsync().Result;
        }

        /// <inheritdoc />
        public EsiaOrganizationInfo GetOrganizationInfo(DynamicDictionary operationParams)
        {
            this.SetClientOAuthTokenInfo(operationParams);
            
            return this.GetOrganizationInfoAsync().Result;
        }

        /// <summary>
        /// Указать для клиента ЕСИА информацию о маркере доступа
        /// </summary>
        private void SetClientOAuthTokenInfo(DynamicDictionary operationParams)
        {
            try
            {
                var token = operationParams.GetAs("token", string.Empty);

                if (token.IsEmpty())
                    throw new Exception("Не передана информация о маркере доступа (token)");

                var oauthTokenInfo = JsonConvert.DeserializeObject<EsiaOAuthTokenInfo>(token);

                /*if (this.esiaClient.VerifyToken(oauthTokenInfo.AccessToken))
                {
                    throw new Exception("Указанный маркер доступа не прошел проверку подлинности");
                }*/

                this.esiaClient.UpdateClientTokenInfo(oauthTokenInfo);
            }
            catch (Exception e)
            {
                throw new Exception($"При обработке переданной информации о маркере доступа возникла ошибка: {e.Message}");
            }
        }

        /// <summary>
        /// Указать для параметров клиента адрес,
        /// на который должна вернуть ЕСИА после авторизации
        /// </summary>
        private void SetClientCallbackUriOption(DynamicDictionary operationParams)
        {
            var callbackUri = operationParams.GetAs("callbackUri", string.Empty);

            if (callbackUri.IsNotEmpty())
            {
                this.esiaClient.Options.CallbackUri = callbackUri;
            }
        }

        /// <summary>
        /// Получить идентификатор учетной записи пользователя
        /// </summary>
        private string GetPersonId()
        {
            var personId = this.esiaClient.OAuthTokenInfo.SbjId;

            if (string.IsNullOrEmpty(personId))
            {
                throw new ArgumentNullException(nameof(personId));
            }

            return personId;
        }

        /// <summary>
        /// Получить Uri контейнера ЕСИА с данными пользователя
        /// </summary>
        private string GetPersonInfoUri =>
            StringExtension.NormalizeUri(this.esiaClient.Options.RestUri, this.esiaClient.Options.PrnsRef);

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        private async Task<EsiaPersonInfo> GetPersonInfoAsync(SendType styles = SendType.Normal)
        {
            var personId = this.GetPersonId();

            var requestUri = string.Format("{0}{1}",
                this.GetPersonInfoUri,
                personId);

            var response = await this.esiaClient.GetAsync(requestUri, styles);

            return response == null ? null : new EsiaPersonInfo(JObject.Parse(response), personId);
        }

        /// <summary>
        /// Получить информацию об организация пользователя
        /// </summary>
        private async Task<IEnumerable<EsiaPersonOrganizationInfo>> GetPersonOrganizationsAsync(SendType styles = SendType.Normal)
        {
            var personId = this.GetPersonId();

            var result = new List<EsiaPersonOrganizationInfo>();

            var requestUri = string.Format("{0}{1}/{2}",
                this.GetPersonInfoUri,
                personId,
                "roles");

            var response = await this.esiaClient.GetAsync(requestUri, styles);

            if (response != null)
            {
                IDictionary<string, JToken> dictionary = JObject.Parse(response);
                if (dictionary != null && dictionary.ContainsKey("elements"))
                {
                    foreach (var jtoken in dictionary["elements"])
                    {
                        if (jtoken is JObject orgInfo)
                        {
                            result.Add(new EsiaPersonOrganizationInfo(orgInfo));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Получить информацию о контактах пользователя
        /// </summary>
        private async Task<IEnumerable<EsiaContactInfo>> GetPersonContactsAsync(SendType styles = SendType.Normal)
        {
            var personId = this.GetPersonId();

            var result = new List<EsiaContactInfo>();

            var requestUri = string.Format("{0}{1}/{2}?embed=(elements)",
                this.GetPersonInfoUri,
                personId,
                this.esiaClient.Options.CttsRef);

            var getResponse = await this.esiaClient.GetAsync(requestUri, styles);

            if (getResponse != null)
            {
                IDictionary<string, JToken> dictionary = JObject.Parse(getResponse);

                if (dictionary != null && dictionary.ContainsKey("elements"))
                {
                    foreach (var jtoken in dictionary["elements"])
                    {
                        if (jtoken is JObject contactInfo)
                        {
                            result.Add(new EsiaContactInfo(contactInfo));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Получить информацию об адресах пользователя
        /// </summary>
        private async Task<IEnumerable<EsiaAddressInfo>> GetPersonAddressesAsync(SendType styles = SendType.Normal)
        {
            var personId = this.GetPersonId();

            var result = new List<EsiaAddressInfo>();

            var requestUri = string.Format("{0}{1}/{2}?embed=(elements)",
                this.GetPersonInfoUri,
                personId,
                this.esiaClient.Options.AddrsRef);

            var response = await this.esiaClient.GetAsync(requestUri, styles);

            if (response != null)
            {
                IDictionary<string, JToken> dictionary = JObject.Parse(response);
                if (dictionary != null && dictionary.ContainsKey("elements"))
                {
                    foreach (var jObject in dictionary["elements"])
                    {
                        if (jObject is JObject addressInfo)
                        {
                            result.Add(new EsiaAddressInfo(addressInfo));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Получить информацию об организации
        /// </summary>
        /// <remarks>
        /// НЕОБХОДИМО РЕАЛИЗОВАТЬ!
        /// Перечень возможных данных в EsiaOrganizationInfo.
        /// Порядок получения информации об организации из ЕСИА:
        /// 1. Получить идентификатор организации (код доступа -> маркер доступа -> GetPersonOrganizations)
        /// 2. Повторное получения авторизационного кода и маркера доступа по нему
        /// (в scope нужно указывать перечень необходимых данных в виде:
        /// “http://esia.gosuslugi.ru/org_emps?org_oid=1000000357", где
        /// org_emps - scope набора требуемых данных, 1000000357 - идентифкатор организации в ЕСИА)
        /// 3. Запрос к ЕСИА по RestUri/OrgsRef/Идентификтаор организации
        /// </remarks>
        private async Task<EsiaOrganizationInfo> GetOrganizationInfoAsync(SendType styles = SendType.Normal)
        {
            throw new NotImplementedException();
        }
    }
}