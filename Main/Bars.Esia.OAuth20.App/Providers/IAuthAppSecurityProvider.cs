namespace Bars.Esia.OAuth20.App.Providers
{
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Интерфейс поставщика безопасности приложения
    /// </summary>
    public interface IAuthAppSecurityProvider
    {
        /// <summary>
        /// Получить сертификат системы
        /// </summary>
        X509Certificate2 GetSystemCertificate();

        /// <summary>
        /// Получить сертификат ЕСИА
        /// </summary>
        X509Certificate2 GetEsiaCertificate();

        /// <summary>
        /// Подписать сообщение
        /// </summary>
        byte[] SignMessage(byte[] message, X509Certificate2 certificate);

        /// <summary>
        /// Выполнить верификацию сообщения
        /// </summary>
        bool VerifyMessage(string alg, byte[] message, byte[] signature, X509Certificate2 certificate);
    }
}