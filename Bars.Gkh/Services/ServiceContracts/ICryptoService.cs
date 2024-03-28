namespace Bars.Gkh.Services.ServiceContracts
{
    using System.Threading.Tasks;

    using Bars.Gkh.Dto;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис для работы с криптографией
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Получить информацию о сертификате из подписи <see cref="CertificateDto"/>
        /// <param name="signature">Подпись</param>
        /// </summary>
        Task<CertificateDto> GetCertificateInfoAsync(string signature);

        /// <summary>
        /// Улучшить подпись CADES
        /// </summary>
        /// <param name="signature">Подпись</param>
        /// <param name="file">Файл</param>
        /// <param name="cadesTypeFrom">Исходный тип CADES</param>
        /// <param name="cadesTypeTo">Результирующий тип CADES</param>
        /// <param name="bDetached">Флаг открепленной подписи</param>
        Task<byte[]> EnhanceSignatureAsync(string signature, FileInfo file, CadesType cadesTypeFrom, CadesType cadesTypeTo, bool bDetached = true);

        /// <summary>
        /// Улучшить подпись CADES
        /// </summary>
        /// <param name="signature">Подпись</param>
        /// <param name="content">Контент</param>
        /// <param name="cadesTypeFrom">Исходный тип CADES</param>
        /// <param name="cadesTypeTo">Результирующий тип CADES</param>
        /// <param name="bDetached">Флаг открепленной подписи</param>
        Task<byte[]> EnhanceSignatureAsync(string signature, byte[] content, CadesType cadesTypeFrom, CadesType cadesTypeTo, bool bDetached = true);

        /// <summary>
        /// Создать хэш
        /// </summary>
        /// <param name="signatureAlgorithm">Тип алгоритма хэширования <see cref="SignatureAlgorithm"/></param>
        /// <param name="file">Файл</param>
        Task<byte[]> CreateHashAsync(string signatureAlgorithm, FileInfo file);
        
        /// <summary>
        /// Создать хэш
        /// </summary>
        /// <param name="signatureAlgorithm">Тип алгоритма хэширования <see cref="SignatureAlgorithm"/></param>
        /// <param name="content">Контент</param>
        Task<byte[]> CreateHashAsync(string signatureAlgorithm, byte[] content);
    }
}