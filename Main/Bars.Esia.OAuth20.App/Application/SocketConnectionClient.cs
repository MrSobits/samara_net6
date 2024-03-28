namespace Bars.Esia.OAuth20.App.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;

    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Entities;
    using Bars.Esia.OAuth20.App.Enums;
    using Bars.Esia.OAuth20.App.Services;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Клиент для обработки подключения по сокету
    /// </summary>
    public class SocketConnectionClient
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        private IWindsorContainer container;

        /// <summary>
        /// Логгер
        /// </summary>
        private ILogManager logManager;

        /// <summary>
        /// Обработчик подключения
        /// </summary>
        private Socket handler;

        /// <summary>
        /// Буффер сокета (для принимаемых данных)
        /// </summary>
        private byte[] socketBuffer;

        public SocketConnectionClient(Socket handler, byte[] socketBuffer, IWindsorContainer container, ILogManager logManager)
        {
            this.handler = handler;
            this.socketBuffer = socketBuffer;
            this.container = container;
            this.logManager = logManager;
        }

        /// <summary>
        /// Обработать подключение
        /// </summary>
        public void ConnectionProcess()
        {
            var data = new StringBuilder();

            do
            {
                var bytesRec = this.handler.Receive(this.socketBuffer);
                data.Append(Encoding.UTF8.GetString(this.socketBuffer, 0, bytesRec));
            }
            while (this.handler.Available != 0);

            var applicationRequest = JsonConvert.DeserializeObject<AuthAppRequest>(data.ToString());
            applicationRequest.Params = applicationRequest.Params ?? new DynamicDictionary();

            this.logManager.Info("Получено сообщение: " +
                $"MessageGuid = '{applicationRequest.MessageGuid}' " +
                $"AuthAppOperation = {applicationRequest.AuthAppOperationCode.ToString()}");

            var authAppOperationService = this.container.Resolve<IAuthAppOperationService>();

            using (this.handler)
            using (this.container.Using(authAppOperationService))
            {
                var response = new AuthAppResponse { MessageGuid = applicationRequest.MessageGuid, Success = true };

                var dict = new Dictionary<AuthAppOperationCode, Func<DynamicDictionary, object>>
                {
                    { AuthAppOperationCode.GetRedirectUri, authAppOperationService.GetRedirectUri },
                    { AuthAppOperationCode.GetOAuthToken, authAppOperationService.GetOAuthToken },
                    { AuthAppOperationCode.GetPersonInfo, authAppOperationService.GetPersonInfo },
                    { AuthAppOperationCode.GetPersonOrganizations, authAppOperationService.GetPersonOrganizations },
                    { AuthAppOperationCode.GetPersonContacts, authAppOperationService.GetPersonContacts },
                    { AuthAppOperationCode.GetPersonAddresses, authAppOperationService.GetPersonAddresses },
                    { AuthAppOperationCode.GetOrganizationInfo, authAppOperationService.GetOrganizationInfo }
                };

                try
                {
                    if (dict.TryGetValue(applicationRequest.AuthAppOperationCode, out var operationMethod))
                    {
                        response.Data = operationMethod.Invoke(applicationRequest.Params);
                    }
                    else
                    {
                        response.Success = false;
                        response.Errors = new[] { $"Операция с кодом '{(int)applicationRequest.AuthAppOperationCode}' не зарегистрирована" };
                    }
                }
                catch (Exception e)
                {
                    response.Success = false;

                    var errorStringBuilder = new StringBuilder();

                    errorStringBuilder.Append($"При выполнении метода \"{applicationRequest.AuthAppOperationCode.GetDisplayName()}\" возникла ошибка:\n {e.Message}");

                    var innerException = e.InnerException;
                    while (innerException != null)
                    {
                        errorStringBuilder.Append($" {innerException.Message}");
                        innerException = innerException?.InnerException;
                    }

                    var error = errorStringBuilder.ToString();

                    response.Errors = new[] { error };
                    this.logManager.Error($"Exception: {error}\n StackTrace: {e.StackTrace}");
                }

                var result = JsonConvert.SerializeObject(response);

                // Первые 4 байта с размером всего сообщения
                var resultBuffer = BitConverter.GetBytes(result.Length)
                    .Concat(Encoding.UTF8.GetBytes(result))
                    .ToArray();

                this.handler.Send(resultBuffer);
                this.logManager.Info("Отправлено сообщение: " +
                    $"MessageGuid = '{response.MessageGuid}' " +
                    $"Success = {response.Success} " +
                    $"Errors = {(response.Errors != null ? string.Join(", ", (string[])response.Errors) : "[]")}");

                this.handler.Shutdown(SocketShutdown.Both);
            }
        }
    }
}