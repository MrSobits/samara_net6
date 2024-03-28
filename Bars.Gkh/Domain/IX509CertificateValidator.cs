namespace Bars.Gkh.Domain
{
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Интерефейс для проверки сертификата открытого ключа пользователя
    /// </summary>
    public interface IX509CertificateValidator
    {
        bool Validate(X509Certificate2 certificate);
    }
}