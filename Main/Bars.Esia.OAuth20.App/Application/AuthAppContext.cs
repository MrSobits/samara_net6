namespace Bars.Esia.OAuth20.App.Application
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Console.CoreComponents;
    using Bars.Esia.OAuth20.App.Entities;
    using Bars.Esia.OAuth20.App.Providers;

    /// <summary>
    /// Контекст приложения
    /// </summary>
    public class AuthAppContext : ConsoleAppContext
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private ILogManager logManager;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private AuthAppOptions authAppOptions;

        /// <summary>
        /// Слушатель входящих подключений
        /// </summary>
        private Socket listener;

        /// <summary>
        /// Поток слушателя входящих подключений
        /// </summary>
        private Thread listenerThread;

        /// <summary>
        /// Признак активности потока слушателя
        /// </summary>
        private volatile bool listenerThreadIsActive;

        /// <summary>
        /// Старт контекста приложения
        /// </summary>
        protected override void StartContext()
        {
            try
            {
                this.logManager.Info("Инициализация контекста приложения");

                this.InitContext();

                var ipAddress = this.authAppOptions.SocketListeningAddress
                    ?? Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];

                var ipEndPoint = new IPEndPoint(ipAddress, this.authAppOptions.SocketListeningPort.Value);

                this.listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.listener.Bind(ipEndPoint);
                this.listener.Listen(this.authAppOptions.SocketConnectionsLength.Value);

                this.logManager.Info("Сервис авторизации запущен с настройками: " +
                    $"Адрес: {ipAddress} " +
                    $"Порт: {this.authAppOptions.SocketListeningPort} " +
                    $"Макс. количество соединений: {this.authAppOptions.SocketConnectionsLength}");
            }
            catch (Exception e)
            {
                this.StopServiceRunning($"При запуске контекста произошла ошибка:\n {e.Message}");
            }

            try
            {
                this.logManager.Info("Ожидание подключений...");
                this.listenerThreadIsActive = true;
                this.listenerThread = new Thread(this.StartSocketListening);
                this.listenerThread.Start();
            }
            catch (Exception e)
            {
                this.StopServiceRunning($"При прослушивании сокета произошла ошибка:\n {e.Message})");
            }
        }

        /// <summary>
        /// Остановка контекста приложения
        /// </summary>
        protected override void StopContext()
        {
            this.logManager.Info("Остановка сервиса...");

            this.listenerThreadIsActive = false;
            base.StopContext();

            if (this.listener != null && this.listener.Connected)
            {
                this.listener.Shutdown(SocketShutdown.Both);
            }
            this.listener?.Close();

            this.listenerThread?.Abort();
            this.listenerThread?.Join();
        }

        /// <summary>
        /// Регистрация компонентов логирования
        /// </summary>
        protected override void RegisterLogComponents()
        {
            this.Container.RegisterSingleton<ILogManager, AuthAppLogManager>();
            this.logManager = this.Container.Resolve<ILogManager>();
        }

        /// <summary>
        /// Инициализация контекста
        /// </summary>
        private void InitContext()
        {
            var authAppOptionProvider = this.Container.Resolve<IAuthAppOptionProvider>();

            var notExistOptions = authAppOptionProvider.GetNotExistRequiredOptions().ToList();

            if (notExistOptions.Any())
            {
                throw new Exception($"Для приложения не указаны следующие настройки:\n  {string.Join(",\n  ", notExistOptions)}");
            }

            this.authAppOptions = authAppOptionProvider.GetAuthAppOptions();
        }

        /// <summary>
        /// Запуск прослушивания сокета
        /// </summary>
        private void StartSocketListening()
        {
            while (this.listenerThreadIsActive)
            {
                var handler = this.listener.Accept();
                var socketBuffer = new byte[this.authAppOptions.SocketReceiveBufferLength.Value];

                var socketConnectionClient = new SocketConnectionClient(handler, socketBuffer, this.Container, this.logManager);

                var clientThread = new Thread(socketConnectionClient.ConnectionProcess);
                clientThread.Start();
            }
        }

        /// <summary>
        /// Остановить запуск сервиса
        /// </summary>
        private void StopServiceRunning(string message)
        {
            throw new Exception($"Принудительное завершение: {message}");
        }
    }
}