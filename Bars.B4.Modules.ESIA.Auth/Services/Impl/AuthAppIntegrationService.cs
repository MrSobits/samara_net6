namespace Bars.B4.Modules.ESIA.Auth.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    using Bars.B4.Config;
    using Bars.B4.IoC;
    using Bars.B4.Modules.ESIA.Auth.Exceptions;
    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Entities;
    using Bars.Esia.OAuth20.App.Enums;

    using Castle.Core;
    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Сервис интеграции с приложением авторизации
    /// </summary>
    public class AuthAppIntegrationService : IAuthAppIntegrationService, IInitializable
    {
        /// <summary>
        /// IoC Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Конечная точка для подключения к модулю авторизации 
        /// </summary>
        private IPEndPoint authAppEndPoint;

        /// <summary>
        /// Буффер сокета
        /// </summary>
        /// <remarks>
        /// Используется в качестве месторасположения
        /// для принимаемых из подключения данных
        /// </remarks>
        private byte[] socketBuffer;

        /// <summary>
        /// Тайм-аут для метода получения
        /// ответного сообщения сервиса авторизации
        /// </summary>
        private int receiveTimeout;

        /// <inheritdoc />
        public void Initialize()
        {
            try
            {
                var configProvider = this.Container.Resolve<IConfigProvider>();

                using (this.Container.Using(configProvider))
                {
                    var config = configProvider.GetConfig().ModulesConfig["Bars.B4.Modules.ESIA.Auth"];

                    if (config == null)
                    {
                        throw new Exception("В конфигурации приложения отсутствют настройки модуля авторизации");
                    }

                    var address = this.GetParameter(config, "SocketListeningAddress", isRequired: false);
                    var port = this.GetParameter(config, "SocketListeningPort", "Порт сервиса авторизации").ToInt();

                    var bufferLength = this.GetParameter(config, "SocketReceiveBufferLength", defaultValue: 1024).ToInt();
                    this.socketBuffer = new byte[bufferLength];

                    this.receiveTimeout = this.GetParameter(config, "ReceiveTimeout", defaultValue: 60000).ToInt();

                    IPAddress ipAddress = null;

                    if (address.IsNotEmpty())
                    {
                        try
                        {
                            ipAddress = IPAddress.Parse(address);
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Приведение адреса сервиса авторизации завершилось с ошибкой: {e.Message}");
                        }
                    }
                    else
                    {
                        var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                        ipAddress = ipHostInfo.AddressList[1];
                    }

                    this.authAppEndPoint = new IPEndPoint(ipAddress, port);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"При получении параметров модуля авторизации произошла ошибка: {e.Message}");
            }
        }

        /// <inheritdoc />
        public string GetRedirectUrl(string callbackUri = null)
        {
            var request = new AuthAppRequest { AuthAppOperationCode = AuthAppOperationCode.GetRedirectUri };

            if (callbackUri.IsNotEmpty())
            {
                request.Params = new DynamicDictionary { { "callbackUri", callbackUri } };
            }

            var response = this.SendRequest(request);

            this.OperationExecutingSuccessCheck(response);

            return response.Data.ToString();
        }

        /// <inheritdoc />
        public EsiaOAuthToken GetOAuthToken(string code)
        {
            var operationParams = new DynamicDictionary { { "code", code } };

            var request = new AuthAppRequest
            {
                AuthAppOperationCode = AuthAppOperationCode.GetOAuthToken,
                Params = operationParams
            };

            var response = this.SendRequest(request);

            this.OperationExecutingSuccessCheck(response);

            return this.DeserializeOperationResult<EsiaOAuthToken>(response);
        }

        /// <inheritdoc />
        public EsiaPersonInfo GetPersonInfo(EsiaOAuthToken esiaOAuthToken)
        {
            var operationParams = new DynamicDictionary { { "token", JsonConvert.SerializeObject(esiaOAuthToken) } };

            var request = new AuthAppRequest
            {
                AuthAppOperationCode = AuthAppOperationCode.GetPersonInfo,
                Params = operationParams
            };

            var response = this.SendRequest(request);

            this.OperationExecutingSuccessCheck(response);

            return this.DeserializeOperationResult<EsiaPersonInfo>(response);
        }

        /// <inheritdoc />
        public IList<EsiaPersonOrganizationInfo> GetPersonOrganizationsInfo(EsiaOAuthToken esiaOAuthToken)
        {
            var operationParams = new DynamicDictionary { { "token", JsonConvert.SerializeObject(esiaOAuthToken) } };

            var request = new AuthAppRequest
            {
                AuthAppOperationCode = AuthAppOperationCode.GetPersonOrganizations,
                Params = operationParams
            };

            var response = this.SendRequest(request);

            this.OperationExecutingSuccessCheck(response);

            return this.DeserializeOperationResult<IList<EsiaPersonOrganizationInfo>>(response);
        }

        /// <summary>
        /// Отправить запрос сервису авторизации
        /// </summary>
        private AuthAppResponse SendRequest(AuthAppRequest request)
        {
            try
            {
                using (var sender = new Socket(this.authAppEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    try
                    {
                        sender.Connect(this.authAppEndPoint);
                    }
                    catch (Exception)
                    {
                        throw new ServiceUnavailableException("Сервис авторизации не отвечает на запросы");
                    }

                    var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));

                    sender.Send(msg);
                    sender.ReceiveTimeout = this.receiveTimeout;

                    var res = new StringBuilder();
                    var allPacketBytes = 0;
                    var readedPacketBytes = 0;
                    var currentReceivedBytes = 0;

                    do
                    {
                        // Получение размера всего сообщения
                        if (allPacketBytes == 0)
                        {
                            var allPacketBytesBuffer = new byte[4];
                            sender.Receive(allPacketBytesBuffer);
                            allPacketBytes = BitConverter.ToInt32(allPacketBytesBuffer, 0);
                        }
                        // Поэтапное получения всего сообщения в рамках одного или нескольких пакетов
                        else
                        {
                            currentReceivedBytes = sender.Receive(this.socketBuffer);
                            readedPacketBytes += currentReceivedBytes;
                            res.Append(Encoding.UTF8.GetString(this.socketBuffer, 0, currentReceivedBytes));
                        }
                    }
                    while (allPacketBytes != 0 && readedPacketBytes < allPacketBytes);

                    sender.Shutdown(SocketShutdown.Both);

                    return JsonConvert.DeserializeObject<AuthAppResponse>(res.ToString());
                }
            }
            catch (Exception e)
            {
                throw new BadGatewayException($"При отправке запроса к сервису авторизации произошла ошибка: {e.Message}");
            }
        }

        /// <summary>
        /// Получить параметр модуля
        /// </summary>
        private string GetParameter(DynamicDictionary config, string name, string description = "", object defaultValue = null, bool isRequired = true)
        {
            var value = (config.Get(name) ?? defaultValue)?.ToString();

            if (value.IsEmpty() && isRequired)
            {
                throw new ServiceUnavailableException($"В секции файла конфигурации модуля Bars.B4.Modules.ESIA.Auth не указан {description} ({name})");
            }

            return value;
        }

        /// <summary>
        /// Проверка успешности выполнения операции
        /// </summary>
        private void OperationExecutingSuccessCheck(AuthAppResponse response)
        {
            if (!response.Success)
            {
                var errors = JsonConvert.DeserializeObject<string[]>(response.Errors.ToString());

                throw new BadGatewayException($"Запрос к сервису авторизации завершился с ошибками: {string.Join("", errors)}");
            }
        }

        /// <summary>
        /// Десериализировать результат выполнения операции
        /// </summary>
        private T DeserializeOperationResult<T>(AuthAppResponse response)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(response.Data.ToString());
            }
            catch (Exception e)
            {
                throw new Exception($"При приведении результата ответа на запрос к сервису авторизации произошла ошибка: {e.Message}");
            }
        }
    }
}