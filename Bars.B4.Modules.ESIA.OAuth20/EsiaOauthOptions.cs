namespace Bars.B4.Modules.ESIA.OAuth20
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;

    using Bars.B4.Config;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Castle.Windsor;

    using EsiaNET;

    public class EsiaOauthOptions
    {
        private readonly EsiaOptions Options;

        //Конфиг модуля Bars.B4.Modules.ESIA.OAuth20
        //CertificateThumbPrint - отпечаток сертификата
        //ClientId - мнемоника ИС
        //CallbackUri - адрес, на который должна вернуть ЕСИА после авторизации
        //RedirectUri - адрес перенаправления на страницу предоставления прав доступа в ЕСИА
        //TokenUri - https-адрес ЕСИА для получения маркера доступа
        //RestUri - Адрес REST-сервиса ЕСИА для получения данных

        public EsiaOauthOptions(IWindsorContainer container)
        {
            var configProvider = container.Resolve<IConfigProvider>();

            using (container.Using(configProvider))
            {

                var esiaOauthConfig = configProvider.GetConfig().ModulesConfig["Bars.B4.Modules.ESIA.OAuth20"];

                if (esiaOauthConfig == null)
                {
                    return;
                }

                var thumbprint = GetParameter(esiaOauthConfig, "CertificateThumbPrint", "отпечаток сертификата");
                var clientId = GetParameter(esiaOauthConfig, "ClientId", "мнемоника ИС");
                var callbackUri = GetParameter(esiaOauthConfig, "CallbackUri", "адрес, на который должна вернуть ЕСИА после авторизации");
                var redirectUri = GetParameter(esiaOauthConfig, "RedirectUri", "адрес перенаправления на страницу предоставления прав доступа в ЕСИА");
                var tokenUri = GetParameter(esiaOauthConfig, "TokenUri", "https-адрес ЕСИА для получения маркера доступа");
                var restUri = GetParameter(esiaOauthConfig, "RestUri", "Адрес REST-сервиса ЕСИА для получения данных");

                this.Options = new EsiaOptions
                {
                    ClientId = clientId,
                    Scope = "fullname usr_org",
                    CallbackUri = callbackUri + "oauthlogin",
                    RedirectUri = redirectUri,
                    TokenUri = tokenUri,
                    RestUri = restUri,
                    SignProvider = EsiaOptions.CreateSignProvider(() =>
                    {
                        var storeMy = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                        storeMy.Open(OpenFlags.OpenExistingOnly);

                        var regexThumbprint = Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper();

                        var certColl = storeMy.Certificates.Find(X509FindType.FindByThumbprint, regexThumbprint, false);

                        storeMy.Close();

                        if (certColl.Count == 0)
                            throw new Exception($"В личном хранилище сертификатов на локальной машине не найден сертификат с отпечатком {regexThumbprint}");
                        else if (certColl.Count > 1)
                            throw new Exception($"В личном хранилище сертификатов на локальной машине найдено {certColl.Count} сертификатов с отпечатком {regexThumbprint}");

                        return certColl[0];
                    })
                };
            }
        }

        /// <summary>
        /// Считать значение параметра из конфига
        /// </summary>
        private string GetParameter(DynamicDictionary esiaOauthConfig, string name, string desc)
        {
            var value = esiaOauthConfig[name] as string;
            if (value == null)
            {
                throw new ArgumentException($"В секции файла конфигурации модуля Bars.B4.Modules.ESIA.OAuth20 не указан {desc} {name}");
            }

            return value;
        }

        public EsiaOptions GetOptions()
        {
            if (this.Options == null)
            {
                throw new Exception("Параметры авторизации через ЕСИА с помощью OAuth 2.0 не были проинициализированны. Возможно не указана конфигурация модуля Bars.B4.Modules.ESIA.OAuth20");
            }

            return this.Options;
        }
    }
}
